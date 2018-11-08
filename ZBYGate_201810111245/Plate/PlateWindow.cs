using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZBYGate_Data_Collection.Plate
{
    public partial class PlateWindow : Form
    {
        public Action<int> PlateLinkAction;//链接车牌
        public Action<int> PlateAbortAction;//断开链接
        public Action<int> PlateTiggerAction;//手动触发
        public Action<int> PlateLiftingAction;//抬杆
        public Action<string> PlateTransmissionAction;//485透明传输
        public Action<int> PlateSearchAction;//搜索设备
        public Action<int> PlateSetIpAction;//设定IP
        public Action<int> PlateSetPathAction;//设定IP
        public Action<bool> PlatePlayAction;//打开视频
        public Action<bool> PlateCloseAction;//关闭视频

        private delegate void UpdateUiInvok(string Message);//跨线程更新UI
        private delegate void UpdateImageInvok(byte[] Jpeg);
        private UpdateImageInvok ImageInvok;
        private UpdateImageInvok DataImInvok;
        private System.Threading.Timer _timer = null;//定时恢复状态

        private MemoryStream Mjpeg = null;

        #region//更新UI
        private delegate void  UpdatePlate(string ChIp, string ChLicesen, string ChColor, string ChTime);
        #endregion

        public PlateWindow()
        {
            InitializeComponent();

            SetObjectTag();

            #region//界面初始化
            PlateIpTextBox.Text = Properties.Settings.Default.Plate_IPAddr;
            PlatePortTextBox.Text = Properties.Settings.Default.Plate_Port.ToString();
            #endregion

            DataImInvok += DataJpeg;
            ImageInvok += JpegCallBack;
            
            _timer = new System.Threading.Timer(ClearText, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(0));
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
            _timer.Change(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(0));
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

        /// <summary>
        /// 车牌识别结果
        /// </summary>
        /// <param name="ChIp"></param>
        /// <param name="ChLicesen"></param>
        /// <param name="ChColor"></param>
        /// <param name="ChTime"></param>
        public void PlateResult(string ChIp, string ChLicesen, string ChColor, string ChTime)
        {
            if(TimeTextBox.InvokeRequired)
            {
                TimeTextBox.Invoke(new UpdatePlate(PlateResult), new object[] { ChIp, ChLicesen, ChColor, ChTime });
            }
            else
            {
                TimeTextBox.Text = ChTime;
                IpTextBox.Text = ChIp;
                PlateTextBox.Text = ChLicesen;
                ColorTextBox.Text = ChColor;
            }
        }

        /// <summary>
        /// 识别结果图片
        /// </summary>
        /// <param name="jpeg"></param>
        public void DataJpeg(byte[] jpeg)
        {
            if (pictureBox2.InvokeRequired)
            {
                pictureBox2.Invoke(DataImInvok, new object[] { jpeg });
            }
            else
            {
                Mjpeg = new MemoryStream(jpeg, 0, jpeg.Length);
                pictureBox2.Image = Image.FromStream(Mjpeg);
            }
            Mjpeg.Seek(0, SeekOrigin.Begin);
            Mjpeg.SetLength(0);          
        }

        /// <summary>
        /// Jpeg流
        /// </summary>
        /// <param name="jpeg"></param>
        public void JpegCallBack(byte[] jpeg)
        {
            if (pictureBox1.InvokeRequired)
            {
                pictureBox1.Invoke(ImageInvok, new object[] { jpeg });
            }
            else
            {
                Mjpeg = new MemoryStream(jpeg, 0, jpeg.Length);
                pictureBox1.Image = Image.FromStream(Mjpeg);
            }
            Mjpeg.Seek(0, SeekOrigin.Begin);
            Mjpeg.SetLength(0);
        }     
        
        /// <summary>
        /// 测试485数据
        /// </summary>
        /// <param name="mes"></param>
        private void TestSend485()
        {
            string tmp = Properties.Settings.Default.Plate_Local_Message;
            if (DataTextBox.Text!=string.Empty)
            {
                tmp = DataTextBox.Text;
            }
            PlateTransmissionAction?.Invoke(tmp);
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
                    PlateLinkAction?.Invoke(0);
                    break;
                case 2:
                    PlateAbortAction?.Invoke(0);
                    break;
                case 3:
                    PlateTiggerAction?.Invoke(0);
                    break;
                case 4:
                    PlateLiftingAction?.Invoke(0);
                    break;
                case 5:
                    TestSend485();
                    break;
                case 6:
                    PlateSearchAction?.BeginInvoke(0, new AsyncCallback(SearchCallBack), null);
                    break;
                case 7:
                    PlateSetPathAction?.Invoke(0);
                    break;
                case 8:
                    PlateSetIpAction?.Invoke(0);
                    break;
                case 9:
                    PlatePlayAction?.Invoke(true);
                    break;
                case 10:
                    PlatePlayAction?.Invoke(false);
                    new System.Threading.Timer(ClearPicCallBack,null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(0));
                    break;
            }
        }

        /// <summary>
        /// 关闭视频清除PICtuxe
        /// </summary>
        /// <param name="state"></param>
        private void ClearPicCallBack(object state)
        {
            try
            {
                Mjpeg.Seek(0, SeekOrigin.Begin);
                Mjpeg.SetLength(0);
                Mjpeg = null;
            }
            catch (Exception)
            {
                SetStatusText("重复关闭视频");
            }
            pictureBox1.Image = null;
            SetStatusText("关闭视频回调完成");
        }

        /// <summary>
        /// 搜索设备回调
        /// </summary>
        /// <param name="ar"></param>
        private void SearchCallBack(IAsyncResult ar)
        {
            SetStatusText("搜索设备完成");
        }
    }
}
