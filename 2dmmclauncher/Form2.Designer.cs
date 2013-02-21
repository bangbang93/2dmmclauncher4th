namespace _2dmmclauncher
{
    partial class downloadForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.speed = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.prog = new System.Windows.Forms.Label();
            this.size = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonCancal = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(39, 24);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(705, 52);
            this.progressBar1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 122);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "速度";
            // 
            // speed
            // 
            this.speed.AutoSize = true;
            this.speed.Location = new System.Drawing.Point(80, 122);
            this.speed.Name = "speed";
            this.speed.Size = new System.Drawing.Size(35, 12);
            this.speed.TabIndex = 2;
            this.speed.Text = "speed";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(45, 152);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "进度";
            // 
            // prog
            // 
            this.prog.AutoSize = true;
            this.prog.Location = new System.Drawing.Point(80, 152);
            this.prog.Name = "prog";
            this.prog.Size = new System.Drawing.Size(11, 12);
            this.prog.TabIndex = 4;
            this.prog.Text = "%";
            // 
            // size
            // 
            this.size.AutoSize = true;
            this.size.Location = new System.Drawing.Point(326, 122);
            this.size.Name = "size";
            this.size.Size = new System.Drawing.Size(17, 12);
            this.size.TabIndex = 7;
            this.size.Text = "MB";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(268, 122);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "总大小";
            // 
            // buttonCancal
            // 
            this.buttonCancal.Location = new System.Drawing.Point(541, 111);
            this.buttonCancal.Name = "buttonCancal";
            this.buttonCancal.Size = new System.Drawing.Size(168, 35);
            this.buttonCancal.TabIndex = 9;
            this.buttonCancal.Text = "取消";
            this.buttonCancal.UseVisualStyleBackColor = true;
            this.buttonCancal.Click += new System.EventHandler(this.buttonCancal_Click);
            // 
            // downloadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(817, 237);
            this.Controls.Add(this.buttonCancal);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.size);
            this.Controls.Add(this.prog);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.speed);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Name = "downloadForm";
            this.ShowIcon = false;
            this.Text = "废话二次元客户端下载器";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.Shown += new System.EventHandler(this.downloadForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label speed;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label prog;
        private System.Windows.Forms.Label size;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonCancal;
    }
}