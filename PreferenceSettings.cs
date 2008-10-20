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
using Microsoft.Win32;

namespace multi_monitor_dock_util
{
    public class PreferenceSettings
    {
        public enum PrimaryMonitorEnum
        {
            LeftSide = 0,
            RightSide = 1
        }

        private const string AppKey = @"Software\MultiMonitorDockUtil";
        private const string AutoRunKey = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string RegValPrimaryMonitor = "PrimaryMonitor";
        private const string RegValAutoStart = "AutoStart";
        private const string RegValAutoRun = "MultiMonitorDockUtil";
        private const string RegValMinimizeOnStart = "MinimizeOnStart";

        public static PrimaryMonitorEnum PrimaryMonitor
        {
            get
            {
                RegistryKey key = GetApplicationRegistryKey();
                return (PrimaryMonitorEnum)key.GetValue(RegValPrimaryMonitor, PrimaryMonitorEnum.LeftSide);
            }
            set
            {
                RegistryKey key = GetApplicationRegistryKey();
                key.SetValue(RegValPrimaryMonitor, value, RegistryValueKind.DWord);
            }
        }

        public static bool AutostartOnSystemStartup
        {
            get 
            { 
                RegistryKey key = GetApplicationRegistryKey();
                return Convert.ToBoolean(key.GetValue(RegValAutoStart, false));
            }
            set
            {
                RegistryKey key = GetApplicationRegistryKey();
                key.SetValue(RegValAutoStart, value, RegistryValueKind.DWord);

                key = GetAutoRunRegistryKey();
                if (value)
                    key.SetValue(RegValAutoRun, Environment.GetCommandLineArgs()[0], RegistryValueKind.String);
                else
                    key.DeleteValue(RegValAutoRun, false);
            }
        }

        public static bool MinimizeOnStart
        {
            get
            {
                RegistryKey key = GetApplicationRegistryKey();
                return Convert.ToBoolean(key.GetValue(RegValMinimizeOnStart, false));
            }
            set
            {
                RegistryKey key = GetApplicationRegistryKey();
                key.SetValue(RegValMinimizeOnStart, value, RegistryValueKind.DWord);
            }
        }

        private static RegistryKey GetApplicationRegistryKey()
        {
            RegistryKey HKCU = Registry.CurrentUser;
            RegistryKey key = HKCU.OpenSubKey(AppKey, true);

            /* Key doesn't exist, so we need to create it */
            if (key == null)
            {
                key = HKCU.CreateSubKey(AppKey);
                if (key == null)
                    throw new Exception(String.Format("Could not create registry key HKLM\\{0}", AppKey));
            }

            return key;
        }
        
        private static RegistryKey GetAutoRunRegistryKey()
        {
            RegistryKey HKCU = Registry.CurrentUser;
            RegistryKey key = HKCU.OpenSubKey(AutoRunKey, true);
            return key;
        }
    }
}
