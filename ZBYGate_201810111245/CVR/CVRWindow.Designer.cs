namespace ZBYGate_Data_Collection.CVR
{
    partial class CVRWindow
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
                _timer.Dispose();
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.labelValidDate = new System.Windows.Forms.Label();
            this.labelDepartment = new System.Windows.Forms.Label();
            this.labelAddress = new System.Windows.Forms.Label();
            this.labelIdCardNo = new System.Windows.Forms.Label();
            this.labelBirthday = new System.Windows.Forms.Label();
            this.labelNation = new System.Windows.Forms.Label();
            this.labelSex = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.pictureBoxPhoto = new System.Windows.Forms.PictureBox();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPhoto)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(674, 17);
            this.StatusLabel.Spring = true;
            this.StatusLabel.TextChanged += new System.EventHandler(this.StatusLabel_TextChanged);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(80, 17);
            this.toolStripStatusLabel2.Text = "身份号码读取";
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(103, 294);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(351, 21);
            this.textBox8.TabIndex = 73;
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(103, 260);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(351, 21);
            this.textBox7.TabIndex = 72;
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(103, 226);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(351, 21);
            this.textBox6.TabIndex = 71;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(103, 192);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(351, 21);
            this.textBox5.TabIndex = 70;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(103, 158);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(351, 21);
            this.textBox4.TabIndex = 69;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(103, 124);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(351, 21);
            this.textBox3.TabIndex = 68;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(103, 90);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(351, 21);
            this.textBox2.TabIndex = 67;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(103, 56);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(351, 21);
            this.textBox1.TabIndex = 66;
            // 
            // labelValidDate
            // 
            this.labelValidDate.AutoSize = true;
            this.labelValidDate.Location = new System.Drawing.Point(32, 264);
            this.labelValidDate.Name = "labelValidDate";
            this.labelValidDate.Size = new System.Drawing.Size(65, 12);
            this.labelValidDate.TabIndex = 64;
            this.labelValidDate.Text = "有效期限：";
            // 
            // labelDepartment
            // 
            this.labelDepartment.AutoSize = true;
            this.labelDepartment.Location = new System.Drawing.Point(32, 230);
            this.labelDepartment.Name = "labelDepartment";
            this.labelDepartment.Size = new System.Drawing.Size(65, 12);
            this.labelDepartment.TabIndex = 63;
            this.labelDepartment.Text = "签发机关：";
            // 
            // labelAddress
            // 
            this.labelAddress.AutoSize = true;
            this.labelAddress.Location = new System.Drawing.Point(56, 196);
            this.labelAddress.Name = "labelAddress";
            this.labelAddress.Size = new System.Drawing.Size(41, 12);
            this.labelAddress.TabIndex = 62;
            this.labelAddress.Text = "地址：";
            // 
            // labelIdCardNo
            // 
            this.labelIdCardNo.AutoSize = true;
            this.labelIdCardNo.Location = new System.Drawing.Point(32, 162);
            this.labelIdCardNo.Name = "labelIdCardNo";
            this.labelIdCardNo.Size = new System.Drawing.Size(65, 12);
            this.labelIdCardNo.TabIndex = 61;
            this.labelIdCardNo.Text = "身份证号：";
            // 
            // labelBirthday
            // 
            this.labelBirthday.AutoSize = true;
            this.labelBirthday.Location = new System.Drawing.Point(32, 128);
            this.labelBirthday.Name = "labelBirthday";
            this.labelBirthday.Size = new System.Drawing.Size(65, 12);
            this.labelBirthday.TabIndex = 60;
            this.labelBirthday.Text = "出生日期：";
            // 
            // labelNation
            // 
            this.labelNation.AutoSize = true;
            this.labelNation.Location = new System.Drawing.Point(56, 298);
            this.labelNation.Name = "labelNation";
            this.labelNation.Size = new System.Drawing.Size(41, 12);
            this.labelNation.TabIndex = 59;
            this.labelNation.Text = "民族：";
            // 
            // labelSex
            // 
            this.labelSex.AutoSize = true;
            this.labelSex.Location = new System.Drawing.Point(56, 94);
            this.labelSex.Name = "labelSex";
            this.labelSex.Size = new System.Drawing.Size(41, 12);
            this.labelSex.TabIndex = 58;
            this.labelSex.Text = "性别：";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(56, 60);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(41, 12);
            this.labelName.TabIndex = 57;
            this.labelName.Text = "姓名：";
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(25, 25);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripButton4,
            this.toolStripButton5});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 32);
            this.toolStrip1.TabIndex = 74;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::ZBYGate_Data_Collection.Resource1.初始化;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(29, 29);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.ToolTipText = "初始化身份证动态库";
            this.toolStripButton1.Click += new System.EventHandler(this.ToolstripButton_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::ZBYGate_Data_Collection.Resource1.读取;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(29, 29);
            this.toolStripButton2.Text = "toolStripButton2";
            this.toolStripButton2.ToolTipText = "读取身份证信息";
            this.toolStripButton2.Click += new System.EventHandler(this.ToolstripButton_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = global::ZBYGate_Data_Collection.Resource1.关_闭;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(29, 29);
            this.toolStripButton3.Text = "toolStripButton3";
            this.toolStripButton3.ToolTipText = "关闭串口";
            this.toolStripButton3.Click += new System.EventHandler(this.ToolstripButton_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.CheckOnClick = true;
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = global::ZBYGate_Data_Collection.Resource1.循环;
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(29, 29);
            this.toolStripButton4.Text = "toolStripButton4";
            this.toolStripButton4.ToolTipText = "循环读取身份证信息";
            this.toolStripButton4.Click += new System.EventHandler(this.ToolstripButton_Click);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton5.Image = global::ZBYGate_Data_Collection.Resource1.定时;
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(29, 29);
            this.toolStripButton5.Text = "toolStripButton5";
            this.toolStripButton5.ToolTipText = "定时读取身份证信息";
            this.toolStripButton5.Click += new System.EventHandler(this.ToolstripButton_Click);
            // 
            // pictureBoxPhoto
            // 
            this.pictureBoxPhoto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxPhoto.Location = new System.Drawing.Point(477, 56);
            this.pictureBoxPhoto.Name = "pictureBoxPhoto";
            this.pictureBoxPhoto.Size = new System.Drawing.Size(112, 137);
            this.pictureBoxPhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxPhoto.TabIndex = 65;
            this.pictureBoxPhoto.TabStop = false;
            // 
            // CVRWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.textBox8);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.pictureBoxPhoto);
            this.Controls.Add(this.labelValidDate);
            this.Controls.Add(this.labelDepartment);
            this.Controls.Add(this.labelAddress);
            this.Controls.Add(this.labelIdCardNo);
            this.Controls.Add(this.labelBirthday);
            this.Controls.Add(this.labelNation);
            this.Controls.Add(this.labelSex);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.statusStrip1);
            this.Name = "CVRWindow";
            this.Text = "CVRWindow";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPhoto)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.PictureBox pictureBoxPhoto;
        private System.Windows.Forms.Label labelValidDate;
        private System.Windows.Forms.Label labelDepartment;
        private System.Windows.Forms.Label labelAddress;
        private System.Windows.Forms.Label labelIdCardNo;
        private System.Windows.Forms.Label labelBirthday;
        private System.Windows.Forms.Label labelNation;
        private System.Windows.Forms.Label labelSex;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
    }
}