using System;
namespace starcraftbot.proxybot
{
	/// <summary> Reference data about the upgrades in StarCraft.
	/// 
	/// See Constants.java for a listing of the upgrade types.
	/// </summary>
	public class UpgradeType
	{
		virtual public int Id
		{
			get
			{
				return id;
			}
			
		}
		virtual public System.String Name
		{
			get
			{
				return name;
			}
			
		}
		virtual public int WhatResearchesID
		{
			get
			{
				return whatResearchesID;
			}
			
		}
		virtual public int Repeats
		{
			get
			{
				return repeats;
			}
			
		}
		virtual public int MineralsBase
		{
			get
			{
				return mineralsBase;
			}
			
		}
		virtual public int MineralsFactor
		{
			get
			{
				return mineralsFactor;
			}
			
		}
		virtual public int GasBase
		{
			get
			{
				return gasBase;
			}
			
		}
		virtual public int GasFactor
		{
			get
			{
				return gasFactor;
			}
			
		}
		
		/// <summary>upgrade type identifier </summary>
		private int id;
		
		/// <summary>name of the upgrade </summary>
		private System.String name;
		
		/// <summary>id of the unit type that researches the upgrade </summary>
		private int whatResearchesID;
		
		/// <summary>number of times the upgrade can be researched. usually 1, 3 for weapon upgrades </summary>
		private int repeats;
		
		/// <summary>mineral cost of the upgrade </summary>
		private int mineralsBase;
		
		/// <summary>increase in mineral cost per upgrade level </summary>
		private int mineralsFactor;
		
		/// <summary>gas cost of the upgrade </summary>
		private int gasBase;
		
		/// <summary>increase in gas cost per upgrade level </summary>
		private int gasFactor;
		
		/// <summary> Parses the upgrade types.</summary>
		public static System.Collections.ArrayList getUpgradeTypes(System.String techData)
		{
			System.Collections.ArrayList types = new System.Collections.ArrayList();
			
			System.String[] typs = techData.Split(":".ToCharArray());
			bool first = true;
			
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			foreach(String typ in typs)
			{
				if (first)
				{
					first = false;
					continue;
				}

                System.String[] attribtes = typ.Split(";".ToCharArray());
				
				UpgradeType type = new UpgradeType();
				type.id = System.Int32.Parse(attribtes[0]);
				type.name = attribtes[1];
				type.whatResearchesID = System.Int32.Parse(attribtes[2]);
				type.repeats = System.Int32.Parse(attribtes[3]);
				type.mineralsBase = System.Int32.Parse(attribtes[4]);
				type.mineralsFactor = System.Int32.Parse(attribtes[5]);
				type.gasBase = System.Int32.Parse(attribtes[6]);
				type.gasFactor = System.Int32.Parse(attribtes[7]);
				
				types.Add(type);
			}
			
			return types;
		}
		
		public override System.String ToString()
		{
			return "id:" + id + " name:" + name + " whatResearches:" + whatResearchesID + " repeats:" + repeats + " minsBase:" + mineralsBase + " minsFactor:" + mineralsFactor + " gasBase:" + gasBase + " gasFactor:" + gasFactor;
		}
	}
}