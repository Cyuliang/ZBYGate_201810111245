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
        public Func<string, string, string, string,string> SetJsonAction;

        public HttpWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestButton_Click(object sender, EventArgs e)
        {
        }
    }       
}
