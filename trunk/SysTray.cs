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
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace multi_monitor_dock_util
{
    public class SysTray: Form
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SysTray());
        }

        private readonly NotifyIcon _TrayIcon;
        private readonly ContextMenu _TrayMenu;
        private Main _MainForm = null;
        private static object _SyncRoot = new object();

        public SysTray()
        {
            _TrayMenu = new ContextMenu();
            //_TrayMenu.MenuItems.Add("Fix Display Settings", OnFixDisplay);
            //_TrayMenu.MenuItems.Add("-");
            _TrayMenu.MenuItems.Add("Restore", OnRestore);
            _TrayMenu.MenuItems.Add("Exit", OnExit);

            _TrayIcon = new NotifyIcon
                           {
                               Text = "Multi-Monitor Dock Utility",
                               Icon = new Icon(Icons.TrayIcon, 40, 40),
                               ContextMenu = _TrayMenu,
                               Visible = true
                           };
            _TrayIcon.MouseDoubleClick += OnRestore;

            SystemEvents.DisplaySettingsChanged += OnDisplaySettingsChanged;
        }

        private void OpenMainDialog()
        {
            if (!Monitor.TryEnter(_SyncRoot) || _MainForm != null) return;
            try
            {
                _MainForm = new Main();
            }
            finally
            {
                Monitor.Exit(_SyncRoot);
            }

            _MainForm.ShowDialog();
            _MainForm.Close();
            _MainForm = null;
        }

        #region Event Handlers
        protected override void OnLoad(EventArgs e)
        {
            Visible = false;
            ShowInTaskbar = false;
            
            base.OnLoad(e);
        }

        private void OnRestore(object sender, EventArgs e)
        {
            OpenMainDialog();
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        private void OnDisplaySettingsChanged(object sender, EventArgs e)
        {
            OpenMainDialog();
        }

        private void OnFixDisplay(object sender, EventArgs e)
        {
            
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _TrayIcon.Dispose();
            }
            
            base.Dispose(disposing);
        }
    }
}
