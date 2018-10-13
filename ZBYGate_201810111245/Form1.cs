using System;
using System.Windows.Forms;
using ZBYGate_201810111245.Container;

namespace ZBYGate_201810111245
{
    public partial class Form1 : Form
    {
        #region Table页面
        private TabPage ContainerTable = new TabPage("集装箱");
        private TabPage PlateTable = new TabPage("电子车牌");
        private TabPage CvrTable = new TabPage("身份证");
        private TabPage GateTable = new TabPage("道闸");
        private TabPage LedTable = new TabPage("显示屏");
        private TabPage PrintTable = new TabPage("打印机");
        private TabPage ScanerTable = new TabPage("扫描仪");
        private TabPage ServerTable = new TabPage("服务端");
        private TabPage ClientTable = new TabPage("客户端");
        private TabPage HttpTable = new TabPage("HTTP");
        private TabPage LocalTable = new TabPage("本地数据库");
        private TabPage InTable = new TabPage("入闸数据库");
        private TabPage OutTable = new TabPage("出闸数据库");
        private TabPage AboutTable = new TabPage("系统说明");
        #endregion

        #region //窗口对象初始化
        private ContainerWindow _ContainerWindow = null;
        #endregion

        #region//类对象初始化
        private Container.Container _Container = new Container.Container();
        #endregion

        #region//箱号识别委托
        private Action<int> LinkAction;
        private Action<int> AbortAction;
        private Action<int> LastRAction;
        private delegate void UpdateUiInvok(string Message);
        #endregion

        public Form1()
        {
            InitializeComponent();

            #region//系统动作初始化
            #endregion

            #region //控件状态初始化
            toolStripButton1.Enabled = false;
            #endregion

            #region//箱号识别委托订阅
            LinkAction  += _Container.LinkC;
            AbortAction += _Container.CloseC;
            LastRAction += _Container.LastR;
            _Container.SetMessage += GetMessage;
            #endregion
        }

        /// <summary>
        /// 箱号结果显示界面委托订阅
        /// </summary>
        private void ContainerWindowActiveInit()
        {
            _Container.NewLPNEvent += _ContainerWindow.NewLPN;
            _Container.UpdateLPNEvent += _ContainerWindow.UpdateLPN;
            _Container.CombinResult += _ContainerWindow.CombinResult;
        }

        /// <summary>
        /// 添加Page页面
        /// </summary>
        /// <param name="Name">页面名称</param>
        /// <param name="tabPage">页面类</param>
        /// <param name="form">窗口类</param>
        private void SetTabPate(string Name, TabPage tabPage, Form form)
        {
            try
            {
                if (ErgodicModiForm(Name, tabControl1))
                {
                    tabPage.Name = Name;
                    tabControl1.Controls.Add(tabPage);
                    form.TopLevel = false;
                    form.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    form.FormBorderStyle = FormBorderStyle.None;
                    form.Dock = DockStyle.Fill;
                    form.Show();
                    tabPage.Controls.Add(form);
                }
                tabControl1.SelectedTab = tabPage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }        
        }

        /// </summary>  
        /// <param name="MainTabControlKey">选项卡的键值</param>  
        /// <param name="objTabControl">要添加到的TabControl对象</param>  
        /// <returns></returns>  
        private Boolean ErgodicModiForm(string MainTabControlKey, TabControl objTabControl)
        {
            //遍历选项卡判断是否存在该子窗体  
            foreach (Control con in objTabControl.Controls)
            {
                TabPage tab = (TabPage)con;
                if (tab.Name == MainTabControlKey)
                {
                    return false;//存在  
                }
            }
            return true;//不存在  
        }

        /// <summary>
        /// 集装箱显示按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContainerButtonShow_Click(object sender, EventArgs e)
        {
            if (_ContainerWindow==null|| _ContainerWindow.IsDisposed)
            {
                _ContainerWindow = new ContainerWindow();
                ContainerWindowActiveInit();
            }         
            SetTabPate("ContainerTable", ContainerTable, form: _ContainerWindow);
        }

        /// <summary>
        /// 关闭TabPage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseTabPageButton_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex != 0)
            {
                foreach (Control form in tabControl1.SelectedTab.Controls)
                {
                    Form f = (Form)form;
                    f.Close();
                }
                tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);
            }
        }

        /// <summary>
        /// 当前选区的页面，关闭按钮生效。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {           
            if (tabControl1.SelectedIndex == 0)
            {
                toolStripButton1.Enabled = false;
            }
            else
            {
                toolStripButton1.Enabled = true;
            }
        }

        /// <summary>
        /// 链接箱号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContainerLinkButton_Click(object sender, EventArgs e)
        {
            LinkAction?.Invoke(0);
        }

        /// <summary>
        /// 主动断开链接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripButton4_Click(object sender, EventArgs e)
        {
            AbortAction?.Invoke(0);
        }

        /// <summary>
        /// 获取最后一次识别结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripButton5_Click(object sender, EventArgs e)
        {
            LastRAction?.Invoke(Properties.Settings.Default.Container_Num);
        }

        /// <summary>
        /// 数目大于300，清空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainlistBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(MainlistBox.Items.Count>300)
            {
                MainlistBox.Items.Clear();
            }
        }

        /// <summary>
        /// 主页显示日志
        /// </summary>
        /// <param name="Message"></param>
        public void GetMessage(string Message)
        {
            if (MainlistBox.InvokeRequired)
            {
                MainlistBox.Invoke(new UpdateUiInvok(GetMessage), new object[] { Message });
            }
            else
            {
                MainlistBox.Items.Add(string.Format("ID：{0,3} {1} Message：[{2}]", MainlistBox.Items.Count+1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Message));
                MainlistBox.SelectedIndex = MainlistBox.Items.Count - 1;
            }
        }

        /// <summary>
        /// 清除日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            MainlistBox.Items.Clear();
        }
    }
}
