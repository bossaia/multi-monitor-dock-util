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
using System.Text;
using System.Windows.Forms;

namespace multi_monitor_dock_util
{

    public partial class Main : Form
    {
        private List<Display.Monitor> _Monitors;

        public Main()
        {
            InitializeComponent();

            EnumerateMonitors();
        }

        private void DisplayDebugInfo()
        {
            StringBuilder debug = new StringBuilder();

            foreach (Display.Monitor monitor in _Monitors)
            {
                Win32.DISPLAY_DEVICE device = monitor.Device;

                debug.AppendFormat("DeviceName:   {0}{1}", device.DeviceName, Environment.NewLine);
                debug.AppendFormat("DeviceString: {0}{1}", device.DeviceString, Environment.NewLine);
                debug.AppendFormat("DeviceString: {0}{1}", device.StateFlags, Environment.NewLine);
                debug.AppendFormat("DeviceID:     {0}{1}", device.DeviceID, Environment.NewLine);
                debug.AppendFormat("DeviceKey:    {0}{1}", device.DeviceKey, Environment.NewLine);

                Win32.DEVMODE currMode = monitor.CurrentDisplaySettings;
                debug.AppendFormat("Resolution:   {0}x{1}x{2}{3}", currMode.dmPelsWidth, currMode.dmPelsHeight,
                                   currMode.dmBitsPerPel, Environment.NewLine);
                debug.AppendFormat("Refresh Rate: {0}hz{1}", currMode.dmDisplayFrequency, Environment.NewLine);
                debug.AppendFormat("Position:{0}", Environment.NewLine);
                debug.AppendFormat("  X: {0}{1}", currMode.dmPositionX, Environment.NewLine);
                debug.AppendFormat("  Y: {0}{1}", currMode.dmPositionY, Environment.NewLine);

                debug.AppendFormat("Available Modes: {0}", Environment.NewLine);

                foreach (Win32.DEVMODE mode in monitor.AvailableDisplaySettings)
                {
                    Win32.DEVMODE curr = monitor.CurrentDisplaySettings;
                    bool selected = false;

                    if (mode.dmPelsWidth == curr.dmPelsWidth
                        && mode.dmPelsHeight == curr.dmPelsHeight
                        && mode.dmPelsWidth == curr.dmPelsWidth
                        && mode.dmBitsPerPel == curr.dmBitsPerPel
                        && mode.dmDisplayFrequency == curr.dmDisplayFrequency)
                        selected = true;

                    debug.AppendFormat("    {0}x{1}x{2} {3}hz {4}:{5} {6}{7}", mode.dmPelsWidth,
                                       mode.dmPelsHeight, mode.dmBitsPerPel, mode.dmDisplayFrequency, mode.dmPositionX,
                                       mode.dmPositionY, selected ? "<--" : "", Environment.NewLine);
                }

                debug.Append(Environment.NewLine);
            }
            DebugText.Text = debug.ToString();
        }

        private void EnumerateMonitors()
        {
            _Monitors = Display.EnumerateMonitors();
            DisplayDebugInfo();
        }

        private void SwapMonitors()
        {
            if (_Monitors.Count < 2)
            {
                MessageBox.Show("Sorry, you don't even have two monitors to swap.");
                return;
            }

            if (_Monitors.Count > 2)
            {
                MessageBox.Show("How do you swap more than two monitors?  I don't know.");
                return;
            }

            Display.SwapMonitors(_Monitors[0], _Monitors[1]);

        }

        private void OnDisplaySettingsChanged(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }


        private void Main_Load(object sender, EventArgs e)
        {
            Microsoft.Win32.SystemEvents.DisplaySettingsChanged += OnDisplaySettingsChanged;
        }

        private void YesButton_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void NoButton_Click(object sender, EventArgs e)
        {
            EnumerateMonitors();
            SwapMonitors();

            /* Refresh our settings */
            EnumerateMonitors();
        }


        private void Main_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
                Hide();
        }

        private void systemTrayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var preferencesDlg = new Preferences();
            preferencesDlg.ShowDialog();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DebugText.Visible = !DebugText.Visible;
            MenuItemShowDebug.Text = DebugText.Visible ? "Hide Debug Info" : "Show Debug Info";
        }
    }
}
