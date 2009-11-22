using System;
using System.Collections.Generic;
namespace starcraftbot.proxybot
{
	/// <summary> Represents a unit type in StarCraft.
	/// 
	/// TODO: hit points are wrong for buildings. Not really a problem since this is type data, 
	/// instances should have accurate data.
	/// 
	/// Note: Minerals and Gas and considered unit types.
	/// 
	/// See Constants.java for a listing of the upgrade types.
	/// 
	/// TODO: get the whatBuilds, requiredUnits, requireTech attributes from StarCraft.
	/// </summary>
	public class UnitType
	{
		virtual public int Id
		{
			get
			{
				return id;
			}
			
		}
		virtual public System.String Race
		{
			get
			{
				return race;
			}
			
		}
		virtual public System.String Name
		{
			get
			{
				return name;
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
		virtual public int MaxHitPoints
		{
			get
			{
				return maxHitPoints;
			}
			
		}
		virtual public int MaxShields
		{
			get
			{
				return maxShields;
			}
			
		}
		virtual public int MaxEnergy
		{
			get
			{
				return maxEnergy;
			}
			
		}
		virtual public int BuildTime
		{
			get
			{
				return buildTime;
			}
			
		}
		virtual public bool CanAttack
		{
			get
			{
				return canAttack;
			}
			
		}
		virtual public bool CanMove
		{
			get
			{
				return canMove;
			}
			
		}
		virtual public int TileWidth
		{
			get
			{
				return tileWidth;
			}
			
		}
		virtual public int TileHeight
		{
			get
			{
				return tileHeight;
			}
			
		}
		virtual public int SupplyRequired
		{
			get
			{
				return supplyRequired;
			}
			
		}
		virtual public int SupplyProvided
		{
			get
			{
				return supplyProvided;
			}
			
		}
		virtual public int SightRange
		{
			get
			{
				return sightRange;
			}
			
		}
		virtual public int GroundMinRange
		{
			get
			{
				return groundMinRange;
			}
			
		}
		virtual public int GroundMaxRange
		{
			get
			{
				return groundMaxRange;
			}
			
		}
		virtual public int GroundDamage
		{
			get
			{
				return groundDamage;
			}
			
		}
		virtual public int AirRange
		{
			get
			{
				return airRange;
			}
			
		}
		virtual public int AirDamage
		{
			get
			{
				return airDamage;
			}
			
		}
		virtual public bool Building
		{
			get
			{
				return building;
			}
			
		}
		virtual public bool Flyer
		{
			get
			{
				return flyer;
			}
			
		}
		virtual public bool SpellCaster
		{
			get
			{
				return spellCaster;
			}
			
		}
		virtual public bool Worker
		{
			get
			{
				return worker;
			}
			
		}
		virtual public int WhatBuilds
		{
			get
			{
				return whatBuilds;
			}
			
		}
		virtual public bool Center
		{
			get
			{
				return id == Constants.Terran_Command_Center || id == Constants.Protoss_Nexus || id == Constants.Zerg_Hatchery;
			}
			
		}
		
		/// <summary>unit type identifier </summary>
		private int id;
		
		/// <summary>the race of the unit type </summary>
		private System.String race;
		
		/// <summary>the name of the type </summary>
		private System.String name;
		
		/// <summary>mineral cost to produce </summary>
		private int mineralsCost;
		
		/// <summary>gas cost to produce </summary>
		private int gasCost;
		
		/// <summary>max hit points </summary>
		private int maxHitPoints;
		
		/// <summary>max shields </summary>
		private int maxShields;
		
		/// <summary>max energy, this should not be static </summary>
		private int maxEnergy;
		
		/// <summary>time to produce the unit, specified in game frames </summary>
		private int buildTime;
		
		/// <summary>does the type have an attack </summary>
		private bool canAttack;
		
		/// <summary>is the type mobile </summary>
		private bool canMove;
		
		/// <summary>width of the type in map tiles, note that units use a different size system than buildings </summary>
		private int tileWidth;
		
		/// <summary>height of the type in map tiles, note that units use a different size system than buildings </summary>
		private int tileHeight;
		
		/// <summary>supply require to produce, double the value you'd expect </summary>
		private int supplyRequired;
		
		/// <summary>supply provided by the type, double the value you'd expect </summary>
		private int supplyProvided;
		
		/// <summary>vision range of the type </summary>
		private int sightRange;
		
		/// <summary>minimum ground attack range, i think this pertains only to sieged up tanks </summary>
		private int groundMinRange;
		
		/// <summary>maximum ground attack range </summary>
		private int groundMaxRange;
		
		/// <summary>base damage dealt to ground units </summary>
		private int groundDamage;
		
		/// <summary>maximum air attack range </summary>
		private int airRange;
		
		/// <summary>base damage dealt to air units </summary>
		private int airDamage;
		
		/// <summary>is this type a building </summary>
		private bool building;
		
		/// <summary>is this a flying type </summary>
		private bool flyer;
		
		/// <summary>is this type a spell caster </summary>
		private bool spellCaster;
		
		/// <summary>is this type a worker unit </summary>
		private bool worker;
		
		/// <summary>id of the unit type that produces this </summary>
		private int whatBuilds;
		
		/// <summary> Parses the unit types.</summary>
		//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
        public static Dictionary<int, UnitType> getUnitTypes(System.String unitTypeData)
		{
			//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
            Dictionary<int, UnitType> types = new Dictionary<int, UnitType>();
			
			System.String[] unitTypes = unitTypeData.Split(":".ToCharArray());
			bool first = true;
			
			foreach(String unitType in unitTypes)
			{
				if (first)
				{
					first = false;
					continue;
				}

                System.String[] attributes = unitType.Split(";".ToCharArray());
				UnitType type = new UnitType();
				
				type.id = System.Int32.Parse(attributes[0]);
				type.race = attributes[1];
				type.name = attributes[2];
				type.mineralsCost = System.Int32.Parse(attributes[3]);
				type.gasCost = System.Int32.Parse(attributes[4]);
				type.maxHitPoints = System.Int32.Parse(attributes[5]);
				type.maxShields = System.Int32.Parse(attributes[6]);
				type.maxEnergy = System.Int32.Parse(attributes[7]);
				type.buildTime = System.Int32.Parse(attributes[8]);
				type.canAttack = System.Int32.Parse(attributes[9]) == 1;
				type.canMove = System.Int32.Parse(attributes[10]) == 1;
				type.tileWidth = System.Int32.Parse(attributes[11]);
				type.tileHeight = System.Int32.Parse(attributes[12]);
				type.supplyRequired = System.Int32.Parse(attributes[13]);
				type.supplyProvided = System.Int32.Parse(attributes[14]);
				type.sightRange = System.Int32.Parse(attributes[15]);
				type.groundMaxRange = System.Int32.Parse(attributes[16]);
				type.groundMinRange = System.Int32.Parse(attributes[17]);
				type.groundDamage = System.Int32.Parse(attributes[18]);
				type.airRange = System.Int32.Parse(attributes[19]);
				type.airDamage = System.Int32.Parse(attributes[20]);
				type.building = System.Int32.Parse(attributes[21]) == 1;
				type.flyer = System.Int32.Parse(attributes[22]) == 1;
				type.spellCaster = System.Int32.Parse(attributes[23]) == 1;
				type.worker = System.Int32.Parse(attributes[24]) == 1;
				type.whatBuilds = System.Int32.Parse(attributes[25]);
				
				System.Console.Out.WriteLine(type.Id + " " + type.whatBuilds);
				
				types.Add(type.Id, type);
			}
			
			return types;
		}
		
		public override System.String ToString()
		{
			return "id:" + id + " race:" + race + " name:" + name + " minCost:" + mineralsCost + " gasCost:" + gasCost + " hitPoints:" + maxHitPoints + " shields:" + maxShields + " energy:" + maxEnergy + " buildTime:" + buildTime + " canMove:" + canMove + " canAttack:" + canAttack + " width:" + tileWidth + " height:" + tileHeight + " supplyRequired:" + supplyRequired + " supplyProvided:" + supplyProvided + " sight:" + sightRange + " groundMaxRange:" + groundMaxRange + " groundMinRange:" + groundMinRange + " groundDamage:" + groundDamage + " airRange:" + airRange + " airDamage:" + airDamage + " building:" + building + " flyer:" + flyer + " spellcaster:" + spellCaster + " worker:" + worker + " whatBuilds:" + whatBuilds;
		}
	}
}