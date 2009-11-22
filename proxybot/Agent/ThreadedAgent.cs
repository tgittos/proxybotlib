using System;
using System.Collections.Generic;
using System.Text;
using starcraftbot.proxybot;

namespace StarCraftBot_net.proxybot.Agent
{
    public class ThreadedAgent : SupportClass.ThreadClass
	{
        private ProxyBot proxy;

        public ThreadedAgent(ProxyBot pProxy)
		{
            this.proxy = pProxy;
		}
		override public void  Run()
		{
			StarCraftAgent agent = new StarCraftAgent(proxy);
            agent.start();
		}
	}
}
