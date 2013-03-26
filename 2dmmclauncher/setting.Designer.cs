namespace _2dmmclauncher
{
    partial class setting
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Apply = new System.Windows.Forms.Button();
            this.playerNameBox = new System.Windows.Forms.TextBox();
            this.JavaXmxBox = new System.Windows.Forms.TextBox();
            this.javawBox = new System.Windows.Forms.TextBox();
            this.javawpath = new System.Windows.Forms.OpenFileDialog();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "玩家名";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 177);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "javaw.exe路径";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "JavaXmx";
            // 
            // Apply
            // 
            this.Apply.Location = new System.Drawing.Point(565, 223);
            this.Apply.Name = "Apply";
            this.Apply.Size = new System.Drawing.Size(141, 38);
            this.Apply.TabIndex = 3;
            this.Apply.Text = "确定(&OK)";
            this.Apply.UseVisualStyleBackColor = true;
            this.Apply.Click += new System.EventHandler(this.Apply_Click);
            // 
            // playerNameBox
            // 
            this.playerNameBox.Location = new System.Drawing.Point(129, 19);
            this.playerNameBox.Name = "playerNameBox";
            this.playerNameBox.Size = new System.Drawing.Size(344, 21);
            this.playerNameBox.TabIndex = 4;
            this.playerNameBox.TextChanged += new System.EventHandler(this.playerNameBox_TextChanged);
            // 
            // JavaXmxBox
            // 
            this.JavaXmxBox.Location = new System.Drawing.Point(129, 89);
            this.JavaXmxBox.Name = "JavaXmxBox";
            this.JavaXmxBox.Size = new System.Drawing.Size(344, 21);
            this.JavaXmxBox.TabIndex = 5;
            this.JavaXmxBox.TextChanged += new System.EventHandler(this.JavaXmxBox_TextChanged);
            // 
            // javawBox
            // 
            this.javawBox.Location = new System.Drawing.Point(129, 168);
            this.javawBox.Name = "javawBox";
            this.javawBox.Size = new System.Drawing.Size(344, 21);
            this.javawBox.TabIndex = 6;
            this.javawBox.TextChanged += new System.EventHandler(this.javawBox_TextChanged);
            // 
            // javawpath
            // 
            this.javawpath.FileName = "javaw.exe";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(488, 169);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(34, 19);
            this.button2.TabIndex = 7;
            this.button2.Text = "...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // setting
            // 
            this.AcceptButton = this.Apply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(718, 273);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.javawBox);
            this.Controls.Add(this.JavaXmxBox);
            this.Controls.Add(this.playerNameBox);
            this.Controls.Add(this.Apply);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "setting";
            this.ShowIcon = false;
            this.Text = "设置";
            this.Load += new System.EventHandler(this.setting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Apply;
        private System.Windows.Forms.TextBox playerNameBox;
        private System.Windows.Forms.TextBox JavaXmxBox;
        private System.Windows.Forms.TextBox javawBox;
        private System.Windows.Forms.OpenFileDialog javawpath;
        private System.Windows.Forms.Button button2;
    }
}