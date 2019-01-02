using System;
using System.Windows.Forms;

namespace ZBYGate_Data_Collection.Gate
{
    public partial class GateWindow : Form
    {
        public Action<string, int, string> GateOpenDoorAction;

        private delegate void UpdateUiDelegate(string Message);//跨线程更新UI

        private System.Threading.Timer _Timer;

        public GateWindow()
        {
            InitializeComponent();
            SetObjectTag();

            _Timer = new System.Threading.Timer(ClearText, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(0));

            textBox1.Text = Properties.Settings.Default.Gate_InDoorIp;
            textBox2.Text = Properties.Settings.Default.Gate_Port.ToString();
            textBox3.Text = Properties.Settings.Default.Gate_InDoorSN;
            textBox4.Text = Properties.Settings.Default.Gate_OutDoorIp;
            textBox5.Text = Properties.Settings.Default.Gate_Port.ToString();
            textBox6.Text = Properties.Settings.Default.Gate_OutDoorSN;
        }

        /// <summary>
        ///设置控件对象数据
        /// </summary>
        private void SetObjectTag()
        {
            int i = 1;
            foreach (object _Control in toolStrip1.Items)
            {
                if (_Control is ToolStripButton)
                {
                    ToolStripButton _ToolStripButton = (ToolStripButton)_Control;
                    _ToolStripButton.Tag = i;
                    i++;
                }
            }
        }

        /// <summary>
        /// 公共工具栏按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolstripButton_Click(object sender, EventArgs e)
        {
            ToolStripButton _ToolStripButton = (ToolStripButton)sender;
            switch (int.Parse(_ToolStripButton.Tag.ToString()))
            {
                case 1:
                    GateOpenDoorAction?.Invoke(Properties.Settings.Default.Gate_InDoorIp, Properties.Settings.Default.Gate_Port, Properties.Settings.Default.Gate_InDoorSN);//进闸开门
                    break;
                case 2:
                    GateOpenDoorAction?.Invoke(Properties.Settings.Default.Gate_OutDoorIp, Properties.Settings.Default.Gate_Port, Properties.Settings.Default.Gate_OutDoorSN);//出闸开门
                    break;
            }
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
    }
}
