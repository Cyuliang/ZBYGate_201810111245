using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace ZBYGate_Data_Collection.LocalDataBase
{
    public partial class LocalDataBaseWindow : Form
    {
        private Log.CLog _Log = new Log.CLog();
        private delegate void UpdateUiDelegate(string Message);//跨线程更新UI
        private System.Threading.Timer _Timer;

        public LocalDataBaseWindow()
        {
            InitializeComponent();
            LoadData();

            _Timer = new System.Threading.Timer(ClearText, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(0));
        }

        private void LoadData()
        {
            MySqlDataReader reader = MySqlHelper.ExecuteReader(MySqlHelper.Conn, CommandType.Text, "select * from gate", null);
            bindingSource1.DataSource = reader;
            bindingNavigator1.BindingSource = bindingSource1;
            dataGridView1.DataSource = bindingSource1;
            reader.Close();
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
        /// 编辑数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripButton1_Click(object sender, EventArgs e)
        {
            int rowindex = dataGridView1.CurrentCell.RowIndex;
            ItemDataWindow dataItem = new ItemDataWindow();
            dataItem.UpdataUi(int.Parse(dataGridView1.Rows[rowindex].Cells[0].Value.ToString()));
            dataItem.Text = "本地数据库-【编辑数据】";
            dataItem.ShowDialog();

            LoadData();
            dataGridView1.CurrentCell = dataGridView1.Rows[rowindex].Cells[1];
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            int rowindex = dataGridView1.CurrentCell.RowIndex;
            DialogResult but = MessageBox.Show(string.Format("Confirm deletion Plate={0}  Container={1}  Card={2} ? ", dataGridView1.Rows[rowindex].Cells[1].Value.ToString(), dataGridView1.Rows[rowindex].Cells[2].Value.ToString(), dataGridView1.Rows[rowindex].Cells[7].Value.ToString()), "提示", MessageBoxButtons.YesNo);
            if (but == DialogResult.Yes)
            {
                try
                {
                    string drop = string.Format("DELETE FROM `hw`.`gate` WHERE (`Id` = '{0}')", dataGridView1.Rows[rowindex].Cells[0].Value.ToString());
                    MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, System.Data.CommandType.Text, drop, null);

                    LoadData();
                    _Log.logInfo.Info(drop);
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

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripButton3_Click(object sender, EventArgs e)
        {
            ItemDataWindow dataItem = new ItemDataWindow();
            dataItem.Text="本地数据库-【添加数据】";
            dataItem.ShowDialog();

            LoadData();
            dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1];
        }

        /// <summary>
        /// 查找数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FindButton_Click(object sender, EventArgs e)
        {
            string cmdText = string.Format("SELECT *  FROM `hw`.`gate` WHERE Plate='{0}' or Container='{1}' or Cards='{2}'", FindTextBox.Text, FindTextBox.Text, FindTextBox.Text);
            MySqlDataReader reader = MySqlHelper.ExecuteReader(MySqlHelper.Conn, CommandType.Text, cmdText, null);
            if (reader.Read())
            {
                //MessageBox.Show(String.Format("Find Data\n Plate={0}\n Container={1}\n Cards={2}\n", reader[1].ToString(), reader[2].ToString(), reader[7].ToString()));
                if (reader[1].ToString() != string.Empty)
                {
                    ReturnDataView(reader[1].ToString(), 1);
                }
                else if (reader[2].ToString() != string.Empty)
                {
                    ReturnDataView(reader[2].ToString(), 2);
                }
                else
                {
                    if (reader[7].ToString() != string.Empty)
                    {
                        ReturnDataView(reader[7].ToString(), 7);
                    }
                }
            }
            else
            {
                MessageBox.Show("Not Find Data");
            }
        }

        /// <summary>
        /// 循环判断数据
        /// </summary>
        /// <param name="reader"></param>
        private void ReturnDataView(string reader, int index)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (reader == dataGridView1.Rows[i].Cells[index].Value.ToString())
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[index];
                    break;
                }
            }
        }
    }
}
