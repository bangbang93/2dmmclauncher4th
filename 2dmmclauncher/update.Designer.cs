namespace _2dmmclauncher
{
    partial class update
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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.checkDir = new System.Windows.Forms.Label();
            this.checkFile = new System.Windows.Forms.Label();
            this.downFile = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 12);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(880, 53);
            this.progressBar1.TabIndex = 0;
            // 
            // checkDir
            // 
            this.checkDir.AutoSize = true;
            this.checkDir.Location = new System.Drawing.Point(13, 83);
            this.checkDir.Name = "checkDir";
            this.checkDir.Size = new System.Drawing.Size(101, 12);
            this.checkDir.TabIndex = 1;
            this.checkDir.Text = "正在核对的文件夹";
            // 
            // checkFile
            // 
            this.checkFile.AutoSize = true;
            this.checkFile.Location = new System.Drawing.Point(13, 111);
            this.checkFile.Name = "checkFile";
            this.checkFile.Size = new System.Drawing.Size(89, 12);
            this.checkFile.TabIndex = 2;
            this.checkFile.Text = "正在核对的文件";
            // 
            // downFile
            // 
            this.downFile.AutoSize = true;
            this.downFile.Location = new System.Drawing.Point(13, 139);
            this.downFile.Name = "downFile";
            this.downFile.Size = new System.Drawing.Size(0, 12);
            this.downFile.TabIndex = 3;
            // 
            // update
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(904, 181);
            this.Controls.Add(this.downFile);
            this.Controls.Add(this.checkFile);
            this.Controls.Add(this.checkDir);
            this.Controls.Add(this.progressBar1);
            this.Name = "update";
            this.Text = "update";
            this.Load += new System.EventHandler(this.update_Load);
            this.Shown += new System.EventHandler(this.update_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label checkDir;
        private System.Windows.Forms.Label checkFile;
        private System.Windows.Forms.Label downFile;
    }
}