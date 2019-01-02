using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZBYGate_Data_Collection.IEDataBase
{
    public partial class InDataWindow : Form
    {
        public static InDataWindow InData_;

        public InDataWindow()
        {
            InitializeComponent();

            InData_ = this;

            dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.ShowUpDown = true;
            dateTimePicker1.Value = DateTime.Now.Date;
            dateTimePicker2.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.ShowUpDown = true;
            dateTimePicker2.Value = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            DataradioButton.Checked = true;
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FindButton_Click(object sender, EventArgs e)
        {
            string cmdText = string.Empty;
            if (DataradioButton.Checked)
            {
                if (PlateTextBox.Text != string.Empty && ContainerTextBox.Text != string.Empty)
                {
                    cmdText = string.Format("SELECT *  FROM hw.rundata WHERE Plate='{0}' and Container='{1}'", PlateTextBox.Text, ContainerTextBox.Text);
                }
                else
                {
                    if (PlateTextBox.Text != string.Empty)
                    {
                        cmdText = string.Format("SELECT *  FROM hw.rundata WHERE Plate='{0}'", PlateTextBox.Text);
                    }
                    else
                    {
                        if (ContainerTextBox.Text != string.Empty)
                        {
                            cmdText = string.Format("SELECT *  FROM hw.indata WHERE Container='{0}'", ContainerTextBox.Text);
                        }
                        else
                        {
                            MessageBox.Show("请输入要查询的数据！");
                            return;
                        }
                    }
                }
            }
            if (TimeradioButton.Checked)
            {
                cmdText = string.Format("SELECT *  FROM hw.rundata WHERE  InDatetime between '{0}' and '{1}'", dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss"), dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            MySqlDataReader reader = LocalDataBase.MySqlHelper.ExecuteReader(LocalDataBase.MySqlHelper.Conn, CommandType.Text, cmdText, null);
            bindingSource1.DataSource = reader;
            bindingNavigator1.BindingSource = bindingSource1;
            dataGridView1.DataSource = bindingSource1;
            reader.Close();
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

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripButton1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                string saveFileName = "";
                //bool fileSaved = false;  
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    DefaultExt = "xls",
                    Filter = "Excel文件|*.xls",
                    FileName = string.Format("InData_{0:yyyyMMddHHmmss}.xls", DateTime.Now)
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
                DataGridView dataGridView1 = InDataWindow.InData_.dataGridView1;

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
