using System;
using System.Collections.Generic;
using System.Text;
using starcraftbot.proxybot;
using System.Threading;
using System.Windows.Forms;

namespace StarCraftBot_net.proxybot.GUI
{
    class Map : SupportClass.ThreadClass
	{
        ProxyBot proxyBot;

        public Map(ProxyBot pProxy)
		{
            proxyBot = pProxy;
		}
		override public void Run()
		{
            StarCraftFrame frame;
            Thread formThread = new Thread(delegate()
            {
                frame = new StarCraftFrame(proxyBot);
                Application.Run(frame);
            });
            //Terminate with application
            formThread.IsBackground = true;
            formThread.Start();
		}
	}
}
