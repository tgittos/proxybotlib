using System;
using System.Collections.Generic;
using System.Text;
using starcraftbot.proxybot;

namespace StarCraftBot_net.proxybot.Agent
{
    public class ThreadedAgent : SupportClass.ThreadClass
	{
        private IAgent agent;

        public ThreadedAgent(IAgent pAgent)
		{
            this.agent = pAgent;
		}
		override public void  Run()
		{
            agent.Start();
		}
	}
}
