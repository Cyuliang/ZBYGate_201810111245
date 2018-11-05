using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZBYGate_201810111245.LED
{
    public partial class LEDWindow : Form
    {
        public Action<int> InitAction;
        public Action<int> AddScreenAction;
        public Action<int> AddAreaAction;
        public Action<string[]> AddTextAction;
        public Action<int> SendAction;
        public Action<int> UnintAction;

        private delegate void UpdateUiInvok(string Message);//跨线程更新UI
        private System.Threading.Timer _timer=null;//定时恢复状态

        public LEDWindow()
        {
            InitializeComponent();

            int i = 1;
            foreach(object _Control in toolStrip1.Items)
            {       
                if(_Control is ToolStripButton)
                {
                    ToolStripButton _ToolStripButton = (ToolStripButton)_Control;
                    _ToolStripButton.Tag = i;
                    i++;
                }
            }

            IpTextBox.Text = Properties.Settings.Default.LED_pSocketIP;
            PortTextBox.Text = Properties.Settings.Default.LED_nSocketPort.ToString();
            _timer = new System.Threading.Timer(ClearText, null, TimeSpan.FromSeconds(5),TimeSpan.FromSeconds(0));
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
                case 1:InitAction?.Invoke(0);//初始化动态库
                    break;
                case 2:AddScreenAction?.Invoke(0);//添加显示屏
                    break;
                case 3:AddAreaAction?.Invoke(0);//添加动态区域
                    break;
                case 4:
                    string[] pTexts = Control();
                    if(pTexts!=null)
                    {
                        AddTextAction?.Invoke(pTexts);//添加文本
                    }
                    break;
                case 5: SendAction?.Invoke(0);//推送消息
                    break;
                case 6:
                    toolStripButton5.Enabled = true;
                    UnintAction?.Invoke(0);//释放动态库
                    break;
            }
        }

        /// <summary>
        /// 状态栏显示平息
        /// </summary>
        /// <param name="Message">动作信息</param>
        public void SetStatusText(string Message)
        {
            if (statusStrip1.InvokeRequired)
            {
                statusStrip1.Invoke(new UpdateUiInvok(SetStatusText), new object[] { Message });
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
            _timer.Change(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(0));
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
                if (con is TextBox box)
                {
                    if (box.Text.Trim() == string.Empty)
                    {
                        toolStripButton5.Enabled = false;
                        MessageBox.Show("所有参数不能为空");
                        return pTexts;
                    }
                }
            }
            toolStripButton5.Enabled = true;
            pTexts = new string[]{ PlateTextBox.Text, SupplierTextBox.Text, AppointmentTextBox.Text, ParkedTextBox.Text, OntimeTextBox.Text };
            return pTexts;
        }
    }
}
