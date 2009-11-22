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
        public Map()
		{
		}
		override public void Run()
		{
            StarCraftFrame frame;
            Thread formThread = new Thread(delegate()
            {
                frame = new StarCraftFrame();
                Application.Run(frame);
            });
            //Terminate with application
            formThread.IsBackground = true;
            formThread.Start();
		}
	}
}
