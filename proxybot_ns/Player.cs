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
        /// <summary>the player identifier </summary>
        public int PlayerID { get; set; }

        /// <summary>the player's race, see Constants.java for definitions </summary>
        public int Race { get; set; }

        /// <summary>current mineral supply </summary>
        public int Minerals { get; private set; }

        /// <summary>current gas supply </summary>
        public int Gas { get; private set; }

        /// <summary>amount of supply used by the player </summary>
        public int SupplyUsed { get; private set; }

        /// <summary>amount of supply provided by the player </summary>
        public int SupplyTotal { get; private set; }

        /// <summary>array of the unit types the bot can afford that it has the tech for </summary>
        public bool[] UnitProduction { get; private set; }

        /// <summary>array of the tech types the bot can afford that it has the tech for </summary>
        public bool[] TechProduction { get; private set; }

        /// <summary>array of the upgrade types the bot can afford that it has the tech for </summary>
        public bool[] UpgradeProduction { get; private set; }

		public Player()
		{
            UnitProduction = new bool[Constants.NumUnitTypes];
            TechProduction = new bool[Constants.NumTechTypes];
            UpgradeProduction = new bool[Constants.NumUpgradeTypes];
		}
		
		/// <summary> Updates the players attributes given the command data.
		/// 
		/// Expects a message of the form "status;minerals;gas;supplyUsed;SupplyTotal:..."
		/// </summary>
		public virtual void update(String playerData)
		{
            String[] attributes = playerData.Split(';');
			
			Minerals = System.Int32.Parse(attributes[1]);
			Gas = System.Int32.Parse(attributes[2]);
			SupplyUsed = System.Int32.Parse(attributes[3]);
			SupplyTotal = System.Int32.Parse(attributes[4]);
			
			for (int i = 0; i < UnitProduction.Length; i++)
			{
				UnitProduction[i] = attributes[5][i] == '1';
			}
			
			for (int i = 0; i < TechProduction.Length; i++)
			{
				TechProduction[i] = attributes[6][i] == '1';
			}
			
			for (int i = 0; i < UpgradeProduction.Length; i++)
			{
				UpgradeProduction[i] = attributes[7][i] == '1';
			}
		}
		
		public override System.String ToString()
		{
			return "mins:" + Minerals + " gas:" + Gas + " supplyUsed:" + SupplyUsed + " supplyTotal:" + SupplyTotal;
		}
	}
}