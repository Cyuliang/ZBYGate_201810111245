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

namespace ZBYGate_201810111245.Plate
{
    public partial class PlateWindow : Form
    {
        #region//更新UI
        private delegate void  UpdatePlate(string ChIp, string ChLicesen, string ChColor, string ChTime);
        #endregion

        #region//委托
        public Action<string> PlateTransmissionAction;//发送485数据
        #endregion

        public PlateWindow()
        {
            InitializeComponent();

            #region//界面初始化
            PlateIpTextBox.Text = Properties.Settings.Default.Plate_IPAddr;
            PlatePortTextBox.Text = Properties.Settings.Default.Plate_Port.ToString();
            #endregion
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
            pictureBox2.Image = Image.FromStream(new MemoryStream(jpeg));
        }

        /// <summary>
        /// Jpeg流
        /// </summary>
        /// <param name="jpeg"></param>
        public void JpegCallBack(byte[] jpeg)
        {
            pictureBox1.Image = Image.FromStream(new MemoryStream(jpeg));
        }     
        
        /// <summary>
        /// 测试485数据
        /// </summary>
        /// <param name="mes"></param>
        public void TestSend485Action(int i)
        {
            string tmp = Properties.Settings.Default.Plate_Local_Message;
            if (DataTextBox.Text==string.Empty)
            {
                tmp = DataTextBox.Text;
            }
            PlateTransmissionAction?.Invoke(tmp);
        }
    }
}
