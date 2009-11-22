using System;
using System.Collections.Generic;
using System.Text;
using starcraftbot.proxybot;

namespace StarCraftBot_net.proxybot.Agent
{
    public class ThreadedAgent : SupportClass.ThreadClass
	{
        private IAgent agent;
        private ProxyBot proxy;

        public ThreadedAgent(IAgent pAgent, ProxyBot pProxy)
		{
            this.agent = pAgent;
            this.proxy = pProxy;
		}
		override public void  Run()
		{
            agent.Start(proxy);
		}
	}
}
