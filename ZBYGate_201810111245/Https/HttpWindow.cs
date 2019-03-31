using System;
using System.Windows.Forms;

namespace ZBYGate_Data_Collection.Https
{
    public partial class HttpWindow : Form
    {
        public Func<string, string, string,string> SetJsonAction;
        public Func<string, string[]> JsonSplitAction;

        public HttpWindow()
        {
            InitializeComponent();

            dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.ShowUpDown = true;

            IDtextBox.Text = Properties.Settings.Default.Http_eqId;
            ArriveParkTimeradioButton.Checked = true;//默认入园
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestButton_Click(object sender, EventArgs e)
        {
            string Type = string.Empty;

            if (ArrivePortalTimeradioButton.Checked)
            {
                Type = "1";
            }
            if (LeavePortalTimeradioButton.Checked)
            {
                Type = "2";
            }
            if (ArriveParkTimeradioButton.Checked)
            {
                Type = "3";
            }
            if(LeaveParkTimeradioButton.Checked)
            {
                Type = "4";
            }

            string tmp = string.Empty;
            tmp= SetJsonAction?.Invoke(dateTimePicker1.Value.ToString("yyyyMMddHHmmss"), PlatetextBox.Text, ContainertextBox.Text+"|"+Type);
            if(!string.IsNullOrEmpty(tmp))
            {
                try
                {
                    string[] ReturnData = JsonSplitAction?.Invoke(tmp);
                    textBox1.Text = ReturnData[0];
                    textBox2.Text = ReturnData[1];
                    textBox3.Text = ReturnData[2];
                    textBox4.Text = ReturnData[3];
                    textBox5.Text = ReturnData[4];
                    textBox6.Text = ReturnData[5];
                    textBox7.Text = ReturnData[6];
                    textBox8.Text = ReturnData[7];
                    textBox9.Text = ReturnData[8];
                    textBox10.Text = ReturnData[9];
                }
                catch (Exception)
                {
                    MessageBox.Show(string.Format("http 返回数据不满足协议：{0}",tmp));
                }

            }
            else
            {
                ;
            }
        }
    }       
}
