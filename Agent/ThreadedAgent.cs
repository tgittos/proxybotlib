using System;
using System.Collections.Generic;
using System.Text;
using ProxyBotLib.Threading;

namespace ProxyBotLib.Agent
{
    public class ThreadedAgent : ThreadClass
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
