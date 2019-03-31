using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace ZBYGate_Data_Collection.IEDataBase
{
    public partial class StatisticsWindow : Form
    {
        public static StatisticsWindow _Statistics;

        public StatisticsWindow()
        {
            InitializeComponent();

            _Statistics = this;

            DateTime Sd = new DateTime(DateTime.Now .Year, DateTime.Now.Month, 1);
            DateTime Ed = Sd.AddMonths(1).AddDays(-1);

            dateTimePicker1.Value = Sd;
            dateTimePicker2.Value = Ed;

            TimeradioButton.Checked = true;
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FindButton_Click(object sender, System.EventArgs e)
        {
            string cmdText = string.Empty;
            if (TimeradioButton.Checked)
            {
                cmdText = string.Format("SELECT *  FROM `hw`.`traffic` WHERE `Date` between '{0}' and '{1}'", dateTimePicker1.Value.ToString("yyyy-MM-dd"), dateTimePicker2.Value.ToString("yyyy-MM-dd"));
            }
            if(radioButton1.Checked)//结余
            {
                cmdText = string.Format("SELECT *  FROM `hw`.`traffic` WHERE `Balance` between '{0}' and '{1}'", StartTextBox.Text,EndtextBox.Text);
            }
            if (radioButton2.Checked)//进闸
            {
                cmdText = string.Format("SELECT *  FROM `hw`.`traffic` WHERE `In` between '{0}' and '{1}'", StartTextBox.Text, EndtextBox.Text);
            }
            if (radioButton3.Checked)//出闸
            {
                cmdText = string.Format("SELECT *  FROM `hw`.`traffic` WHERE `Out` between '{0}' and '{1}'", StartTextBox.Text, EndtextBox.Text);
            }

            MySqlDataReader reader = LocalDataBase.MySqlHelper.ExecuteReader(LocalDataBase.MySqlHelper.Conn, CommandType.Text, cmdText, null);
            bindingSource1.DataSource = reader;
            bindingNavigator1.BindingSource = bindingSource1;
            dataGridView1.DataSource = bindingSource1;
            reader.Close();
        }

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton2_Click(object sender, System.EventArgs e)
        {

        }
    }
}
