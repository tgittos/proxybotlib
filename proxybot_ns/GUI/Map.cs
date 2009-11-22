using System;
using System.Collections.Generic;
using System.Text;
using starcraftbot.proxybot;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;

namespace StarCraftBot_net.proxybot.GUI
{
    class Map : SupportClass.ThreadClass
	{
        private Thread formThread;
        private ProxyBot proxyBot;
        private StarCraftFrame Frame;

        public delegate void RefreshDelegate();

        public Map(ProxyBot pProxy)
		{
            proxyBot = pProxy;
		}
		override public void Run()
		{
            Frame = new StarCraftFrame(proxyBot);
            formThread = new Thread(delegate()
            {
                Application.Run(Frame);
            });
            //Terminate with application
            formThread.IsBackground = true;
            formThread.Start();
		}
        private void refreshMap()
        {
            Frame.Refresh();
        }
        public void Refresh()
        {
            RefreshDelegate d = new RefreshDelegate(refreshMap);
            if (Frame != null && Frame.Created)
            {
                Frame.Invoke(d);
            }
        }
	}
}
