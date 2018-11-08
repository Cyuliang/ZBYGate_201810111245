namespace ZBYGate_Data_Collection
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                _Container.Dispose();
                _Plate.Dispose();
                _Gate.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip2 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.设备ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.集装箱号码ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.身份证读卡ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.车牌识别ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.显示屏ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.扫描仪ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打印机ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.道闸ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.网络ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.服务端ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.客户端ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hTTPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.数据库ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.本地数据库ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.入闸数据库ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.车闸数据库ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.系统设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.技术支持ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于DataCollectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于深圳众百源科技ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.MainPage = new System.Windows.Forms.TabPage();
            this.MainlistBox = new System.Windows.Forms.ListBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1.SuspendLayout();
            this.statusStrip2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.MainPage.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BackColor = System.Drawing.SystemColors.Highlight;
            this.toolStripStatusLabel1.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(785, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "深圳众百源科技有限公司";
            // 
            // statusStrip2
            // 
            this.statusStrip2.BackColor = System.Drawing.Color.Coral;
            this.statusStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel4,
            this.toolStripStatusLabel5});
            this.statusStrip2.Location = new System.Drawing.Point(0, 406);
            this.statusStrip2.Name = "statusStrip2";
            this.statusStrip2.Size = new System.Drawing.Size(800, 22);
            this.statusStrip2.TabIndex = 2;
            this.statusStrip2.Text = "statusStrip2";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel2.ForeColor = System.Drawing.SystemColors.Window;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(68, 17);
            this.toolStripStatusLabel2.Text = "集装箱识别";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.ForeColor = System.Drawing.SystemColors.Window;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(56, 17);
            this.toolStripStatusLabel3.Text = "车牌识别";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.ForeColor = System.Drawing.SystemColors.Window;
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(56, 17);
            this.toolStripStatusLabel4.Text = "入闸道闸";
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.ForeColor = System.Drawing.SystemColors.Window;
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(56, 17);
            this.toolStripStatusLabel5.Text = "出闸道闸";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设备ToolStripMenuItem,
            this.网络ToolStripMenuItem,
            this.数据库ToolStripMenuItem,
            this.设置ToolStripMenuItem,
            this.关于ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 25);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 设备ToolStripMenuItem
            // 
            this.设备ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.集装箱号码ToolStripMenuItem,
            this.身份证读卡ToolStripMenuItem,
            this.车牌识别ToolStripMenuItem,
            this.显示屏ToolStripMenuItem,
            this.扫描仪ToolStripMenuItem,
            this.打印机ToolStripMenuItem,
            this.道闸ToolStripMenuItem});
            this.设备ToolStripMenuItem.Name = "设备ToolStripMenuItem";
            this.设备ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.设备ToolStripMenuItem.Text = "设备";
            // 
            // 集装箱号码ToolStripMenuItem
            // 
            this.集装箱号码ToolStripMenuItem.Image = global::ZBYGate_Data_Collection.Resource1.集装箱;
            this.集装箱号码ToolStripMenuItem.Name = "集装箱号码ToolStripMenuItem";
            this.集装箱号码ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.集装箱号码ToolStripMenuItem.Text = "集装箱号码";
            this.集装箱号码ToolStripMenuItem.Click += new System.EventHandler(this.ContainerButtonShow_Click);
            // 
            // 身份证读卡ToolStripMenuItem
            // 
            this.身份证读卡ToolStripMenuItem.Image = global::ZBYGate_Data_Collection.Resource1.身份证;
            this.身份证读卡ToolStripMenuItem.Name = "身份证读卡ToolStripMenuItem";
            this.身份证读卡ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.身份证读卡ToolStripMenuItem.Text = "身份证读卡";
            this.身份证读卡ToolStripMenuItem.Click += new System.EventHandler(this.CVRWindowShow_Click);
            // 
            // 车牌识别ToolStripMenuItem
            // 
            this.车牌识别ToolStripMenuItem.Image = global::ZBYGate_Data_Collection.Resource1.车牌__2_;
            this.车牌识别ToolStripMenuItem.Name = "车牌识别ToolStripMenuItem";
            this.车牌识别ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.车牌识别ToolStripMenuItem.Text = "车牌识别";
            this.车牌识别ToolStripMenuItem.Click += new System.EventHandler(this.PlateWindowShow_Click);
            // 
            // 显示屏ToolStripMenuItem
            // 
            this.显示屏ToolStripMenuItem.Image = global::ZBYGate_Data_Collection.Resource1.led;
            this.显示屏ToolStripMenuItem.Name = "显示屏ToolStripMenuItem";
            this.显示屏ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.显示屏ToolStripMenuItem.Text = "显示屏";
            this.显示屏ToolStripMenuItem.Click += new System.EventHandler(this.LEDWindowShow_Click);
            // 
            // 扫描仪ToolStripMenuItem
            // 
            this.扫描仪ToolStripMenuItem.Image = global::ZBYGate_Data_Collection.Resource1.扫描仪;
            this.扫描仪ToolStripMenuItem.Name = "扫描仪ToolStripMenuItem";
            this.扫描仪ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.扫描仪ToolStripMenuItem.Text = "扫描仪";
            // 
            // 打印机ToolStripMenuItem
            // 
            this.打印机ToolStripMenuItem.Image = global::ZBYGate_Data_Collection.Resource1.打印机;
            this.打印机ToolStripMenuItem.Name = "打印机ToolStripMenuItem";
            this.打印机ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.打印机ToolStripMenuItem.Text = "打印机";
            // 
            // 道闸ToolStripMenuItem
            // 
            this.道闸ToolStripMenuItem.Image = global::ZBYGate_Data_Collection.Resource1.道闸02;
            this.道闸ToolStripMenuItem.Name = "道闸ToolStripMenuItem";
            this.道闸ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.道闸ToolStripMenuItem.Text = "道闸";
            this.道闸ToolStripMenuItem.Click += new System.EventHandler(this.GateWindowShow_Click);
            // 
            // 网络ToolStripMenuItem
            // 
            this.网络ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.服务端ToolStripMenuItem,
            this.客户端ToolStripMenuItem,
            this.hTTPToolStripMenuItem});
            this.网络ToolStripMenuItem.Name = "网络ToolStripMenuItem";
            this.网络ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.网络ToolStripMenuItem.Text = "网络";
            // 
            // 服务端ToolStripMenuItem
            // 
            this.服务端ToolStripMenuItem.Name = "服务端ToolStripMenuItem";
            this.服务端ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.服务端ToolStripMenuItem.Text = "服务端";
            // 
            // 客户端ToolStripMenuItem
            // 
            this.客户端ToolStripMenuItem.Name = "客户端ToolStripMenuItem";
            this.客户端ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.客户端ToolStripMenuItem.Text = "客户端";
            // 
            // hTTPToolStripMenuItem
            // 
            this.hTTPToolStripMenuItem.Name = "hTTPToolStripMenuItem";
            this.hTTPToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.hTTPToolStripMenuItem.Text = "HTTP";
            // 
            // 数据库ToolStripMenuItem
            // 
            this.数据库ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.本地数据库ToolStripMenuItem,
            this.入闸数据库ToolStripMenuItem,
            this.车闸数据库ToolStripMenuItem});
            this.数据库ToolStripMenuItem.Name = "数据库ToolStripMenuItem";
            this.数据库ToolStripMenuItem.Size = new System.Drawing.Size(56, 21);
            this.数据库ToolStripMenuItem.Text = "数据库";
            // 
            // 本地数据库ToolStripMenuItem
            // 
            this.本地数据库ToolStripMenuItem.Name = "本地数据库ToolStripMenuItem";
            this.本地数据库ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.本地数据库ToolStripMenuItem.Text = "本地数据库";
            // 
            // 入闸数据库ToolStripMenuItem
            // 
            this.入闸数据库ToolStripMenuItem.Name = "入闸数据库ToolStripMenuItem";
            this.入闸数据库ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.入闸数据库ToolStripMenuItem.Text = "入闸数据库";
            // 
            // 车闸数据库ToolStripMenuItem
            // 
            this.车闸数据库ToolStripMenuItem.Name = "车闸数据库ToolStripMenuItem";
            this.车闸数据库ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.车闸数据库ToolStripMenuItem.Text = "车闸数据库";
            // 
            // 设置ToolStripMenuItem
            // 
            this.设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.系统设置ToolStripMenuItem});
            this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            this.设置ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.设置ToolStripMenuItem.Text = "设置";
            // 
            // 系统设置ToolStripMenuItem
            // 
            this.系统设置ToolStripMenuItem.Name = "系统设置ToolStripMenuItem";
            this.系统设置ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.系统设置ToolStripMenuItem.Text = "系统设置";
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.技术支持ToolStripMenuItem,
            this.关于DataCollectionToolStripMenuItem,
            this.关于深圳众百源科技ToolStripMenuItem});
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.关于ToolStripMenuItem.Text = "关于";
            // 
            // 技术支持ToolStripMenuItem
            // 
            this.技术支持ToolStripMenuItem.Name = "技术支持ToolStripMenuItem";
            this.技术支持ToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.技术支持ToolStripMenuItem.Text = "技术支持";
            // 
            // 关于DataCollectionToolStripMenuItem
            // 
            this.关于DataCollectionToolStripMenuItem.Name = "关于DataCollectionToolStripMenuItem";
            this.关于DataCollectionToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.关于DataCollectionToolStripMenuItem.Text = "关于 Data Collection";
            // 
            // 关于深圳众百源科技ToolStripMenuItem
            // 
            this.关于深圳众百源科技ToolStripMenuItem.Name = "关于深圳众百源科技ToolStripMenuItem";
            this.关于深圳众百源科技ToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.关于深圳众百源科技ToolStripMenuItem.Text = "关于深圳众百源科技";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 57);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 349F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 349);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tabControl1.Controls.Add(this.MainPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(794, 343);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.TabControl1_SelectedIndexChanged);
            // 
            // MainPage
            // 
            this.MainPage.Controls.Add(this.MainlistBox);
            this.MainPage.Location = new System.Drawing.Point(22, 4);
            this.MainPage.Name = "MainPage";
            this.MainPage.Padding = new System.Windows.Forms.Padding(3);
            this.MainPage.Size = new System.Drawing.Size(768, 335);
            this.MainPage.TabIndex = 0;
            this.MainPage.Text = "Main";
            this.MainPage.UseVisualStyleBackColor = true;
            // 
            // MainlistBox
            // 
            this.MainlistBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainlistBox.FormattingEnabled = true;
            this.MainlistBox.ItemHeight = 12;
            this.MainlistBox.Location = new System.Drawing.Point(3, 3);
            this.MainlistBox.Name = "MainlistBox";
            this.MainlistBox.Size = new System.Drawing.Size(762, 329);
            this.MainlistBox.TabIndex = 0;
            this.MainlistBox.SelectedIndexChanged += new System.EventHandler(this.MainlistBox_SelectedIndexChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 32);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(25, 25);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripSeparator1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 25);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 32);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::ZBYGate_Data_Collection.Resource1.cloase;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(29, 29);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.ToolTipText = "关闭页面";
            this.toolStripButton1.Click += new System.EventHandler(this.CloseTabPageButton_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(29, 29);
            this.toolStripButton2.Text = "toolStripButton2";
            this.toolStripButton2.ToolTipText = "清除日志";
            this.toolStripButton2.Click += new System.EventHandler(this.MainlistBoxClear_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "ZBY Gate Data Collection";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.statusStrip2.ResumeLayout(false);
            this.statusStrip2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.MainPage.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.StatusStrip statusStrip2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 设备ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 集装箱号码ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 车牌识别ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 身份证读卡ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 道闸ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 显示屏ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打印机ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 扫描仪ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 网络ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 服务端ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 客户端ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hTTPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 数据库ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 本地数据库ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 入闸数据库ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 车闸数据库ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 系统设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 技术支持ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于DataCollectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于深圳众百源科技ToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage MainPage;
        private System.Windows.Forms.ListBox MainlistBox;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStrip toolStrip1;
    }
}

