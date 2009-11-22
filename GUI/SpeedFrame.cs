using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ProxyBotLib;

namespace ProxyBotLib.GUI
{
    public partial class SpeedFrame : Form
    {
        private ProxyBot proxyBot;
        private int initialSpeed = 20;
        private int slowest = 0;
        private int fastest = 100;

        public SpeedFrame(ProxyBot pProxy)
        {
            InitializeComponent();

            proxyBot = pProxy;

            //Init the track bar
            tbSpeed.SetRange(slowest, fastest);
            tbSpeed.Value = initialSpeed;

            tbSpeed.ValueChanged += new EventHandler(tbSpeed_ValueChanged);
        }

        void tbSpeed_ValueChanged(object sender, EventArgs e)
        {
            int newSpeed = 100 - tbSpeed.Value;
            proxyBot.setGameSpeed(newSpeed);
        }
    }
}
