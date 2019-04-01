using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Threading.Tasks;
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
            if (dataGridView1.Rows.Count > 0)
            {
                string saveFileName = "";
                //bool fileSaved = false;  
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    DefaultExt = "xls",
                    Filter = "Excel文件|*.xls",
                    FileName = string.Format("StatisticsDataBase_{0:yyyyMMddHHmmss}.xls", DateTime.Now)
                };
                saveDialog.ShowDialog();
                saveFileName = saveDialog.FileName;
                if (saveFileName.IndexOf(":") < 0)
                {
                    return; //被点了取消   
                }
                else
                {
                    var t1 = new Task(TaskMethod, saveFileName, TaskCreationOptions.LongRunning);
                    t1.Start();
                }
            }
            else
            {
                MessageBox.Show("报表为空,无表格需要导出", "提示", MessageBoxButtons.OK);
            }
        }

        static object taskMethodLocj = new object();
        static void TaskMethod(object title)
        {
            lock (taskMethodLocj)
            {
                DataGridView dataGridView1 = StatisticsWindow._Statistics.dataGridView1;

                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                if (xlApp == null)
                {
                    MessageBox.Show("无法创建Excel对象，可能您的机子未安装Excel");
                    return;
                }
                Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
                Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];//取得sheet1  

                //写入标题  
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    worksheet.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
                }
                //写入数值  
                for (int r = 0; r < dataGridView1.Rows.Count; r++)
                {
                    for (int i = 0; i < dataGridView1.ColumnCount; i++)
                    {
                        if (i == 7)
                        {
                            worksheet.Cells[r + 2, i + 1] = "'" + dataGridView1.Rows[r].Cells[i].Value;
                        }
                        else
                        {
                            worksheet.Cells[r + 2, i + 1] = dataGridView1.Rows[r].Cells[i].Value;
                        }
                    }
                    Application.DoEvents();
                }
                worksheet.Columns.EntireColumn.AutoFit();//列宽自适应  
                                                         //   if (Microsoft.Office.Interop.cmbxType.Text != "Notification")  
                                                         //   {  
                                                         //       Excel.Range rg = worksheet.get_Range(worksheet.Cells[2, 2], worksheet.Cells[ds.Tables[0].Rows.Count + 1, 2]);  
                                                         //      rg.NumberFormat = "00000000";  
                                                         //   }  

                if (title.ToString() != "")
                {
                    try
                    {
                        workbook.Saved = true;
                        workbook.SaveCopyAs(title.ToString());
                        //fileSaved = true;  
                    }
                    catch (Exception ex)
                    {
                        //fileSaved = false;  
                        MessageBox.Show("导出文件时出错,文件可能正被打开！\n" + ex.Message);
                    }

                }
                //else  
                //{  
                //    fileSaved = false;  
                //}  
                xlApp.Quit();
                GC.Collect();//强行销毁   
                             // if (fileSaved && System.IO.File.Exists(saveFileName)) System.Diagnostics.Process.Start(saveFileName); //打开EXCEL  
                MessageBox.Show("导出文件成功", "提示", MessageBoxButtons.OK);
            }
        }    
    }
}
