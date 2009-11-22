using System;
//UPGRADE_TODO: The type 'starcraftbot.proxybot.Constants.Order' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using Order = starcraftbot.proxybot.Constants.Order;
namespace starcraftbot.proxybot
{
	/// <summary> Throw in your bot code here.</summary>
	public class StarCraftAgent
	{
		
		public virtual void  start()
		{
			
			ProxyBot proxyBot = ProxyBot.Proxy;
			int playerID = proxyBot.PlayerID;
			
			while (true)
			{
				try
				{
					//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
					System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * 250));
				}
				catch (System.Exception e)
				{
				}
				
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				foreach(Unit unit in proxyBot.Units)
				{
					
					// make idle works mine				
					if (unit.getPlayerID() == playerID && unit.getType().isWorker())
					{
						
						if (unit.getOrder() == Order.PlayerGuard.ordinal())
						{
							int closestID = - 1;
							//UPGRADE_TODO: The equivalent in .NET for field 'java.lang.Double.MAX_VALUE' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
							double closest = System.Double.MaxValue;
							
							//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
							foreach(Unit patch in proxyBot.getUnits())
							{
								
								if (patch.getType().getId() == Constants.Resource_Mineral_Field)
								{
									
									double distance = (0.5 + SupportClass.Random.NextDouble()) * unit.distance(patch);
									if (distance < closest)
									{
										closest = distance;
										closestID = patch.getID();
									}
								}
							}
							
							if (closestID != - 1)
							{
								System.Console.Out.WriteLine("Right on patch: " + unit.getID() + " " + closestID);
								proxyBot.rightClick(unit.getID(), closestID);
							}
						}
					}
				}
			}
		}
	}
}