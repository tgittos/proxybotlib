using System;
using System.Collections;
namespace ProxyBotLib.Types
{
	/// <summary> Reference data about the upgrades in StarCraft.
	/// 
	/// See Constants.java for a listing of the upgrade types.
	/// </summary>
	public class UpgradeType
	{		
		/// <summary>upgrade type identifier </summary>
        public int ID { get; private set; }
		
		/// <summary>name of the upgrade </summary>
        public String Name { get; private set; }
		
		/// <summary>id of the unit type that researches the upgrade </summary>
        public int WhatResearchesID { get; private set; }
		
		/// <summary>number of times the upgrade can be researched. usually 1, 3 for weapon upgrades </summary>
        public int Repeats { get; private set; }
		
		/// <summary>mineral cost of the upgrade </summary>
        public int MineralsBase { get; private set; }
		
		/// <summary>increase in mineral cost per upgrade level </summary>
        public int MineralsFactor { get; private set; }
		
		/// <summary>gas cost of the upgrade </summary>
        public int GasBase { get; private set; }
		
		/// <summary>increase in gas cost per upgrade level </summary>
        public int GasFactor { get; private set; }
		
		/// <summary> Parses the upgrade types.</summary>
		public static ArrayList getUpgradeTypes(System.String techData)
		{
			ArrayList types = new ArrayList();
			
			String[] typs = techData.Split(':');
			bool first = true;
			
			foreach(String typ in typs)
			{
				if (first)
				{
					first = false;
					continue;
				}

                System.String[] attribtes = typ.Split(";".ToCharArray());

                UpgradeType type = new UpgradeType
                {
                    ID = System.Int32.Parse(attribtes[0]),
                    Name = attribtes[1],
                    WhatResearchesID = System.Int32.Parse(attribtes[2]),
                    Repeats = System.Int32.Parse(attribtes[3]),
                    MineralsBase = System.Int32.Parse(attribtes[4]),
                    MineralsFactor = System.Int32.Parse(attribtes[5]),
                    GasBase = System.Int32.Parse(attribtes[6]),
                    GasFactor = System.Int32.Parse(attribtes[7])
                };

				types.Add(type);
			}
			
			return types;
		}
		
		public override System.String ToString()
		{
			return "id:" + ID + " name:" + Name + " whatResearches:" + WhatResearchesID + " repeats:" + Repeats + " minsBase:" + MineralsBase + " minsFactor:" + MineralsFactor + " gasBase:" + GasBase + " gasFactor:" + GasFactor;
		}
	}
}