using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZBYGate_201810111245.Gate
{
    public partial class GateWindow : Form
    {
        public GateWindow()
        {
            InitializeComponent();

            textBox1.Text = Properties.Settings.Default.Gate_InDoorIp;
            textBox2.Text = Properties.Settings.Default.Gate_Port.ToString();
            textBox3.Text = Properties.Settings.Default.Gate_InDoorSN;
            textBox4.Text = Properties.Settings.Default.Gate_OutDoorIp;
            textBox5.Text = Properties.Settings.Default.Gate_Port.ToString();
            textBox6.Text = Properties.Settings.Default.Gate_OutDoorSN;
        }
    }
}
