using System;
using System.Collections.Generic;
using System.Collections;
using ProxyBotLib.Types;
namespace ProxyBotLib.Data
{
	/// <summary> Represents a unit in StarCraft.
	/// 
	/// TODO: use a location more accuracy than map tile.
	/// </summary>
	public class Unit
	{
		/// <summary>a unique identifier for referencing the unit </summary>
        public int ID { get; private set; }
		
		/// <summary>the player the unit belongs too </summary>
        public int PlayerID { get; private set; }
		
		/// <summary>the unit type </summary>
        public UnitType Type { get; private set; }
		
		/// <summary>x tile position </summary>
        public int X { get; private set; }
		
		/// <summary>y tile position </summary>
        public int Y { get; private set; }
		
		/// <summary>unit hit points </summary>
        public int HitPoints { get; private set; }
		
		/// <summary>unit shields </summary>
        public int Shields { get; private set; }
		
		/// <summary>unit energy </summary>
        public int Energy { get; private set; }
		
		/// <summary>an internal timer used in StarCraft </summary>
        public int OrderTimer { get; private set; }
		
		/// <summary> Order type currently being executed by the unit.</summary>
		/// <See>  the Order enum in Constants.java </See>
        public int Order { get; private set; }
		
		/// <summary>resources remaining, mineral count for patches, and gas for geysers </summary>
        public int Resources { get; private set; }
		
		/// <summary> Parses the unit data.</summary>
		//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
		public static ArrayList getUnits(String unitData,  Dictionary<int, UnitType> types )
		{
			ArrayList units = new ArrayList();
			
			String[] unitDatas = unitData.Split(':');
			bool first = true;
			
			foreach(String data in unitDatas)
			{
				if (first)
				{
					first = false;
					continue;
				}
				
				String[] attributes = data.Split(";".ToCharArray());
                Unit unit = new Unit
                {
                    ID = System.Int32.Parse(attributes[0]),
                    PlayerID = System.Int32.Parse(attributes[1]),
                    Type = types[System.Int32.Parse(attributes[2])],
                    X = System.Int32.Parse(attributes[3]),
                    Y = System.Int32.Parse(attributes[4]),
                    HitPoints = System.Int32.Parse(attributes[5]),
                    Shields = System.Int32.Parse(attributes[6]),
                    Energy = System.Int32.Parse(attributes[7]),
                    OrderTimer = System.Int32.Parse(attributes[8]),
                    Order = System.Int32.Parse(attributes[9]),
                    Resources = System.Int32.Parse(attributes[10])
                };
				
				units.Add(unit);
			}
			
			return units;
		}
		
		public virtual double distance(Unit unit)
		{
			double dx = unit.X - X;
			double dy = unit.Y - Y;
			
            //Manhattan distance
			return Math.Sqrt(dx * dx + dy * dy);
		}
		
		public override System.String ToString()
		{
			return "ID:" + ID + " player:" + PlayerID + " type:" + Type.Name + " x:" + X + " y:" + X + " hitPoints:" + HitPoints + " shields:" + Shields + " enemy:" + Energy + " orderTimer:" + OrderTimer + " order:" + Order + " resource:" + Resources;
		}
	}
}