using System;
namespace starcraftbot.proxybot
{
	/// <summary> Stores information about the player (Bot)
	/// 
	/// Note: the supply used and supply total variables are double what you would expect, because
	/// small units are represented as 1 supply in StarCraft.
	/// </summary>
	public class Player
	{
		public Player()
		{
			InitBlock();
		}
		private void  InitBlock()
		{
			unitProduction = new bool[Constants.NumUnitTypes];
			techProduction = new bool[Constants.NumTechTypes];
			upgradeProduction = new bool[Constants.NumUpgradeTypes];
		}
		virtual public int Minerals
		{
			get
			{
				return minerals;
			}
			
		}
		virtual public int Gas
		{
			get
			{
				return gas;
			}
			
		}
		virtual public int SupplyUsed
		{
			get
			{
				return supplyUsed;
			}
			
		}
		virtual public int SupplyTotal
		{
			get
			{
				return supplyTotal;
			}
			
		}
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
		virtual public int Race
		{
			get
			{
				return race;
			}
			
			set
			{
				this.race = value;
			}
			
		}
		virtual public bool[] UnitProduction
		{
			get
			{
				return unitProduction;
			}
			
		}
		virtual public bool[] TechProduction
		{
			get
			{
				return techProduction;
			}
			
		}
		virtual public bool[] UpgradeProduction
		{
			get
			{
				return upgradeProduction;
			}
			
		}
		
		/// <summary>the player identifier </summary>
		private int playerID;
		
		/// <summary>the player's race, see Constants.java for definitions </summary>
		private int race;
		
		/// <summary>current mineral supply </summary>
		private int minerals;
		
		/// <summary>current gas supply </summary>
		private int gas;
		
		/// <summary>amount of supply used by the player </summary>
		private int supplyUsed;
		
		/// <summary>amount of supply provided by the player </summary>
		private int supplyTotal;
		
		/// <summary>array of the unit types the bot can afford that it has the tech for </summary>
		//UPGRADE_NOTE: The initialization of  'unitProduction' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private bool[] unitProduction;
		
		/// <summary>array of the tech types the bot can afford that it has the tech for </summary>
		//UPGRADE_NOTE: The initialization of  'techProduction' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private bool[] techProduction;
		
		/// <summary>array of the upgrade types the bot can afford that it has the tech for </summary>
		//UPGRADE_NOTE: The initialization of  'upgradeProduction' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private bool[] upgradeProduction;
		
		/// <summary> Updates the players attributes given the command data.
		/// 
		/// Expects a message of the form "status;minerals;gas;supplyUsed;SupplyTotal:..."
		/// </summary>
		public virtual void  update(System.String playerData)
		{
			System.String[] attributes = playerData.split(":")[0].split(";");
			
			minerals = System.Int32.Parse(attributes[1]);
			gas = System.Int32.Parse(attributes[2]);
			supplyUsed = System.Int32.Parse(attributes[3]);
			supplyTotal = System.Int32.Parse(attributes[4]);
			
			for (int i = 0; i < unitProduction.Length; i++)
			{
				unitProduction[i] = attributes[5][i] == '1';
			}
			
			for (int i = 0; i < techProduction.Length; i++)
			{
				techProduction[i] = attributes[6][i] == '1';
			}
			
			for (int i = 0; i < upgradeProduction.Length; i++)
			{
				upgradeProduction[i] = attributes[7][i] == '1';
			}
		}
		
		public override System.String ToString()
		{
			return "mins:" + minerals + " gas:" + gas + " supplyUsed:" + supplyUsed + " supplyTotal:" + supplyTotal;
		}
	}
}