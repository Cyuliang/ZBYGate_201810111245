using AxVeconclientProj;
using System;
using System.Windows.Forms;

namespace ZBYGate_Data_Collection.Container
{
    public partial class ContainerWindow : Form
    {
        public Action<int> ContainerLinkAction;//链接箱号服务端
        public Action<int> ContainerAbortAction;//断开链接
        public Action<int> ContainerLastRAction;//获取最后一次结果

        private delegate void UpdateUiInvok(string Message);//跨线程更新UI
        private int Gate = 0;

        private System.Threading.Timer _Timer = null;

        public ContainerWindow()
        {
            InitializeComponent();

            SetObjectTag();

            _Timer = new System.Threading.Timer(ClearText, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(0));

            Gate = Properties.Settings.Default.Container_Num;
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
                statusStrip1?.Invoke(new UpdateUiInvok(SetStatusText), new object[] { Message });
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
        /// 空车车牌
        /// </summary>
        /// <param name="e"></param>
        public void NewLPN(IVECONclientEvents_OnNewLPNEventEvent e)
        {
            textBox6.Text = e.laneNum.ToString();
            textBox7.Text = e.triggerTime.ToString("yyyy-MM-dd HH:mm:ss");
            textBox8.Text = e.lPN;
            textBox9.Text = e.colorCode.ToString();
        }

        /// <summary>
        /// 重车车牌
        /// </summary>
        /// <param name="e"></param>
        public void UpdateLPN(IVECONclientEvents_OnUpdateLPNEventEvent e)
        {
            textBox6.Text = e.laneNum.ToString();
            textBox7.Text = e.triggerTime.ToString("yyyy-MM-dd HH:mm:ss");
            textBox8.Text = e.lPN;
            textBox9.Text = e.colorCode.ToString();
        }

        /// <summary>
        /// 箱号结果集
        /// </summary>
        /// <param name="e"></param>
        public void CombinResult(IVECONclientEvents_OnCombinedRecognitionResultISOEvent e)
        {
            textBox1.Text = e.laneNum.ToString();
            textBox2.Text = e.triggerTime.ToString("yyyy-MM-dd HH:mm:ss");
            textBox3.Text = e.containerNum1;
            textBox11.Text = e.iSO1;
            textBox13.Text = e.checkSum1;
            textBox4.Text = e.containerNum2;
            textBox10.Text = e.iSO2;
            textBox12.Text = e.checkSum2;
            switch (e.containerType)
            {
                case -1: textBox5.Text = "未知"; break;
                case 0: textBox5.Text = "20 吋集装箱"; break;
                case 1: textBox5.Text = "40 吋集装箱"; break;
                case 2: textBox5.Text = "两个 20 吋集装箱"; break;
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
                    ContainerLinkAction?.Invoke(0);      
                    break;
                case 2:
                    ContainerAbortAction?.Invoke(0);                    
                    break;
                case 3:
                    ContainerLastRAction?.Invoke(Gate);
                    break;
            }
        }

        /// <summary>
        ///设置控件对象数据
        /// </summary>
        private void SetObjectTag()
        {
            int i = 1;
            foreach (object _Control in toolStrip1.Items)
            {
                if (_Control is ToolStripButton _ToolStripButton)
                {
                    _ToolStripButton.Tag = i;
                    i++;
                }
            }
        }
    }
}
