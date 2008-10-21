/*
 * Copyright (c) 2008, Hans Van Slooten
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without 
 * modification, are permitted provided that the following conditions 
 * are met:
 * 
 * * Redistributions of source code must retain the above copyright 
 *   notice, this list of conditions and the following disclaimer.
 * * Redistributions in binary form must reproduce the above copyright 
 *   notice, this list of conditions and the following disclaimer in 
 *   the documentation and/or other materials provided with the 
 *   distribution.
 * * Neither the name of Hans Van Slooten nor the names of its 
 *   contributors may be used to endorse or promote products derived 
 *   from this software without specific prior written permission.
 *   
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS 
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT 
 * LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS 
 * FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE 
 * COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, 
 * BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; 
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER 
 * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT 
 * LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
 * ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
 * POSSIBILITY OF SUCH DAMAGE.
 * 
 */
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace multi_monitor_dock_util
{
    public class Display
    {
        public class Monitor
        {
            public bool IsPrimary { get; private set; }
            public Win32.DEVMODE CurrentDisplaySettings { get; private set; }
            public List<Win32.DEVMODE> AvailableDisplaySettings { get; private set; }
            public Win32.DISPLAY_DEVICE Device { get; private set; }

            public Monitor(Win32.DISPLAY_DEVICE device, Win32.DEVMODE currentDisplaySettings, List<Win32.DEVMODE> availableDisplaySettings)
            {
                Device = device;
                AvailableDisplaySettings = availableDisplaySettings;
                CurrentDisplaySettings = currentDisplaySettings;
                IsPrimary = CurrentDisplaySettings.dmPositionX == 0 && CurrentDisplaySettings.dmPositionY == 0;
            }

            public Win32.DEVMODE GetCurrentDisplaySettingsStructure()
            {
                var displaySettings = new Win32.DEVMODE();

                displaySettings.dmSize = (short) Marshal.SizeOf(displaySettings);
                //displaySettings.dmPelsWidth = CurrentDisplaySettings.dmPelsWidth;
                //displaySettings.dmPelsHeight = CurrentDisplaySettings.dmPelsHeight;
                //displaySettings.dmDisplayFlags = CurrentDisplaySettings.dmDisplayFlags;
                //displaySettings.dmDisplayFrequency = CurrentDisplaySettings.dmDisplayFrequency;
                displaySettings.dmPositionX = CurrentDisplaySettings.dmPositionX;
                displaySettings.dmPositionY = CurrentDisplaySettings.dmPositionY;
                displaySettings.dmFields = (int)Win32.DM_POSITION;
                return displaySettings;
            }
        }

        public static List<Monitor> EnumerateMonitors()
        {
            List<Monitor> monitors = new List<Monitor>();

            var displayDevices = EnumerateDesktopDisplayDevices();

            foreach (var displayDevice in displayDevices)
            {
                var availableDisplaySettings = EnumerateAllDisplaySettings(displayDevice.DeviceName);
                var currentDisplaySettings = EnumerateCurrentDisplaySettings(displayDevice.DeviceName);
                var monitor = new Monitor(displayDevice, currentDisplaySettings, availableDisplaySettings);

                monitors.Add(monitor);
            }

            return monitors;
        }

        public static void SwapMonitors(Monitor monitor0, Monitor monitor1)
        {
            Monitor primary = monitor0.IsPrimary ? monitor0 : monitor1;
            Monitor secondary = monitor0.IsPrimary ? monitor1 : monitor0;
            Win32.DEVMODE primarySettings = primary.GetCurrentDisplaySettingsStructure();
            Win32.DEVMODE secondarySettings = secondary.GetCurrentDisplaySettingsStructure();

            const uint dwFlags = Win32.CDS_UPDATEREGISTRY | Win32.CDS_NORESET;
            
            if (PreferenceSettings.PrimaryMonitor == PreferenceSettings.PrimaryMonitorEnum.LeftSide)
            {
                primarySettings.dmPositionX = secondary.CurrentDisplaySettings.dmPelsWidth;
                secondarySettings.dmPositionX = 0;

                Win32.ChangeDisplaySettingsEx(primary.Device.DeviceName, ref primarySettings, (IntPtr) null,
                                              dwFlags, (IntPtr) null);
                Win32.ChangeDisplaySettingsEx(secondary.Device.DeviceName, ref secondarySettings, (IntPtr) null,
                                              dwFlags | Win32.CDS_SET_PRIMARY, (IntPtr) null);
            }
            else
            {
                primarySettings.dmPositionX = -primary.CurrentDisplaySettings.dmPelsWidth;
                secondarySettings.dmPositionX = 0;
                Win32.ChangeDisplaySettingsEx(secondary.Device.DeviceName, ref secondarySettings, (IntPtr) null,
                                              dwFlags, (IntPtr) null);
                Win32.ChangeDisplaySettingsEx(primary.Device.DeviceName, ref primarySettings, (IntPtr) null,
                                              dwFlags | Win32.CDS_SET_PRIMARY, (IntPtr) null);

            }
            
            Win32.ChangeDisplaySettingsEx(null, null, (IntPtr)null, 0, (IntPtr)null);
        }

        private static List<Win32.DISPLAY_DEVICE> EnumerateDesktopDisplayDevices()
        {
            var displayDevices = new List<Win32.DISPLAY_DEVICE>();

            /* Keep looping through device indicies with i until we are done
             */
            for (uint i = 0; ; i++)
            {
                Win32.DISPLAY_DEVICE displayDevice = new Win32.DISPLAY_DEVICE();
                const uint displayFlags = Win32.EDD_GET_DEVICE_INTERFACE_NAME;
                displayDevice.cb = Marshal.SizeOf(displayDevice);

                bool deviceAvailable = Win32.EnumDisplayDevices(null, i, ref displayDevice, displayFlags);

                /* If EnumDisplayDevices returns 'false' for a given device number, i, then
                 * there are no more devices.
                 */
                if (!deviceAvailable) break;

                /* Only retrieve monitors that are actually attached to the desktop
                 */
                if ((displayDevice.StateFlags & Win32.DISPLAY_DEVICE_ATTACHED_TO_DESKTOP) ==
                    Win32.DISPLAY_DEVICE_ATTACHED_TO_DESKTOP)
                {
                    displayDevices.Add(displayDevice);
                }
            }

            return displayDevices;
        }

        private static List<Win32.DEVMODE> EnumerateAllDisplaySettings(string deviceName)
        {
            var displaySettings = new List<Win32.DEVMODE>();

            for (uint i = 0; ; i++)
            {
                Win32.DEVMODE deviceMode = new Win32.DEVMODE();
                deviceMode.dmSize = (short)Marshal.SizeOf(deviceMode);

                bool success = Win32.EnumDisplaySettingsEx(deviceName, i, ref deviceMode, 0);

                if (!success) break;

                displaySettings.Add(deviceMode);
            }

            return displaySettings;
        }

        private static Win32.DEVMODE EnumerateCurrentDisplaySettings(string deviceName)
        {
            Win32.DEVMODE deviceMode = new Win32.DEVMODE();
            deviceMode.dmSize = (short) Marshal.SizeOf(deviceMode);

            bool success = Win32.EnumDisplaySettingsEx(deviceName, Win32.ENUM_CURRENT_SETTINGS, ref deviceMode, 0);

            if (!success) throw new Exception("Unable to retrieve the current display settings.");
            
            return deviceMode;
        }

    }
}
