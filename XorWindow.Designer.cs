namespace XTCRecovery
{
    partial class XorWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XorWindow));
            this.windowBar1 = new AntdUI.WindowBar();
            this.uploadDragger = new AntdUI.UploadDragger();
            this.SuspendLayout();
            // 
            // windowBar1
            // 
            this.windowBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.windowBar1.IconSvg = resources.GetString("windowBar1.IconSvg");
            this.windowBar1.Location = new System.Drawing.Point(0, 0);
            this.windowBar1.MaximizeBox = false;
            this.windowBar1.Name = "windowBar1";
            this.windowBar1.Size = new System.Drawing.Size(403, 56);
            this.windowBar1.TabIndex = 0;
            this.windowBar1.Text = "XTC 加密/解密工具";
            // 
            // uploadDragger
            // 
            this.uploadDragger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uploadDragger.Location = new System.Drawing.Point(0, 56);
            this.uploadDragger.Name = "uploadDragger";
            this.uploadDragger.Padding = new System.Windows.Forms.Padding(16);
            this.uploadDragger.Size = new System.Drawing.Size(403, 321);
            this.uploadDragger.TabIndex = 1;
            this.uploadDragger.Text = "点击选择或拖入到这里";
            // 
            // XorWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 377);
            this.Controls.Add(this.uploadDragger);
            this.Controls.Add(this.windowBar1);
            this.Name = "XorWindow";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.WindowBar windowBar1;
        private AntdUI.UploadDragger uploadDragger;
    }
}