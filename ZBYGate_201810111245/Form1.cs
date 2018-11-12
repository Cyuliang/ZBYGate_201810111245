using System;
using System.Drawing;
using System.Windows.Forms;

namespace ZBYGate_Data_Collection
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
        //private volatile bool ReadForBooen=true;
        #endregion

        #region//界面委托
        private delegate void UpdateUiBInvok(bool Status);
        private delegate void UpdateUiInvok(string Message);
        private delegate void UpdateStatus(string Ip, uint status);
        private delegate void UpdateUiUInvok(int Status, Int32 SN);
        #endregion

        #region //窗口对象初始化
        private Container.ContainerWindow _ContainerWindow = null;
        private Plate.PlateWindow _PlateWindow = null;
        private CVR.CVRWindow _CVRWindow = null;
        private Gate.GateWindow _GateWindow = null;
        private LED.LEDWindow _LEDWindow = null;
        private LocalDataBase.LocalDataBaseWindow _LocalDataBaseWindow = null;
        private IEDataBase.InDataWindow _InDataWindow = null;
        private IEDataBase.OutDataWindow _OutDataWindow = null;
        private Https.HttpWindow _HttpWindow = null;
        #endregion

        #region//类对象初始化
        private Container.Container _Container = new Container.Container();
        private Plate.Plate _Plate = new Plate.Plate();
        private CVR.CVR _CVR = new CVR.CVR();
        private Gate.Gate _Gate = new Gate.Gate();
        private LED.LED _LED = new LED.LED();
        private LocalDataBase.LocalDataBase _LocalDataBase = new LocalDataBase.LocalDataBase();
        private IEDataBase.RunData _RunData = new IEDataBase.RunData();
        private Https.CHttp _CHttp = new Https.CHttp();
        private Working _Working = new Working();
        #endregion

        #region//箱号识别委托
        #endregion

        #region//车牌委托
        #endregion

        #region//身份证委托
        #endregion

        #region//道闸委托
        #endregion

        #region//LED委托
        #endregion

        #region//本地数据库委托
        #endregion

        public Form1()
        {
            InitializeComponent();

            #region//系统动作初始化
            _Plate.PlateDataCallBack += _Working.PlateResult;//出闸车牌识别结果
            _Container.CombinResult += _Working.ContainerResult;//集装箱结果
            _Container.NewLPNEvent += _Working.NewLpnResult;//空车车牌结果
            _Container.UpdateLPNEvent += _Working.UpdateLpnResult;//重车车牌结果
            _Working.SelectDataBase += _LocalDataBase.SelectData;//本地数据库查询
            _Working.AddTextAction += _LED.AddScreenDynamicAreaText;//LED推送流程数据
            _Working.SendAction += _LED.SendDynamicAreasInfoCommand;
            _Working.OpenDoorAction += _Gate.OpenDoor;//开闸
            _Working.In_InsertDataBaseAction += _RunData.In_Insert;//入闸数据库写入
            _Working.Out_InsertDataBaseAction += _RunData.Out_Insert;//出闸数据写入
            _Working.SetMessage += GetMessage;//动作日志
            _Working.HttpPostAction += _CHttp.SetJosn;//查询远端服务器
            _Working.SetOutLedMessageAction += _Plate.RS485Send;
            #endregion

            #region //控件状态初始化
            toolStripButton1.Enabled = false;
            #endregion

            #region//箱号识别委托订阅
            _Container.SetMessage += GetMessage;
            _Container.GetStatusAction += ContainerStatus;
            #endregion

            #region//车牌委托订阅
            _Plate.SetMessage += GetMessage;
            _Plate.PlateCallBack += PlateStatus;
            #endregion

            #region//身份证委托订阅
            _CVR.SetMessageAction += GetMessage;
            #endregion

            #region//道闸委托订阅
            _Gate.NewState += _Gate_NewState;
            _Gate.SetMessage += GetMessage;
            #endregion

            #region//显示屏委托订阅
            _LED.SetMessage += GetMessage;
            #endregion

            #region//本地数据库委托
            _LocalDataBase.SetMessage += GetMessage;
            #endregion

            #region//出入闸数据库委托
            _RunData.SetMessage += GetMessage;
            #endregion

            #region//Https 委托
            _CHttp.SetMessage += GetMessage;
            #endregion
        }

        #region//窗口初始化
        /// <summary>
        /// 窗口加载初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            _Plate.SetIpNetwork(0);//设置车牌绑定IP
            _LED.Initialize(0);
            _LED.AddScreen_Dynamic(0);
            _LED.AddScreenDynamicArea(0);            
        }
        #endregion

        #region//主界面

        /// <summary>
        /// 后台进程退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            _Container.SetMessage += _ContainerWindow.SetStatusText;
            _ContainerWindow.ContainerLinkAction += _Container.LinkC;
            _ContainerWindow.ContainerAbortAction += _Container.CloseC;
            _ContainerWindow.ContainerLastRAction += _Container.LastR;            
        }

        /// <summary>
        /// 箱号链接状态
        /// </summary>
        /// <param name="i"></param>
        private void ContainerStatus(bool status)
        {
            if (statusStrip2.InvokeRequired)
            {
                statusStrip2.Invoke(new UpdateUiBInvok(ContainerStatus), new object[] { status });
            }
            else
            {
                if (status)
                {
                    toolStripStatusLabel2.BackColor = Color.DarkGreen;
                }
                else
                {
                    toolStripStatusLabel2.BackColor = Color.DarkRed;
                }
            }
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
            _Plate.SetMessage += _PlateWindow.SetStatusText;
            _PlateWindow.PlateLinkAction += _Plate.CallbackFuntion;
            _PlateWindow.PlateAbortAction += _Plate.QuitDevice;
            _PlateWindow.PlateTiggerAction += _Plate.SetTrigger;
            _PlateWindow.PlateLiftingAction += _Plate.SetRelayClose;
            _PlateWindow.PlateTransmissionAction += _Plate.RS485Send;
            _PlateWindow.PlateSearchAction += _Plate.SearchDeviceList;
            _PlateWindow.PlateSetIpAction += _Plate.SetIpNetwork;
            _PlateWindow.PlateSetPathAction += _Plate.SetSaveImagePath;
            _PlateWindow.PlatePlayAction += _Plate.Play;
            _PlateWindow.PlateCloseAction += _Plate.Play;
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
            _CVR.SetMessageAction += _CVRWindow.SetStatusText;
            _CVR.FillDataActive += _CVRWindow.FillData;
            _CVR.FillDataBmpActive += _CVRWindow.FillDataBmp;
            _CVRWindow.CVRInitAction += _CVR.InitComm;
            _CVRWindow.CVRReadAction += _CVR.Authenticate;
            _CVRWindow.CVRForReadAction += _CVR.AuthenticateFor;
            _CVRWindow.CVRWhileReadAction += _CVR.AuthenticateWhile;
            _CVRWindow.CVRCloseAction += _CVR.CloseComm;
            _CVRWindow.CVRSetCVRvolatile += _CVR.GetStarted;
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
                GateWindowActiveInit();
            }
            SetTabPate("GateTable", GateTable, form: _GateWindow);
        }

        /// <summary>
        /// 界面委托
        /// </summary>
        private void GateWindowActiveInit()
        {
            _Gate.SetMessage += _GateWindow.SetStatusText;
            _GateWindow.GateOpenDoorAction += _Gate.OpenDoor;
        }

        /// <summary>
        /// 道闸链接事件订阅
        /// </summary>
        /// <param name="i"></param>
        private void _Gate_NewState(object sender, Gate.DoorStateEventArgs e)
        {
            if (e.State == 1)
            {
                if (e.SN == Int32.Parse(Properties.Settings.Default.Gate_InDoorSN))
                {
                    GateStatus(e.State, e.SN);
                }
                if (e.SN == Int32.Parse(Properties.Settings.Default.Gate_OutDoorSN))
                {
                    GateStatus(e.State, e.SN);
                }
            }
            else
            {
                if (e.SN == Int32.Parse(Properties.Settings.Default.Gate_InDoorSN))
                {
                    GateStatus(e.State, e.SN);
                }
                if (e.SN == Int32.Parse(Properties.Settings.Default.Gate_OutDoorSN))
                {
                    GateStatus(e.State, e.SN);
                }
            }
        }

        private void  GateStatus(int Status ,Int32 SN)
        {
            if (statusStrip2.InvokeRequired)
            {
                statusStrip2.Invoke(new UpdateUiUInvok(GateStatus), new object[] { Status, SN });
            }
            else
            {
                if (SN == Int32.Parse(Properties.Settings.Default.Gate_InDoorSN))
                {
                    if (Status == 1)
                    {
                        toolStripStatusLabel4.BackColor = Color.DarkGreen;
                    }
                    else
                    {
                        toolStripStatusLabel4.BackColor = Color.DarkRed;
                    }
                }
                if (SN == Int32.Parse(Properties.Settings.Default.Gate_OutDoorSN))
                {
                    if (Status == 1)
                    {
                        toolStripStatusLabel5.BackColor = Color.DarkGreen;
                    }
                    else
                    {
                        toolStripStatusLabel5.BackColor = Color.DarkRed;
                    }
                }
            }
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

        #region//本地数据库

        /// <summary>
        /// 数据库显示界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LocalDataWindowShow_Click(object sender, EventArgs e)
        {
            if (_LocalDataBaseWindow == null || _LocalDataBaseWindow.IsDisposed)
            {
                _LocalDataBaseWindow = new LocalDataBase.LocalDataBaseWindow();
                LocalDataWindowActiveInit();
            }
            SetTabPate("LocalTable", LocalTable, form: _LocalDataBaseWindow);
        }

        /// <summary>
        /// 数据库界面初始化
        /// </summary>
        private void LocalDataWindowActiveInit()
        {
            //string[] message= _LocalDataBase.SelectData("123", "", "");
            _LocalDataBase.SetMessage += _LocalDataBaseWindow.SetStatusText;
        }

        #endregion

        #region//进闸数据库
        /// <summary>
        /// 数据库显示界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InDataWindowShow_Click(object sender, EventArgs e)
        {
            if (_InDataWindow == null || _InDataWindow.IsDisposed)
            {
                _InDataWindow = new IEDataBase.InDataWindow();
            }
            SetTabPate("InTable", InTable, form: _InDataWindow);
        }
        #endregion

        #region//出闸数据库
        /// <summary>
        /// 数据库显示界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutDataWindowShow_Click(object sender, EventArgs e)
        {
            if (_OutDataWindow == null || _OutDataWindow.IsDisposed)
            {
                _OutDataWindow = new IEDataBase.OutDataWindow();
            }
            SetTabPate("OutTable", OutTable, form: _OutDataWindow);
        }
        #endregion

        #region//Https
        /// <summary>
        /// 数据库显示界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HttpWindowShow_Click(object sender, EventArgs e)
        {
            if (_HttpWindow == null || _HttpWindow.IsDisposed)
            {
                _HttpWindow = new Https.HttpWindow();
            }
            SetTabPate("HttpTable", HttpTable, form: _HttpWindow);
        }
        #endregion
    }
}
