using System;
using Order = ProxyBotLib.Types.Constants.Order;
using ProxyBotLib.Agent;
using ProxyBotLib;
using ProxyBotLib.Types;
using ProxyBotLib.Data;
namespace ProxyBotLib.Test
{
	/// <summary> Throw in your bot code here.</summary>
	internal class StarCraftAgent : IAgent
	{
        private ProxyBot proxyBot;

        public StarCraftAgent()
        {
        }

        public virtual void Start(ProxyBot pProxy)
		{
            this.proxyBot = pProxy;

			int playerID = proxyBot.PlayerID;
			
			while (true)
			{
				try
				{
					//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
					System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * 250));
				}
				catch (System.Exception)
				{
				}
				
				foreach(Unit unit in proxyBot.Units)
				{
					
					// make idle works mine				
					if (unit.PlayerID == playerID && unit.Type.Worker)
					{
						
						if (unit.Order == Convert.ToInt32(Order.PlayerGuard))
						{
							int closestID = - 1;
							//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MAX_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
							double closest = System.Double.MaxValue;
							
							foreach(Unit patch in proxyBot.Units)
							{
								
								if (patch.Type.ID == Constants.Resource_Mineral_Field)
								{
									
									double distance = (0.5 + new Random().NextDouble()) * unit.distance(patch);
									if (distance < closest)
									{
										closest = distance;
										closestID = patch.ID;
									}
								}
							}
							
							if (closestID != - 1)
							{
								//System.Console.Out.WriteLine("Right on patch: " + unit.ID + " " + closestID);
								proxyBot.rightClick(unit.ID, closestID);
							}
						}
					}
				}
			}
		}
	}
}