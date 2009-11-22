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
        /// <summary>
        /// is the unit a base
        /// </summary>
		public bool Center
		{
			get
			{
				return ID == Constants.Terran_Command_Center || ID == Constants.Protoss_Nexus || ID == Constants.Zerg_Hatchery;
			}
			
		}
		
		/// <summary>unit type identifier </summary>
        public int ID { get; private set; }
		
		/// <summary>the race of the unit type </summary>
        public String Race { get; private set; }
		
		/// <summary>the name of the type </summary>
        public String Name { get; private set; }
		
		/// <summary>mineral cost to produce </summary>
        public int MineralsCost { get; private set; }
		
		/// <summary>gas cost to produce </summary>
        public int GasCost { get; private set; }
		
		/// <summary>max hit points </summary>
        public int MaxHitPoints { get; private set; }
		
		/// <summary>max shields </summary>
        public int MaxShields { get; private set; }
		
		/// <summary>max energy, this should not be static </summary>
        public int MaxEnergy { get; private set; }
		
		/// <summary>time to produce the unit, specified in game frames </summary>
        public int BuildTime { get; private set; }
		
		/// <summary>does the type have an attack </summary>
        public bool CanAttack { get; private set; }
		
		/// <summary>is the type mobile </summary>
        public bool CanMove { get; private set; }
		
		/// <summary>width of the type in map tiles, note that units use a different size system than buildings </summary>
        public int TileWidth { get; private set; }
		
		/// <summary>height of the type in map tiles, note that units use a different size system than buildings </summary>
        public int TileHeight { get; private set; }
		
		/// <summary>supply require to produce, double the value you'd expect </summary>
        public int SupplyRequired { get; private set; }
		
		/// <summary>supply provided by the type, double the value you'd expect </summary>
        public int SupplyProvided { get; private set; }
		
		/// <summary>vision range of the type </summary>
        public int SightRange { get; private set; }
		
		/// <summary>minimum ground attack range, i think this pertains only to sieged up tanks </summary>
        public int GroundMinRange { get; private set; }
		
		/// <summary>maximum ground attack range </summary>
        public int GroundMaxRange { get; private set; }
		
		/// <summary>base damage dealt to ground units </summary>
        public int GroundDamage { get; private set; }
		
		/// <summary>maximum air attack range </summary>
        public int AirRange { get; private set; }
		
		/// <summary>base damage dealt to air units </summary>
        public int AirDamage { get; private set; }
		
		/// <summary>is this type a building </summary>
        public bool Building { get; private set; }
		
		/// <summary>is this a flying type </summary>
        public bool Flyer { get; private set; }
		
		/// <summary>is this type a spell caster </summary>
        public bool SpellCaster { get; private set; }
		
		/// <summary>is this type a worker unit </summary>
        public bool Worker { get; private set; }
		
		/// <summary>id of the unit type that produces this </summary>
        public int WhatBuilds { get; private set; }
		
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
                UnitType type = new UnitType
                {
                    ID = System.Int32.Parse(attributes[0]),
                    Race = attributes[1],
                    Name = attributes[2],
                    MineralsCost = System.Int32.Parse(attributes[3]),
                    GasCost = System.Int32.Parse(attributes[4]),
                    MaxHitPoints = System.Int32.Parse(attributes[5]),
                    MaxShields = System.Int32.Parse(attributes[6]),
                    MaxEnergy = System.Int32.Parse(attributes[7]),
                    BuildTime = System.Int32.Parse(attributes[8]),
                    CanAttack = System.Int32.Parse(attributes[9]) == 1,
                    CanMove = System.Int32.Parse(attributes[10]) == 1,
                    TileWidth = System.Int32.Parse(attributes[11]),
                    TileHeight = System.Int32.Parse(attributes[12]),
                    SupplyRequired = System.Int32.Parse(attributes[13]),
                    SupplyProvided = System.Int32.Parse(attributes[14]),
                    SightRange = System.Int32.Parse(attributes[15]),
                    GroundMaxRange = System.Int32.Parse(attributes[16]),
                    GroundMinRange = System.Int32.Parse(attributes[17]),
                    GroundDamage = System.Int32.Parse(attributes[18]),
                    AirRange = System.Int32.Parse(attributes[19]),
                    AirDamage = System.Int32.Parse(attributes[20]),
                    Building = System.Int32.Parse(attributes[21]) == 1,
                    Flyer = System.Int32.Parse(attributes[22]) == 1,
                    SpellCaster = System.Int32.Parse(attributes[23]) == 1,
                    Worker = System.Int32.Parse(attributes[24]) == 1,
                    WhatBuilds = System.Int32.Parse(attributes[25]),
                };
				System.Console.Out.WriteLine(type.ID + " " + type.WhatBuilds);
				
				types.Add(type.ID, type);
			}
			
			return types;
		}
		
		public override System.String ToString()
		{
			return "id:" + ID + " race:" + Race + " name:" + Name + " minCost:" + MineralsCost + " gasCost:" + GasCost + " hitPoints:" + MaxHitPoints + " shields:" + MaxShields + " energy:" + MaxEnergy + " buildTime:" + BuildTime + " canMove:" + CanMove + " canAttack:" + CanAttack + " width:" + TileWidth + " height:" + TileHeight + " supplyRequired:" + SupplyRequired + " supplyProvided:" + SupplyProvided + " sight:" + SightRange + " groundMaxRange:" + GroundMaxRange + " groundMinRange:" + GroundMinRange + " groundDamage:" + GroundDamage + " airRange:" + AirRange + " airDamage:" + AirDamage + " building:" + Building + " flyer:" + Flyer + " spellcaster:" + SpellCaster + " worker:" + Worker + " whatBuilds:" + WhatBuilds;
		}
	}
}