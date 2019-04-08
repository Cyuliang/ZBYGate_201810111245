using System;
using System.Windows.Forms;

namespace ZBYGate_Data_Collection.LED
{
    public partial class LEDWindow : Form
    {
        public Action<int> InitAction;//初始化动态库
        public Action<int> AddScreenAction;//添加显示屏
        public Action<int> AddAreaAction;//添加动态区
        public Action<string[]> AddTextAction;//添加文本
        public Action<int> SendAction;//推送消息
        public Action<int> UnintAction;//卸载动态库

        private delegate void UpdateUiDelegate(string Message);//跨线程更新UI

        private System.Threading.Timer _Timer;

        public LEDWindow()
        {
            InitializeComponent();
            SetObjectTag(true);

            _Timer = new System.Threading.Timer(ClearText, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(0));

            IpTextBox.Text = Properties.Settings.Default.LED_pSocketIP;
            PortTextBox.Text = Properties.Settings.Default.LED_nSocketPort.ToString();
        }

        /// <summary>
        /// 公共工具栏按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolstripButton_Click(object sender,EventArgs e)
        {
            ToolStripButton _ToolStripButton = (ToolStripButton)sender;
            switch(int.Parse(_ToolStripButton.Tag.ToString()))
            {
                case 1:InitAction?.Invoke(0);      
                    SetObjectTag(false);
                    break;
                case 2:AddScreenAction?.Invoke(0);                    
                    break;
                case 3:AddAreaAction?.Invoke(0);
                    break;
                case 4:
                    if (Control()!=null)
                    {
                        AddTextAction?.Invoke(Control());
                    }
                    break;
                case 5:
                    SendAction?.BeginInvoke(0, new AsyncCallback(BeginInvokeCallBack), null);
                    //SendAction?.Invoke(0);
                    break;
                case 6:
                    UnintAction?.Invoke(0);
                    SetObjectTag(true);
                    break;
            }
        }

        /// <summary>
        /// 异步完成回调
        /// </summary>
        /// <param name="ar"></param>
        private void BeginInvokeCallBack(IAsyncResult ar)
        {         
            SetStatusText("推送信息完成");
        }

        /// <summary>
        /// 状态栏显示平息
        /// </summary>
        /// <param name="Message">动作信息</param>
        public void SetStatusText(string Message)
        {
            if (statusStrip1.InvokeRequired)
            {
                statusStrip1.Invoke(new UpdateUiDelegate(SetStatusText), new object[] { Message });
            }
            else
            {
                StatusLabel.Text = Message;
            }
        }

        /// <summary>
        /// 文本发生变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusLabel_TextChanged(object sender, EventArgs e)
        {
            _Timer.Change(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(0));
        }

        /// <summary>
        /// 定时回调函数
        /// </summary>
        /// <param name="state"></param>
        private void ClearText(object state)
        {
            SetStatusText("就绪");
        }

        /// <summary>
        /// 遍历窗口控件
        /// </summary>
        private string[] Control()
        {
            string[] pTexts = null;
            //遍历TextBox
            foreach (Control con in this.Controls)
            {
                if (con is TextBox )
                {
                    TextBox box = new TextBox();
                    if (box.Text.Trim() == string.Empty)
                    {
                        toolStripButton5.Enabled = false;
                        MessageBox.Show("所有参数不能为空");
                        return pTexts;
                    }
                }
            }
            toolStripButton5.Enabled = true;
            pTexts = new string[]{ PlateTextBox.Text, SupplierTextBox.Text, AppointmentTextBox.Text, ParkedTextBox.Text, OntimeTextBox.Text,LogtextBox.Text };
            return pTexts;
        }

        /// <summary>
        ///设置控件对象数据
        /// </summary>
        private void SetObjectTag(bool SetEnable)
        {
            int i = 1;
            foreach (object _Control in toolStrip1.Items)
            {
                if (_Control is ToolStripButton)
                {
                    ToolStripButton _ToolStripButton = new ToolStripButton();
                    _ToolStripButton.Tag = i;
                    if (SetEnable)
                    {
                        if (i != 1)
                        {
                            _ToolStripButton.Enabled = false;
                        }
                    }
                    else
                    {
                        _ToolStripButton.Enabled = true;
                    }
                    i++;
                }
            }
        }
    }
}
