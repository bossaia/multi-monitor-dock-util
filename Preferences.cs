using System;
using System.Windows.Forms;

namespace multi_monitor_dock_util
{
    public partial class Preferences : Form
    {
        public Preferences()
        {
            InitializeComponent();

            PrimaryMonitorLeft.Checked = PreferenceSettings.PrimaryMonitor ==
                                         PreferenceSettings.PrimaryMonitorEnum.LeftSide;
            PrimaryMonitorRight.Checked = PreferenceSettings.PrimaryMonitor ==
                                          PreferenceSettings.PrimaryMonitorEnum.RightSide;
            AutoStartCheckbox.Checked = PreferenceSettings.AutostartOnSystemStartup;
            MinimizeOnStartCheckbox.Checked = PreferenceSettings.MinimizeOnStart;
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            PreferenceSettings.AutostartOnSystemStartup = AutoStartCheckbox.Checked;
            PreferenceSettings.MinimizeOnStart = MinimizeOnStartCheckbox.Checked;
            PreferenceSettings.PrimaryMonitor = PrimaryMonitorLeft.Checked
                                                    ? PreferenceSettings.PrimaryMonitorEnum.LeftSide
                                                    : PreferenceSettings.PrimaryMonitorEnum.RightSide;
                

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
