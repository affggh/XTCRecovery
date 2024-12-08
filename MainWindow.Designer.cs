namespace XTCRecovery
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            AntdUI.StepsItem stepsItem7 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem8 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem9 = new AntdUI.StepsItem();
            this.windowBar1 = new AntdUI.WindowBar();
            this.refreshButton = new AntdUI.Button();
            this.deviceBadge = new AntdUI.Badge();
            this.InfoButton = new AntdUI.Button();
            this.LogInput = new AntdUI.Input();
            this.StatusBadge = new AntdUI.Badge();
            this.battery1 = new AntdUI.Battery();
            this.downloadButton = new AntdUI.Button();
            this.ProgressBar = new AntdUI.Progress();
            this.FlashSteps = new AntdUI.Steps();
            this.modelSelect = new AntdUI.Select();
            this.uploadDragger = new AntdUI.UploadDragger();
            this.labelTime1 = new AntdUI.LabelTime();
            this.button1 = new AntdUI.Button();
            this.button2 = new AntdUI.Button();
            this.button3 = new AntdUI.Button();
            this.versionSelect = new AntdUI.Select();
            this.flashButton = new AntdUI.Button();
            this.image3D1 = new AntdUI.Image3D();
            this.windowBar1.SuspendLayout();
            this.SuspendLayout();
            // 
            // windowBar1
            // 
            this.windowBar1.BackColor = System.Drawing.Color.Transparent;
            this.windowBar1.BadgeOffsetX = 0;
            this.windowBar1.BadgeOffsetY = 0;
            this.windowBar1.BadgeSize = 0F;
            this.windowBar1.Controls.Add(this.refreshButton);
            this.windowBar1.Controls.Add(this.deviceBadge);
            this.windowBar1.Controls.Add(this.InfoButton);
            this.windowBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.windowBar1.ForeColor = System.Drawing.SystemColors.Window;
            this.windowBar1.Icon = global::XTCRecovery.Properties.Resources.icon;
            this.windowBar1.IconSvg = resources.GetString("windowBar1.IconSvg");
            this.windowBar1.Location = new System.Drawing.Point(0, 0);
            this.windowBar1.Margin = new System.Windows.Forms.Padding(0);
            this.windowBar1.MaximizeBox = false;
            this.windowBar1.Name = "windowBar1";
            this.windowBar1.Size = new System.Drawing.Size(781, 54);
            this.windowBar1.TabIndex = 0;
            this.windowBar1.Text = "低仿小天才超级恢复";
            // 
            // refreshButton
            // 
            this.refreshButton.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.refreshButton.Ghost = true;
            this.refreshButton.IconSvg = resources.GetString("refreshButton.IconSvg");
            this.refreshButton.Location = new System.Drawing.Point(554, 2);
            this.refreshButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Shape = AntdUI.TShape.Circle;
            this.refreshButton.Size = new System.Drawing.Size(49, 50);
            this.refreshButton.TabIndex = 2;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // deviceBadge
            // 
            this.deviceBadge.Location = new System.Drawing.Point(489, 2);
            this.deviceBadge.Margin = new System.Windows.Forms.Padding(4);
            this.deviceBadge.Name = "deviceBadge";
            this.deviceBadge.Size = new System.Drawing.Size(72, 50);
            this.deviceBadge.State = AntdUI.TState.Warn;
            this.deviceBadge.TabIndex = 1;
            this.deviceBadge.Text = "未连接";
            // 
            // InfoButton
            // 
            this.InfoButton.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.InfoButton.Ghost = true;
            this.InfoButton.IconSvg = resources.GetString("InfoButton.IconSvg");
            this.InfoButton.Location = new System.Drawing.Point(609, 2);
            this.InfoButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.InfoButton.Name = "InfoButton";
            this.InfoButton.Shape = AntdUI.TShape.Circle;
            this.InfoButton.Size = new System.Drawing.Size(49, 50);
            this.InfoButton.TabIndex = 0;
            this.InfoButton.Click += new System.EventHandler(this.InfoButton_Click);
            // 
            // LogInput
            // 
            this.LogInput.AutoScroll = true;
            this.LogInput.Font = new System.Drawing.Font("Iosevka Extended", 9F);
            this.LogInput.Location = new System.Drawing.Point(395, 259);
            this.LogInput.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.LogInput.Multiline = true;
            this.LogInput.Name = "LogInput";
            this.LogInput.Size = new System.Drawing.Size(361, 152);
            this.LogInput.TabIndex = 1;
            this.LogInput.Text = "日志将会显示在这里...\r\n选择本地rom中的rawprogram0.xml或拖入rom中的这个文件可以强制刷入这个rom\r\n\r\n作者：affggh 和 早茶光";
            // 
            // StatusBadge
            // 
            this.StatusBadge.Location = new System.Drawing.Point(32, 302);
            this.StatusBadge.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.StatusBadge.Name = "StatusBadge";
            this.StatusBadge.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.StatusBadge.Size = new System.Drawing.Size(285, 28);
            this.StatusBadge.State = AntdUI.TState.Warn;
            this.StatusBadge.TabIndex = 3;
            this.StatusBadge.Text = "确保你的设备至少有20%电量剩余";
            // 
            // battery1
            // 
            this.battery1.Location = new System.Drawing.Point(325, 302);
            this.battery1.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.battery1.Name = "battery1";
            this.battery1.Size = new System.Drawing.Size(54, 28);
            this.battery1.TabIndex = 4;
            this.battery1.Text = "battery1";
            this.battery1.Value = 20;
            // 
            // downloadButton
            // 
            this.downloadButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.downloadButton.Font = new System.Drawing.Font("Microsoft YaHei UI", 16F);
            this.downloadButton.IconHoverSvg = "";
            this.downloadButton.IconSvg = resources.GetString("downloadButton.IconSvg");
            this.downloadButton.Location = new System.Drawing.Point(32, 334);
            this.downloadButton.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.downloadButton.Name = "downloadButton";
            this.downloadButton.Shape = AntdUI.TShape.Round;
            this.downloadButton.Size = new System.Drawing.Size(173, 47);
            this.downloadButton.TabIndex = 5;
            this.downloadButton.Text = "下载";
            this.downloadButton.Type = AntdUI.TTypeMini.Primary;
            this.downloadButton.Click += new System.EventHandler(this.DownloadButton_Click);
            // 
            // ProgressBar
            // 
            this.ProgressBar.ContainerControl = this;
            this.ProgressBar.Location = new System.Drawing.Point(37, 382);
            this.ProgressBar.Margin = new System.Windows.Forms.Padding(4, 18, 4, 18);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(349, 28);
            this.ProgressBar.TabIndex = 6;
            this.ProgressBar.Text = "progress1";
            // 
            // FlashSteps
            // 
            this.FlashSteps.BadgeAlign = AntdUI.TAlignFrom.Bottom;
            this.FlashSteps.BadgeSize = 0.4F;
            stepsItem7.Description = "从网络下载rom";
            stepsItem7.Name = "";
            stepsItem7.Title = "下载";
            stepsItem8.Description = "解压刷机包";
            stepsItem8.Title = "解压";
            stepsItem9.Description = "时间可能会比较长";
            stepsItem9.Title = "刷入";
            this.FlashSteps.Items.Add(stepsItem7);
            this.FlashSteps.Items.Add(stepsItem8);
            this.FlashSteps.Items.Add(stepsItem9);
            this.FlashSteps.Location = new System.Drawing.Point(395, 62);
            this.FlashSteps.Margin = new System.Windows.Forms.Padding(9, 8, 9, 8);
            this.FlashSteps.Name = "FlashSteps";
            this.FlashSteps.Size = new System.Drawing.Size(166, 188);
            this.FlashSteps.TabIndex = 7;
            this.FlashSteps.Tag = "";
            this.FlashSteps.Text = "steps1";
            this.FlashSteps.Vertical = true;
            // 
            // modelSelect
            // 
            this.modelSelect.Location = new System.Drawing.Point(37, 62);
            this.modelSelect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.modelSelect.Name = "modelSelect";
            this.modelSelect.Size = new System.Drawing.Size(173, 55);
            this.modelSelect.TabIndex = 8;
            this.modelSelect.Text = "选择设备型号";
            // 
            // uploadDragger
            // 
            this.uploadDragger.Location = new System.Drawing.Point(39, 122);
            this.uploadDragger.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.uploadDragger.Name = "uploadDragger";
            this.uploadDragger.Size = new System.Drawing.Size(171, 175);
            this.uploadDragger.TabIndex = 9;
            this.uploadDragger.Text = "rawprogram0.xml";
            // 
            // labelTime1
            // 
            this.labelTime1.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelTime1.Location = new System.Drawing.Point(571, 62);
            this.labelTime1.Name = "labelTime1";
            this.labelTime1.Size = new System.Drawing.Size(185, 46);
            this.labelTime1.TabIndex = 10;
            this.labelTime1.Text = "labelTime1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(571, 114);
            this.button1.Name = "button1";
            this.button1.Shape = AntdUI.TShape.Round;
            this.button1.Size = new System.Drawing.Size(185, 41);
            this.button1.TabIndex = 11;
            this.button1.Text = "加群";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(571, 161);
            this.button2.Name = "button2";
            this.button2.Shape = AntdUI.TShape.Round;
            this.button2.Size = new System.Drawing.Size(185, 41);
            this.button2.TabIndex = 12;
            this.button2.Text = "加密/解密";
            // 
            // button3
            // 
            this.button3.LoadingWaveColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.button3.Location = new System.Drawing.Point(571, 208);
            this.button3.Name = "button3";
            this.button3.Shape = AntdUI.TShape.Round;
            this.button3.Size = new System.Drawing.Size(185, 41);
            this.button3.TabIndex = 13;
            this.button3.Text = "刷新设备型号";
            // 
            // versionSelect
            // 
            this.versionSelect.Location = new System.Drawing.Point(216, 63);
            this.versionSelect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.versionSelect.Name = "versionSelect";
            this.versionSelect.Size = new System.Drawing.Size(166, 55);
            this.versionSelect.TabIndex = 14;
            this.versionSelect.Text = "选择ROM版本";
            // 
            // flashButton
            // 
            this.flashButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.flashButton.Enabled = false;
            this.flashButton.Font = new System.Drawing.Font("Microsoft YaHei UI", 16F);
            this.flashButton.IconHoverSvg = "";
            this.flashButton.IconSvg = resources.GetString("flashButton.IconSvg");
            this.flashButton.Location = new System.Drawing.Point(213, 334);
            this.flashButton.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.flashButton.Name = "flashButton";
            this.flashButton.Shape = AntdUI.TShape.Round;
            this.flashButton.Size = new System.Drawing.Size(173, 47);
            this.flashButton.TabIndex = 15;
            this.flashButton.Text = "刷入";
            this.flashButton.Type = AntdUI.TTypeMini.Success;
            // 
            // image3D1
            // 
            this.image3D1.Image = global::XTCRecovery.Properties.Resources.logo1;
            this.image3D1.Location = new System.Drawing.Point(216, 123);
            this.image3D1.Name = "image3D1";
            this.image3D1.Radius = 10;
            this.image3D1.Size = new System.Drawing.Size(163, 174);
            this.image3D1.TabIndex = 16;
            this.image3D1.Text = "image3D1";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(781, 429);
            this.ControlBox = false;
            this.Controls.Add(this.image3D1);
            this.Controls.Add(this.flashButton);
            this.Controls.Add(this.versionSelect);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.labelTime1);
            this.Controls.Add(this.uploadDragger);
            this.Controls.Add(this.modelSelect);
            this.Controls.Add(this.FlashSteps);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.downloadButton);
            this.Controls.Add(this.battery1);
            this.Controls.Add(this.StatusBadge);
            this.Controls.Add(this.LogInput);
            this.Controls.Add(this.windowBar1);
            this.Dark = true;
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Mode = AntdUI.TAMode.Dark;
            this.Name = "MainWindow";
            this.Resizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.windowBar1.ResumeLayout(false);
            this.windowBar1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.WindowBar windowBar1;
        private AntdUI.Input LogInput;
        private AntdUI.Badge StatusBadge;
        private AntdUI.Battery battery1;
        private AntdUI.Button downloadButton;
        private AntdUI.Progress ProgressBar;
        private AntdUI.Steps FlashSteps;
        private AntdUI.UploadDragger uploadDragger;
        private AntdUI.Select modelSelect;
        private AntdUI.Button InfoButton;
        private AntdUI.Badge deviceBadge;
        private AntdUI.Button refreshButton;
        private AntdUI.LabelTime labelTime1;
        private AntdUI.Button button3;
        private AntdUI.Button button2;
        private AntdUI.Button button1;
        private AntdUI.Select versionSelect;
        private AntdUI.Button flashButton;
        private AntdUI.Image3D image3D1;
    }
}
