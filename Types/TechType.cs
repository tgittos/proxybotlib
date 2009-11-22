using System;
using System.Collections;
namespace ProxyBotLib.Types
{
	/// <summary> Reference data about the tech types in StarCraft.
	/// 
	/// See Constants.java for a listing of the tech types.
	/// </summary>
	public class TechType
	{
		/// <summary>the tech unique ID </summary>
        public int ID { get; private set; }
		
		/// <summary>the tech type name </summary>
        public String Name { get; private set; }
		
		/// <summary>the unit type that research the tech </summary>
        public int WhatResearchesID { get; private set; }
		
		/// <summary>mineral cost of the tech </summary>
        public int MineralsCost { get; private set; }
		
		/// <summary>gas cost of the tech </summary>
        public int GasCost { get; private set; }
		
		/// <summary> Parses the tech types.</summary>
		public static ArrayList getTechTypes(String techData)
		{
			ArrayList types = new ArrayList();
			
			System.String[] typs = techData.Split(':');
			bool first = true;
			
			foreach(String typ in typs)
			{
				if (first)
				{
					first = false;
					continue;
				}
				
				System.String[] attribtes = typ.Split(';');

                TechType type = new TechType
                {
                    ID = System.Int32.Parse(attribtes[0]),
                    Name = attribtes[1],
                    WhatResearchesID = System.Int32.Parse(attribtes[2]),
                    MineralsCost = System.Int32.Parse(attribtes[3]),
                    GasCost = System.Int32.Parse(attribtes[4])
                };
				
				types.Add(type);
			}
			
			return types;
		}
		
		public override System.String ToString()
		{
			return "id:" + ID + " name:" + Name + " whatResearches:" + WhatResearchesID + " mins:" + MineralsCost + " gas:" + GasCost;
		}
	}
}