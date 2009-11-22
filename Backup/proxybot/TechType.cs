using System;
namespace starcraftbot.proxybot
{
	/// <summary> Reference data about the tech types in StarCraft.
	/// 
	/// See Constants.java for a listing of the tech types.
	/// </summary>
	public class TechType
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
		virtual public int MineralsCost
		{
			get
			{
				return mineralsCost;
			}
			
		}
		virtual public int GasCost
		{
			get
			{
				return gasCost;
			}
			
		}
		
		/// <summary>the tech unique ID </summary>
		private int id;
		
		/// <summary>the tech type name </summary>
		private System.String name;
		
		/// <summary>the unit type that research the tech </summary>
		private int whatResearchesID;
		
		/// <summary>mineral cost of the tech </summary>
		private int mineralsCost;
		
		/// <summary>gas cost of the tech </summary>
		private int gasCost;
		
		/// <summary> Parses the tech types.</summary>
		public static System.Collections.ArrayList getTechTypes(System.String techData)
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
				
				TechType type = new TechType();
				type.id = System.Int32.Parse(attribtes[0]);
				type.name = attribtes[1];
				type.whatResearchesID = System.Int32.Parse(attribtes[2]);
				type.mineralsCost = System.Int32.Parse(attribtes[3]);
				type.gasCost = System.Int32.Parse(attribtes[4]);
				
				types.Add(type);
			}
			
			return types;
		}
		
		public override System.String ToString()
		{
			return "id:" + id + " name:" + name + " whatResearches:" + whatResearchesID + " mins:" + mineralsCost + " gas:" + gasCost;
		}
	}
}