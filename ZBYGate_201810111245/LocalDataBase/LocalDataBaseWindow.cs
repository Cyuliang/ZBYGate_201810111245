using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZBYGate_Data_Collection.LocalDataBase
{
    public partial class LocalDataBaseWindow : Form
    {
        private Log.CLog _Log = new Log.CLog();
        private delegate void UpdateUiInvok(string Message);//跨线程更新UI

        private System.Threading.Timer _Timer = null;

        public LocalDataBaseWindow()
        {
            InitializeComponent();
            Init();
            _Timer = new System.Threading.Timer(ClearText, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(0));
        }

        private void Init()
        {
            dataSet1 = MySqlHelper.GetDataSet(MySqlHelper.Conn, System.Data.CommandType.Text, "select * from hw.gate", null);
            bindingSource1.DataSource = dataSet1.Tables[0];
            bindingNavigator1.BindingSource = bindingSource1;
            dataGridView1.DataSource = bindingSource1;

            dataGridView1.Columns[0].Visible = false;
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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //int row = dataGridView1.NewRowIndex-1;
            //string cmdText = string.Format("INSERT INTO `hw`.`gate` " +
            //    "(`Plate`, `Container`, `Supplier`, `Appointment`, `Parked`, `Ontime`, `Cards`, `Truetime`) " +
            //    "VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}')", 
            //    dataGridView1.Rows[row].Cells[1].Value,
            //    dataGridView1.Rows[row].Cells[2].Value,
            //    dataGridView1.Rows[row].Cells[3].Value,
            //    dataGridView1.Rows[row].Cells[4].Value,
            //    dataGridView1.Rows[row].Cells[5].Value,
            //    dataGridView1.Rows[row].Cells[6].Value,
            //    dataGridView1.Rows[row].Cells[7].Value,
            //    dataGridView1.Rows[row].Cells[8].Value.ToString());
            //MySqlHelper.DataPersistence(MySqlHelper.Conn, System.Data.CommandType.Text, cmdText, null);
            int rowindex = dataGridView1.CurrentCell.RowIndex;
            ItemDataWindow dataItem = new ItemDataWindow();
            dataItem.UpdataUi(int.Parse(dataGridView1.Rows[rowindex].Cells[0].Value.ToString()));
            dataItem.ShowDialog();
            Init();
            dataGridView1.CurrentCell = dataGridView1.Rows[rowindex].Cells[1];
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            int rowindex = dataGridView1.CurrentCell.RowIndex;
            DialogResult but = MessageBox.Show(string.Format("Confirm deletion Plate={0}  Container={1}  Card={2} ? ", dataGridView1.Rows[rowindex].Cells[1].Value.ToString(), dataGridView1.Rows[rowindex].Cells[2].Value.ToString(), dataGridView1.Rows[rowindex].Cells[7].Value.ToString()), "提示", MessageBoxButtons.YesNo);
            if (but == DialogResult.Yes)
            {
                try
                {
                    string drop = string.Format("DELETE FROM `hw`.`gate` WHERE (`Id` = '{0}')", dataGridView1.Rows[rowindex].Cells[0].Value.ToString());
                    MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, System.Data.CommandType.Text, drop, null);
                    Init();
                    _Log.logInfo.Info(drop);
                    MessageBox.Show("Delete Success!");
                }
                catch (Exception ex)
                {
                    _Log.logError.Error("Delete Fail", ex);
                }
            }
            else if (but == DialogResult.No)
            {
                MessageBox.Show("Nothing!!!");
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = MySqlHelper.GetDataSet(MySqlHelper.Conn, System.Data.CommandType.Text, "select * from gate", null).Tables[0].DefaultView;
            ItemDataWindow dataItem = new ItemDataWindow();
            dataItem.ShowDialog();
            Init();
            dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1];
        }
    }
}
