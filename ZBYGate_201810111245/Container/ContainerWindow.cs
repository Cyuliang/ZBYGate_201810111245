using AxVeconclientProj;
using System.Windows.Forms;

namespace ZBYGate_201810111245.Container
{
    public partial class ContainerWindow : Form
    {
        public ContainerWindow()
        {
            InitializeComponent();            
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
    }
}
