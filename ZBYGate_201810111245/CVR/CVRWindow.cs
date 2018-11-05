using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ZBYGate_201810111245.CVR
{
    public partial class CVRWindow : Form
    {
        #region//更新UI
        private delegate void UpdateCVR(byte[] name, byte[] sex, byte[] peopleNation, byte[] birthday, byte[] number, byte[] address, byte[] signdate, byte[] validtermOfStart, byte[] validtermOfEnd);
        private delegate void UpdateCVRImage(byte[] imgData, int length);
        #endregion

        public CVRWindow()
        {
            InitializeComponent();
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
                textBox1.Invoke(new UpdateCVR(FillData), new object[] { name, sex,peopleNation,birthday,number,address,signdate,validtermOfStart,validtermOfEnd });
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
                pictureBoxPhoto.Invoke(new UpdateCVRImage(FillDataBmp), new object[] { imgData, length });
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
            }
        }
    }
}
