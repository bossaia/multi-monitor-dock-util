namespace multi_monitor_dock_util
{
    partial class Preferences
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.PrimaryMonitorRight = new System.Windows.Forms.RadioButton();
            this.PrimaryMonitorLeft = new System.Windows.Forms.RadioButton();
            this.AutoStartCheckbox = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(47, 107);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 0;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(128, 107);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 1;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.PrimaryMonitorRight);
            this.groupBox1.Controls.Add(this.PrimaryMonitorLeft);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(189, 39);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Primary Monitor";
            // 
            // PrimaryMonitorRight
            // 
            this.PrimaryMonitorRight.AutoSize = true;
            this.PrimaryMonitorRight.Location = new System.Drawing.Point(108, 16);
            this.PrimaryMonitorRight.Name = "PrimaryMonitorRight";
            this.PrimaryMonitorRight.Size = new System.Drawing.Size(74, 17);
            this.PrimaryMonitorRight.TabIndex = 1;
            this.PrimaryMonitorRight.TabStop = true;
            this.PrimaryMonitorRight.Text = "Right Side";
            this.PrimaryMonitorRight.UseVisualStyleBackColor = true;
            // 
            // PrimaryMonitorLeft
            // 
            this.PrimaryMonitorLeft.AutoSize = true;
            this.PrimaryMonitorLeft.Location = new System.Drawing.Point(17, 16);
            this.PrimaryMonitorLeft.Name = "PrimaryMonitorLeft";
            this.PrimaryMonitorLeft.Size = new System.Drawing.Size(67, 17);
            this.PrimaryMonitorLeft.TabIndex = 0;
            this.PrimaryMonitorLeft.TabStop = true;
            this.PrimaryMonitorLeft.Text = "Left Side";
            this.PrimaryMonitorLeft.UseVisualStyleBackColor = true;
            // 
            // AutoStartCheckbox
            // 
            this.AutoStartCheckbox.AutoSize = true;
            this.AutoStartCheckbox.Location = new System.Drawing.Point(21, 69);
            this.AutoStartCheckbox.Name = "AutoStartCheckbox";
            this.AutoStartCheckbox.Size = new System.Drawing.Size(153, 17);
            this.AutoStartCheckbox.TabIndex = 7;
            this.AutoStartCheckbox.Text = "Autostart on system startup";
            this.AutoStartCheckbox.UseVisualStyleBackColor = true;
            // 
            // Preferences
            // 
            this.AcceptButton = this.OKBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(215, 142);
            this.Controls.Add(this.AutoStartCheckbox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Preferences";
            this.Text = "Preferences";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton PrimaryMonitorRight;
        private System.Windows.Forms.RadioButton PrimaryMonitorLeft;
        private System.Windows.Forms.CheckBox AutoStartCheckbox;
    }
}