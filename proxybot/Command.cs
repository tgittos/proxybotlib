using System;
namespace starcraftbot.proxybot
{
	/// <summary> Representation of a command (Order) in StarCraft. The list of commands is enumerated here:
	/// http://code.google.com/p/bwapi/wiki/Orders
	/// 
	/// The actual function definitions are provided in Unit.h on the AIModule side:
	/// virtual bool attackMove(Position position) = 0;
	/// virtual bool attackUnit(Unit* target) = 0;
	/// virtual bool rightClick(Position position) = 0;
	/// virtual bool rightClick(Unit* target) = 0;
	/// virtual bool train(UnitType type) = 0;
	/// virtual bool build(TilePosition position, UnitType type) = 0;
	/// virtual bool buildAddon(UnitType type) = 0;
	/// virtual bool research(TechType tech) = 0;
	/// virtual bool upgrade(UpgradeType upgrade) = 0;
	/// virtual bool stop() = 0;
	/// virtual bool holdPosition() = 0;
	/// virtual bool patrol(Position position) = 0;
	/// virtual bool follow(Unit* target) = 0;
	/// virtual bool setRallyPosition(Position target) = 0;
	/// virtual bool setRallyUnit(Unit* target) = 0;
	/// virtual bool repair(Unit* target) = 0;
	/// virtual bool morph(UnitType type) = 0;
	/// virtual bool burrow() = 0;
	/// virtual bool unburrow() = 0;
	/// virtual bool siege() = 0;
	/// virtual bool unsiege() = 0;
	/// virtual bool cloak() = 0;
	/// virtual bool decloak() = 0;
	/// virtual bool lift() = 0;
	/// virtual bool land(TilePosition position) = 0;
	/// virtual bool load(Unit* target) = 0;
	/// virtual bool unload(Unit* target) = 0;
	/// virtual bool unloadAll() = 0;
	/// virtual bool unloadAll(Position position) = 0;
	/// virtual bool cancelConstruction() = 0;
	/// virtual bool haltConstruction() = 0;
	/// virtual bool cancelMorph() = 0;
	/// virtual bool cancelTrain() = 0;
	/// virtual bool cancelTrain(int slot) = 0;
	/// virtual bool cancelAddon() = 0;
	/// virtual bool cancelResearch() = 0;
	/// virtual bool cancelUpgrade() = 0;
	/// virtual bool useTech(TechType tech) = 0;
	/// virtual bool useTech(TechType tech, Position position) = 0;
	/// virtual bool useTech(TechType tech, Unit* target) = 0;
	/// 
	/// On the java side, the command function definitions are provided in ProxyBot.
	/// 
	/// In StarCraft, commands take up to 3 arguments. 
	/// </summary>
	public class Command
	{
        
		virtual public int UnitID
		{
			get
			{
				return unitID;
			}
			
		}
		virtual public int Arg0
		{
			get
			{
				return arg0;
			}
			
		}
		virtual public int Arg1
		{
			get
			{
				return arg1;
			}
			
		}
		virtual public int Arg2
		{
			get
			{
				return arg2;
			}
			
		}
		
		/// <summary>the command to execute, as defined by StarCraftCommand </summary>
        private StarCraftCommand command;

        public StarCraftCommand CommandID
        {
            get { return command; }
        }
		
		/// <summary>the unit to execute the command </summary>
		private int unitID;
		
		/// <summary>the first argument </summary>
		private int arg0;
		
		/// <summary>the second argument </summary>
		private int arg1;
		
		/// <summary>the third argument </summary>
		private int arg2;
		
		/// <summary> Creates a command
		/// 
		/// </summary>
		/// <param name="command">
		/// </param>
		/// <param name="unitID">
		/// </param>
		/// <param name="arg0">
		/// </param>
		/// <param name="arg1">
		/// </param>
		/// <param name="arg2">
		/// </param>
		public Command(StarCraftCommand command, int unitID, int arg0, int arg1, int arg2)
		{
            this.command = command;
			this.unitID = unitID;
			this.arg0 = arg0;
			this.arg1 = arg1;
			this.arg2 = arg2;
		}

        public virtual int getCommand()
		{
			return (int)command;
		}
	}
}