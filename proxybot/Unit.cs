using System;
using System.Collections.Generic;
namespace starcraftbot.proxybot
{
	/// <summary> Represents a unit in StarCraft.
	/// 
	/// TODO: use a location more accuracy than map tile.
	/// </summary>
	public class Unit
	{
		virtual public int PlayerID
		{
			get
			{
				return playerID;
			}
			
			set
			{
				this.playerID = value;
			}
			
		}
		virtual public UnitType Type
		{
			get
			{
				return type;
			}
			
			set
			{
				this.type = value;
			}
			
		}
		virtual public int X
		{
			get
			{
				return x;
			}
			
			set
			{
				this.x = value;
			}
			
		}
		virtual public int Y
		{
			get
			{
				return y;
			}
			
			set
			{
				this.y = value;
			}
			
		}
		virtual public int HitPoints
		{
			get
			{
				return hitPoints;
			}
			
			set
			{
				this.hitPoints = value;
			}
			
		}
		virtual public int Shields
		{
			get
			{
				return shields;
			}
			
			set
			{
				this.shields = value;
			}
			
		}
		virtual public int Energy
		{
			get
			{
				return energy;
			}
			
			set
			{
				this.energy = value;
			}
			
		}
		virtual public int OrderTimer
		{
			get
			{
				return orderTimer;
			}
			
			set
			{
				this.orderTimer = value;
			}
			
		}
		virtual public int Order
		{
			get
			{
				return order;
			}
			
			set
			{
				this.order = value;
			}
			
		}
		virtual public int Resources
		{
			get
			{
				return resources;
			}
			
		}
		
		/// <summary>a unique identifier for referencing the unit </summary>
        private int id;

        public int ID
        {
            get { return id; }
        }
		
		/// <summary>the player the unit belongs too </summary>
		private int playerID;
		
		/// <summary>the unit type </summary>
		private UnitType type;
		
		/// <summary>x tile position </summary>
		private int x;
		
		/// <summary>y tile position </summary>
		private int y;
		
		/// <summary>unit hit points </summary>
		private int hitPoints;
		
		/// <summary>unit shields </summary>
		private int shields;
		
		/// <summary>unit energy </summary>
		private int energy;
		
		/// <summary>an internal timer used in StarCraft </summary>
		private int orderTimer;
		
		/// <summary> Order type currently being executed by the unit.</summary>
		/// <See>  the Order enum in Constants.java </See>
		private int order;
		
		/// <summary>resources remaining, mineral count for patches, and gas for geysers </summary>
		private int resources;
		
		/// <summary> Parses the unit data.</summary>
		//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
		public static System.Collections.ArrayList getUnits(System.String unitData,  Dictionary<int, UnitType> types )
		{
			System.Collections.ArrayList units = new System.Collections.ArrayList();
			
			System.String[] unitDatas = unitData.Split(":".ToCharArray());
			bool first = true;
			
			foreach(String data in unitDatas)
			{
				if (first)
				{
					first = false;
					continue;
				}
				
				System.String[] attributes = data.Split(";".ToCharArray());
				Unit unit = new Unit();
				unit.id = System.Int32.Parse(attributes[0]);
				unit.playerID = System.Int32.Parse(attributes[1]);
                //TODO:Check
				unit.type = types[System.Int32.Parse(attributes[2])];
				unit.x = System.Int32.Parse(attributes[3]);
				unit.y = System.Int32.Parse(attributes[4]);
				unit.hitPoints = System.Int32.Parse(attributes[5]);
				unit.shields = System.Int32.Parse(attributes[6]);
				unit.energy = System.Int32.Parse(attributes[7]);
				unit.orderTimer = System.Int32.Parse(attributes[8]);
				unit.order = System.Int32.Parse(attributes[9]);
				unit.resources = System.Int32.Parse(attributes[10]);
				
				units.Add(unit);
			}
			
			return units;
		}
		
		public virtual double distance(Unit unit)
		{
			double dx = unit.x - x;
			double dy = unit.y - y;
			
			return System.Math.Sqrt(dx * dx + dy * dy);
		}
		
		public virtual int getID()
		{
			return id;
		}
		
		public virtual void  setID(int ID)
		{
			this.id = ID;
		}
		
		public override System.String ToString()
		{
			return "ID:" + id + " player:" + playerID + " type:" + type.Name + " x:" + x + " y:" + y + " hitPoints:" + hitPoints + " shields:" + shields + " enemy:" + energy + " orderTimer:" + orderTimer + " order:" + order + " resource:" + resources;
		}
	}
}