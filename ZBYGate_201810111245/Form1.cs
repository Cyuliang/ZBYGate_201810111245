using System;
using System.Drawing;
using System.Windows.Forms;

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

        #region//初始变量初始化
        private volatile bool ReadForBooen=true;
        #endregion

        #region//界面委托
        private delegate void UpdateUiInvok(string Message);
        private delegate void UpdateStatus(string Ip, uint status);
        #endregion

        #region //窗口对象初始化
        private Container.ContainerWindow _ContainerWindow = null;
        private Plate.PlateWindow _PlateWindow = null;
        private CVR.CVRWindow _CVRWindow = null;
        private Gate.GateWindow _GateWindow = null;
        private LED.LEDWindow _LEDWindow = null;    
        #endregion

        #region//类对象初始化
        private Container.Container _Container = new Container.Container();
        private Plate.Plate _Plate = new Plate.Plate();
        private CVR.CVR _CVR = new CVR.CVR();
        private Gate.Gate _Gate = new Gate.Gate();
        private LED.LED _LED = new LED.LED();
        #endregion

        #region//箱号识别委托
        private Action<int> ContainerLinkAction;
        private Action<int> ContainerAbortAction;
        private Action<int> ContainerLastRAction;
        #endregion

        #region//车牌委托
        private Action<int> PlateLinkAction;
        private Action<int> PlateAbortAction;
        private Action<int> PlateTiggerAction;
        private Action<int> PlateLiftingAction;
        private Action<int> PlateTransmissionAction;
        private Action<int> PlateSearchAction;
        private Action<int> PlateSetIpAction;
        private Action<int> PlateSetPathAction;
        private Action<bool> PlatePlayAction;
        private Action<bool> PlateCloseAction;
        #endregion

        #region//身份证委托
        private Action<int> CVRInitAction;
        private Action<int> CVRReadAction;
        private Action<int> CVRCloseAction;
        private Action<int> CVRWhileReadAction;
        private Action<int> CVRForReadAction;
        private Action<bool> CVRSetCVRvolatile;
        #endregion

        #region//道闸委托
        private Action<string,int,string> GateOpenDoorAction;
        #endregion

        #region//LED委托
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
            ContainerLinkAction  += _Container.LinkC;
            ContainerAbortAction += _Container.CloseC;
            ContainerLastRAction += _Container.LastR;
            _Container.SetMessage += GetMessage;
            _Container.GetStatusAction += SetStatusAction;
            #endregion

            #region//车牌委托订阅
            _Plate.SetMessage += GetMessage;
            PlateLinkAction += _Plate.CallbackFuntion;
            PlateAbortAction += _Plate.QuitDevice;
            PlateTiggerAction += _Plate.SetTrigger;
            PlateLiftingAction += _Plate.SetRelayClose;
            PlateSearchAction += _Plate.SearchDeviceList;
            PlateSetIpAction += _Plate.SetIpNetwork;
            PlateSetPathAction += _Plate.SetSaveImagePath;
            PlatePlayAction += _Plate.Play;
            PlateCloseAction += _Plate.Play;
            _Plate.PlateCallBack += PlateStatus;
            #endregion

            #region//身份证委托订阅
            CVRInitAction += _CVR.InitComm;
            CVRReadAction += _CVR.Authenticate;
            CVRCloseAction += _CVR.CloseComm;
            _CVR.SetMessageAction += GetMessage;
            CVRForReadAction += _CVR.AuthenticateFor;
            CVRWhileReadAction += _CVR.AuthenticateWhile;
            CVRSetCVRvolatile += _CVR.GetStarted;
            #endregion

            #region//道闸委托订阅
            GateOpenDoorAction += _Gate.OpenDoor;
            _Gate.SetMessage += GetMessage;
            #endregion

            #region//显示屏委托订阅
            _LED.SetMessage += GetMessage;
            #endregion
        }

        #region//主界面

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        #endregion

        #region//日志
        /// <summary>
        /// 数目大于300，清空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainlistBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MainlistBox.Items.Count > 300)
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
                MainlistBox.Items.Add(string.Format("ID：{0,3} {1} Message：[{2}]", MainlistBox.Items.Count + 1, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Message));
                MainlistBox.SelectedIndex = MainlistBox.Items.Count - 1;
            }
        }

        /// <summary>
        /// 清除日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainlistBoxClear_Click(object sender, EventArgs e)
        {
            MainlistBox.Items.Clear();
        }
        #endregion

        #region//Page页面
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
        #endregion

        #region //集装箱
        /// <summary>
        /// 集装箱显示按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContainerButtonShow_Click(object sender, EventArgs e)
        {
            if (_ContainerWindow==null|| _ContainerWindow.IsDisposed)
            {
                _ContainerWindow = new Container.ContainerWindow();
                ContainerWindowActiveInit();
            }         
            SetTabPate("ContainerTable", ContainerTable, form: _ContainerWindow);
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
        /// 链接箱号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContainerLinkButton_Click(object sender, EventArgs e)
        {
            ContainerLinkAction?.Invoke(0);
        }

        /// <summary>
        /// 主动断开链接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContainerAbortAction_Click(object sender, EventArgs e)
        {
            ContainerAbortAction?.Invoke(0);
        }

        /// <summary>
        /// 箱号链接状态
        /// </summary>
        /// <param name="i"></param>
        private void SetStatusAction(bool status)
        {
            if(status)
            {
                toolStripStatusLabel2.BackColor = Color.DarkGreen;
            }
            else
            {
                toolStripStatusLabel2.BackColor = Color.DarkRed;
            }
        }

        /// <summary>
        /// 获取最后一次识别结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContainerLastRAction_Click(object sender, EventArgs e)
        {
            ContainerLastRAction?.Invoke(Properties.Settings.Default.Container_Num);
        }
        #endregion

        #region//车牌
        /// <summary>
        /// 打开车牌显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlateWindowShow_Click(object sender, EventArgs e)
        {
            if (_PlateWindow == null || _PlateWindow.IsDisposed)
            {
                _PlateWindow = new Plate.PlateWindow();
                PlateWindowActiveInit();
            }
            SetTabPate("PlateTable", PlateTable, form: _PlateWindow);
        }

        /// <summary>
        /// 车牌链接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlateLinkAction_Click(object sender, EventArgs e)
        {
            PlateLinkAction?.Invoke(0);
        }

        /// <summary>
        /// 断开车牌链接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlateAbortAction_Click(object sender, EventArgs e)
        {
            PlateAbortAction?.Invoke(0);
        }

        /// <summary>
        /// 手动抓拍
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlateTiggerAction_Click(object sender, EventArgs e)
        {
            PlateTiggerAction?.Invoke(0);
        }

        /// <summary>
        /// 手动开闸
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlateLiftingAction_Click(object sender, EventArgs e)
        {
            PlateLiftingAction?.Invoke(0);
        }

        /// <summary>
        /// 发送485数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlateTransmissionAction_Click(object sender, EventArgs e)
        {
            PlateTransmissionAction?.Invoke(0);
        }

        /// <summary>
        /// 搜索设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlateSearchAction_Click(object sender, EventArgs e)
        {
            PlateSearchAction?.Invoke(0);
        }

        /// <summary>
        /// 设置路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlateSetPathAction_Click(object sender, EventArgs e)
        {
            PlateSetPathAction?.Invoke(0);
        }

        /// <summary>
        /// 设置IP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlateSetIpAction_Click(object sender, EventArgs e)
        {
            PlateSetIpAction?.Invoke(0);
        }

        /// <summary>
        /// 打开视频
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlatePlayAction_Click(object sender, EventArgs e)
        {
            PlatePlayAction?.Invoke(true);
        }

        /// <summary>
        /// 关闭视频
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlateCloseAction_Click(object sender, EventArgs e)
        {
            PlateCloseAction?.Invoke(false);
        }

        /// <summary>
        /// 车牌相机状态
        /// </summary>
        /// <param name="Ip"></param>
        /// <param name="status"></param>
        private void PlateStatus(string Ip,uint status)
        {
            if (statusStrip2.InvokeRequired)
            {
                statusStrip2.Invoke(new UpdateStatus(PlateStatus), new object[] { Ip,status });
            }
            else
            {
                if (status == 1)
                {
                    toolStripStatusLabel3.BackColor = Color.DarkGreen;
                }
                if (status == 0)
                {
                    toolStripStatusLabel3.BackColor = Color.DarkRed;
                }
            }                
        }

        /// <summary>
        /// 车牌界面初始化
        /// </summary>
        private void PlateWindowActiveInit()
        {
            _Plate.PlateDataCallBack += _PlateWindow.PlateResult;
            _Plate.DataJpegCallBack += _PlateWindow.DataJpeg;
            _Plate.JpegCallBack += _PlateWindow.JpegCallBack;
            PlateTransmissionAction += _PlateWindow.TestSend485Action;
            _PlateWindow.PlateTransmissionAction += _Plate.RS485Send;
        }
        #endregion

        #region//身份证
        private void CVRWindowShow_Click(object sender, EventArgs e)
        {
            if (_CVRWindow == null || _CVRWindow.IsDisposed)
            {
                _CVRWindow = new CVR.CVRWindow();
                CVRWindowActiveInit();
            }
            SetTabPate("CvrTable", CvrTable, form: _CVRWindow);
        }

        /// <summary>
        /// 界面委托
        /// </summary>
        private void CVRWindowActiveInit()
        {
            _CVR.FillDataActive += _CVRWindow.FillData;
            _CVR.FillDataBmpActive += _CVRWindow.FillDataBmp;
        }

        /// <summary>
        /// 动态库初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CVRInit_Click(object sender, EventArgs e)
        {
            CVRInitAction?.Invoke(0);
        }

        /// <summary>
        /// 读取身份证信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CVRRead_Click(object sender, EventArgs e)
        {
            CVRReadAction?.Invoke(0);
        }

        /// <summary>
        /// 关闭动态库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CVRClose_Click(object sender, EventArgs e)
        {
            CVRCloseAction?.Invoke(0);
        }

        /// <summary>
        /// 定时循环读取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CVRForRead_Click(object sender, EventArgs e)
        {
            if (ReadForBooen)
            {
                whileToolStripMenuItem.Checked = false;
                CVRForReadAction?.BeginInvoke(0,new AsyncCallback(CallForDone), null);
                ReadForBooen = false;
            }
        }

        /// <summary>
        /// 定时循环读取回调
        /// </summary>
        /// <param name="ar"></param>
        private void CallForDone(IAsyncResult ar)
        {
            MessageBox.Show("调用完成");
            ReadForBooen = true;
        }

        /// <summary>
        /// 无限循环读取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void whileToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if (whileToolStripMenuItem.Checked)
            {
                CVRSetCVRvolatile?.Invoke(true);
                CVRWhileReadAction?.BeginInvoke(0,new AsyncCallback(CallWhileDone), null);
            }
            else//取消选中状态，停止循环
            {
                CVRSetCVRvolatile?.Invoke(false);
            }
        }

        /// <summary>
        /// 无限循环读卡回调
        /// </summary>
        /// <param name="ar"></param>
        private void CallWhileDone(IAsyncResult ar)
        {
            MessageBox.Show("停止循环");
        }
        #endregion

        #region//道闸

        /// <summary>
        /// 道闸显示界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GateWindowShow_Click(object sender, EventArgs e)
        {
            if (_GateWindow == null || _GateWindow.IsDisposed)
            {
                _GateWindow = new Gate.GateWindow();
            }
            SetTabPate("GateTable", GateTable, form: _GateWindow);
        }

        /// <summary>
        /// 进闸开门
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GateOpenInDoor_Click(object sender, EventArgs e)
        {
            GateOpenDoorAction?.Invoke(Properties.Settings.Default.Gate_InDoorIp,Properties.Settings.Default.Gate_Port,Properties.Settings.Default.Gate_InDoorSN);
        }

        /// <summary>
        /// 出闸开门
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GateOpenOutDoor_Click(object sender, EventArgs e)
        {
            GateOpenDoorAction?.Invoke(Properties.Settings.Default.Gate_OutDoorIp, Properties.Settings.Default.Gate_Port, Properties.Settings.Default.Gate_OutDoorSN);
        }

        #endregion

        #region//LED

        /// <summary>
        /// LED显示界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LEDWindowShow_Click(object sender, EventArgs e)
        {
            if (_LEDWindow == null || _LEDWindow.IsDisposed)
            {
                _LEDWindow = new LED.LEDWindow();
                LedWindowActiveInit();
            }
            SetTabPate("LedTable", LedTable, form: _LEDWindow);
        }

        /// <summary>
        /// Led界面初始化
        /// </summary>
        private void LedWindowActiveInit()
        {
            _LEDWindow.InitAction += _LED.Initialize;
            _LEDWindow.AddScreenAction += _LED.AddScreen_Dynamic;
            _LEDWindow.AddAreaAction += _LED.AddScreenDynamicArea;
            _LEDWindow.AddTextAction += _LED.AddScreenDynamicAreaText;
            _LEDWindow.SendAction += _LED.SendDynamicAreasInfoCommand;
            _LEDWindow.UnintAction += _LED.Uninitialize;
            _LED.SetMessage += _LEDWindow.SetStatusText;
        }

        #endregion
    }
}
