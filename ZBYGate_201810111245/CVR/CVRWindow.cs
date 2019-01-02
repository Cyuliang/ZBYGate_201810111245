using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ZBYGate_Data_Collection.CVR
{
    public partial class CVRWindow : Form
    {
        public Action<int> CVRInitAction;//初始化动态库
        public Action<int> CVRReadAction;//寻卡·读卡
        public Action<int> CVRCloseAction;//关闭读取
        public Action<int> CVRWhileReadAction;//循环读取
        public Action<int> CVRForReadAction;//定时读取
        public Action<bool> CVRSetCVRvolatileAction;//设置定时读取状态

        private volatile bool ReadForBooen = true;//定时循环状态
        private System.Threading.Timer _Timer;

        #region//更新UI
        private delegate void UpdateUiDelegate(string Message);//跨线程更新UI
        private delegate void UpdateCVRDelegate(byte[] name, byte[] sex, byte[] peopleNation, byte[] birthday, byte[] number, byte[] address, byte[] signdate, byte[] validtermOfStart, byte[] validtermOfEnd);
        private delegate void UpdateCVRImageDelegate(byte[] imgData, int length);
        #endregion

        public CVRWindow()
        {
            InitializeComponent();
            SetObjectTag();

            _Timer = new System.Threading.Timer(ClearText, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(0));
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
        /// 读取数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sex"></param>
        /// <param name="peopleNation"></param>
        /// <param name="birthday"></param>
        /// <param name="number"></param>
        /// <param name="address"></param>
        /// <param name="signdate"></param>
        /// <param name="validtermOfStart"></param>
        /// <param name="bCivic"></param>
        public void FillData(byte[] name, byte[] sex, byte[] peopleNation, byte[] birthday, byte[] number, byte[] address, byte[] signdate, byte[] validtermOfStart, byte[] validtermOfEnd)
        {
            if(textBox1.InvokeRequired)
            {
                textBox1?.Invoke(new UpdateCVRDelegate(FillData), new object[] { name, sex,peopleNation,birthday,number,address,signdate,validtermOfStart,validtermOfEnd });
            }
            else
            {
                textBox1.Text = System.Text.Encoding.GetEncoding("GB2312").GetString(name);
                textBox2.Text = System.Text.Encoding.GetEncoding("GB2312").GetString(sex).Replace("\0", "").Trim();
                textBox8.Text = System.Text.Encoding.GetEncoding("GB2312").GetString(peopleNation).Replace("\0", "").Trim();
                textBox3.Text = System.Text.Encoding.GetEncoding("GB2312").GetString(birthday).Replace("\0", "").Trim();
                textBox4.Text = System.Text.Encoding.GetEncoding("GB2312").GetString(number).Replace("\0", "").Trim();
                textBox5.Text = System.Text.Encoding.GetEncoding("GB2312").GetString(address).Replace("\0", "").Trim();
                textBox6.Text = System.Text.Encoding.GetEncoding("GB2312").GetString(signdate).Replace("\0", "").Trim();
                textBox7.Text = System.Text.Encoding.GetEncoding("GB2312").GetString(validtermOfStart).Replace("\0", "").Trim() + "-" + System.Text.Encoding.GetEncoding("GB2312").GetString(validtermOfEnd).Replace("\0", "").Trim();
            }
        }

        /// <summary>
        /// 读取头像
        /// </summary>
        /// <param name="imgData"></param>
        public void FillDataBmp(byte[] imgData, int length)
        {
            if(pictureBoxPhoto.InvokeRequired)
            {
                pictureBoxPhoto?.Invoke(new UpdateCVRImageDelegate(FillDataBmp), new object[] { imgData, length });
            }
            else
            {
                MemoryStream myStream = new MemoryStream();
                for (int i = 0; i < length; i++)
                {
                    myStream.WriteByte(imgData[i]);
                }
                Image myImage = Image.FromStream(myStream);            
                pictureBoxPhoto.Image = myImage;
                myStream.Seek(0, SeekOrigin.Begin);
                myStream.SetLength(0);
                myStream.Close();
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
                    CVRInitAction?.BeginInvoke(0, new AsyncCallback(InitCalllBack), null);
                    break;
                case 2:
                    CVRReadAction?.Invoke(0);
                    break;
                case 3:
                    CVRCloseAction?.Invoke(0);
                    break;
                case 4:
                    if(_ToolStripButton.Checked)
                    {
                        CVRSetCVRvolatileAction?.Invoke(true);
                        CVRWhileReadAction?.BeginInvoke(0, new AsyncCallback(CallWhileDone), null);
                    }
                    else//取消选中停止循环
                    {
                        CVRSetCVRvolatileAction?.Invoke(false);
                    }
                    break;
                case 5:
                    if (ReadForBooen)
                    {
                        CVRForReadAction?.BeginInvoke(0, new AsyncCallback(CallForDone), null);
                        ReadForBooen = false;
                    }
                    break;
            }
        }

        /// <summary>
        /// 初始化回调完成
        /// </summary>
        /// <param name="ar"></param>
        private void InitCalllBack(IAsyncResult ar)
        {
            SetStatusText("初始化动作完成");
        }

        /// <summary>
        /// 定时异步回到完成
        /// </summary>
        /// <param name="ar"></param>
        private void CallForDone(IAsyncResult ar)
        {
            SetStatusText("循环读取完成");
            ReadForBooen = true;
        }

        /// <summary>
        /// 循环异步回调完成
        /// </summary>
        /// <param name="ar"></param>
        private void CallWhileDone(IAsyncResult ar)
        {
            SetStatusText("循环读取完成");
        }

        /// <summary>
        ///设置控件对象数据
        /// </summary>
        private void SetObjectTag()
        {
            int i = 1;
            foreach (object _Control in toolStrip1.Items)
            {
                if (_Control is ToolStripButton)
                {
                    ToolStripButton _ToolStripButton = (ToolStripButton)_Control;
                    _ToolStripButton.Tag = i;
                    i++;
                }
            }
        }
    }
}
