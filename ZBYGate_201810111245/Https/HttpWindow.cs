using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            IDtextBox.Text = Properties.Settings.Default.Http_eqId;
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestButton_Click(object sender, EventArgs e)
        {
            string tmp = string.Empty;
            tmp= SetJsonAction?.Invoke(TimetextBox.Text, PlatetextBox.Text, ContainertextBox.Text);
            if(!string.IsNullOrEmpty(tmp))
            {
                string[] ReturnData = JsonSplitAction?.Invoke(tmp);
                textBox5.Text = ReturnData[0];
                textBox8.Text = ReturnData[2];
                textBox9.Text = ReturnData[3];
                textBox10.Text = ReturnData[1];
                textBox1.Text = ReturnData[4];
            }
        }
    }       
}
