using System;
using System.Reflection;
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
