using System;
//UPGRADE_TODO: The type 'starcraftbot.proxybot.Command.StarCraftCommand' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using System.Collections.Generic;
namespace starcraftbot.proxybot
{
	/// <summary> StarCraft AI Interface.
	/// 
	/// Maintains StarCraft state and provides hooks for StarCraft commands.
	/// 
	/// Note: all coordinates are specified in tile coordinates.
	/// </summary>
	public class ProxyBot
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassThread' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassThread:SupportClass.ThreadClass
		{
			public AnonymousClassThread(ProxyBot enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(ProxyBot enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private ProxyBot enclosingInstance;
			public ProxyBot Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			override public void  Run()
			{
				new StarCraftFrame().start();
			}
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassThread1' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassThread1:SupportClass.ThreadClass
		{
			public AnonymousClassThread1(ProxyBot enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(ProxyBot enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private ProxyBot enclosingInstance;
			public ProxyBot Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			override public void  Run()
			{
				new StarCraftAgent().start();
			}
		}
		private void  InitBlock()
		{
			return unitTypes;
		}
		/// <summary> Returns the singleton instance of ProxyBot.</summary>
		public static ProxyBot Proxy
		{
			get
			{
				return thisInstance;
			}
			
		}
		virtual public Map Map
		{
			get
			{
				return map;
			}
			
		}
		virtual public System.Collections.ArrayList StartingLocations
		{
			get
			{
				return startingLocations;
			}
			
		}
		virtual public System.Collections.ArrayList Units
		{
			get
			{
				return units;
			}
			
		}
		virtual public System.Collections.ArrayList TechTypes
		{
			get
			{
				return techTypes;
			}
			
		}
		virtual public System.Collections.ArrayList UpgradeTypes
		{
			get
			{
				return upgradeTypes;
			}
			
		}
		virtual public int PlayerID
		{
			get
			{
				return player.PlayerID;
			}
			
		}
		virtual public int EnemyID
		{
			get
			{
				return enemy.PlayerID;
			}
			
		}
		virtual public Player Enemy
		{
			get
			{
				return enemy;
			}
			
		}
		virtual public Player Player
		{
			get
			{
				return player;
			}
			
		}
		
		/// <summary>allow the user to control units </summary>
		public static bool allowUserControl = true;
		
		/// <summary>turn on complete information </summary>
		public static bool completeInformation = true;
		
		/// <summary>log a verbose amount of data </summary>
		public static bool verboseLogging = false;
		
		/// <summary>bring up the ProxyBot GUI </summary>
		public static bool showGUI = true;
		
		/// <summary>Start up the StarCraft agent class </summary>
		public static bool runAgent = true;
		
		/// <summary>port to start the server socket on </summary>
		public static int port = 13337;
		
		/// <summary>map information </summary>
		private Map map;
		
		/// <summary>player (bot) attributes </summary>
		private Player player = new Player();
		
		/// <summary>enemy attributes </summary>
		private Player enemy = new Player();
		
		/// <summary>StarCraft unit types </summary>
		//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
		private System.Collections.Hashtable unitTypes;
		
		/// <summary>a list of the starting locations </summary>
		private System.Collections.ArrayList startingLocations;
		
		/// <summary>a list of the units </summary>
		private System.Collections.ArrayList units;
		
		/// <summary>list of tech types </summary>
		private System.Collections.ArrayList techTypes;
		
		/// <summary>list of upgrade types </summary>
		private System.Collections.ArrayList upgradeTypes;
		
		/// <summary>queued up commands (orders) to send to StarCraft </summary>
		private System.Collections.ArrayList commandQueue = new System.Collections.ArrayList();
		
		/// <summary>message number of commands to send to starcraft per response </summary>
		private int maxCommandsPerMessage = 20;
		
		/// <summary>ProxyBot is a singleton </summary>
		private static ProxyBot thisInstance;
		
		/// <summary> Starts the proxy bot.</summary>
		[STAThread]
		public static void  Main(System.String[] args)
		{
			ProxyBot proxyBot = new ProxyBot();
			
			try
			{
				proxyBot.start();
			}
			catch (System.Net.Sockets.SocketException e)
			{
				SupportClass.WriteStackTrace(e, Console.Error);
				System.Environment.Exit(0);
			}
			catch (System.Exception e)
			{
				SupportClass.WriteStackTrace(e, Console.Error);
			}
		}
		
		public ProxyBot()
		{
			InitBlock();
			thisInstance = this;
		}

       
		
		/// <summary> Starts up a server socket and waits for StarCraft to initiate communication.
		/// 
		/// StarCraft sends the ProxyBot several messages about unit type, upgrades, locations.
		/// 
		/// Then the main communication loop begins. The ProxyBot waits for status upgrades from StarCraft
		/// and then sends queued up commands. The socket on the StarCraft end is a blocking socket, so 
		/// the ProxyBot will cause StarCraft to pause if it doesn't immediately respond. 
		/// </summary>
		public virtual void  start()
		{
			System.Net.Sockets.TcpListener temp_tcpListener;
			temp_tcpListener = new System.Net.Sockets.TcpListener(System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0], port);
			temp_tcpListener.Start();
			System.Net.Sockets.TcpListener serverSocket = temp_tcpListener;
			
			System.Console.Out.WriteLine("Waiting for client");
			System.Net.Sockets.TcpClient clientSocket = serverSocket.AcceptTcpClient();
			
			System.Console.Out.WriteLine("Client connected");
			//UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
			//UPGRADE_WARNING: At least one expression was used more than once in the target code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1181'"
			System.IO.StreamReader reader = new System.IO.StreamReader(new System.IO.StreamReader(clientSocket.GetStream(), System.Text.Encoding.Default).BaseStream, new System.IO.StreamReader(clientSocket.GetStream(), System.Text.Encoding.Default).CurrentEncoding);
			
			System.Console.Out.WriteLine("Waiting for client ACK message");
			System.String line = reader.ReadLine();
			
			// send bot options
			System.String botOptions = (allowUserControl?"1":"0") + (completeInformation?"1":"0");
			sbyte[] temp_sbyteArray;
			temp_sbyteArray = SupportClass.ToSByteArray(SupportClass.ToByteArray(botOptions));
			clientSocket.GetStream().Write(SupportClass.ToByteArray(temp_sbyteArray), 0, temp_sbyteArray.Length);
			
			// get the rest of the data
			System.String unitTypeData = reader.ReadLine();
			System.String locationData = reader.ReadLine();
			System.String mapData = reader.ReadLine();
			System.String techTypeData = reader.ReadLine();
			System.String upgradeTypeData = reader.ReadLine();
			
			System.String[] players = line.split(":");
			int playerID = System.Int32.Parse(players[1]);
			int playerRace = System.Int32.Parse(players[2]);
			player.PlayerID = playerID;
			player.Race = playerRace;
			
			int enemyID = System.Int32.Parse(players[3]);
			int enemyRace = System.Int32.Parse(players[4]);
			enemy.PlayerID = enemyID;
			enemy.Race = enemyRace;
			
			startingLocations = StartingLocation.getLocations(locationData);
			unitTypes = UnitType.getUnitTypes(unitTypeData);
			map = new Map(mapData);
			techTypes = TechType.getTechTypes(techTypeData);
			upgradeTypes = UpgradeType.getUpgradeTypes(upgradeTypeData);
			
			// show information received from the StarCraft client
			if (verboseLogging)
			{
				//UPGRADE_TODO: Method 'java.io.PrintStream.println' was converted to 'System.Console.Out.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintStreamprintln_javalangObject'"
				System.Console.Out.WriteLine(SupportClass.CollectionToString(startingLocations));
				map.print();
				
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				foreach(UnitType type in unitTypes.values())
				{
					//UPGRADE_TODO: Method 'java.io.PrintStream.println' was converted to 'System.Console.Out.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintStreamprintln_javalangObject'"
					System.Console.Out.WriteLine(type);
				}
				
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				foreach(TechType type in techTypes)
				{
					System.Console.Out.WriteLine("Type: " + type);
				}
				
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				foreach(UpgradeType type in upgradeTypes)
				{
					System.Console.Out.WriteLine("Upgrade: " + type);
				}
			}
			
			// display game state?
			if (showGUI)
			{
				new AnonymousClassThread(this).Start();
			}
			
			// begin the communication loop
			bool first = true;
			while (true)
			{
				System.String unitData = reader.ReadLine();
				if (line == null)
				{
					break;
				}
				
				// update game state
				player.update(unitData);
				units = Unit.getUnits(unitData, unitTypes);
				if (verboseLogging)
				{
					//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
					foreach(Unit unit in units)
					{
						//UPGRADE_TODO: Method 'java.io.PrintStream.println' was converted to 'System.Console.Out.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintStreamprintln_javalangObject'"
						System.Console.Out.WriteLine(unit);
					}
				}
				
				// run the agent?
				if (first)
				{
					first = false;
					
					if (runAgent)
					{
						new AnonymousClassThread1(this).Start();
					}
				}
				
				// build the command send
				System.Text.StringBuilder commandData = new System.Text.StringBuilder("commands");
				lock (commandQueue.SyncRoot)
				{
					int commandsAdded = 0;
					
					while (commandQueue.Count > 0 && commandsAdded < maxCommandsPerMessage)
					{
						commandsAdded++;
						System.Object tempObject;
						tempObject = commandQueue[commandQueue.Count - 1];
						commandQueue.RemoveAt(commandQueue.Count - 1);
						Command command = tempObject;
						commandData.Append(":" + command.getCommand() + ";" + command.UnitID + ";" + command.Arg0 + ";" + command.Arg1 + ";" + command.Arg2);
					}
				}
				
				// send commands to the starcraft client
				sbyte[] temp_sbyteArray2;
				temp_sbyteArray2 = SupportClass.ToSByteArray(SupportClass.ToByteArray(commandData.ToString()));
				clientSocket.GetStream().Write(SupportClass.ToByteArray(temp_sbyteArray2), 0, temp_sbyteArray2.Length);
			}
		}
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
        public Dictionary<int, UnitType> getUnitTypes()
        {
            return unitTypes;
        }
		
		/// <summary> Adds a command to the command queue.
		/// 
		/// </summary>
		/// <param name="command">- the command to execture, see the Orders enumeration
		/// </param>
		/// <param name="unitID">- the unit to control
		/// </param>
		/// <param name="arg0">- the first command argument
		/// </param>
		/// <param name="arg1">- the second command argument
		/// </param>
		/// <param name="arg2">- the third command argument
		/// </param>
		private void  doCommand(starcraftbot.proxybot.Command.StarCraftCommand command, int unitID, int arg0, int arg1, int arg2)
		{
			lock (commandQueue.SyncRoot)
			{
				commandQueue.Add(new Command(command, unitID, arg0, arg1, arg2));
			}
		}
		
		/// <summary>*******************************************************
		/// Commands
		/// *******************************************************
		/// </summary>
		
		/// <summary> Tells the unit to attack move the specific location (in tile coordinates).
		/// 
		/// // virtual bool attackMove(Position position) = 0;
		/// </summary>
		public virtual void  attackMove(int unitID, int x, int y)
		{
			doCommand(StarCraftCommand.attackMove, unitID, x, y, 0);
		}
		
		/// <summary> Tells the unit to attack another unit.
		/// 
		/// // virtual bool attackUnit(Unit* target) = 0;
		/// </summary>
		public virtual void  attackUnit(int unitID, int targetID)
		{
			doCommand(StarCraftCommand.attackUnit, unitID, targetID, 0, 0);
		}
		
		/// <summary> Tells the unit to right click (move) to the specified location (in tile coordinates).
		/// 
		/// // virtual bool rightClick(Position position) = 0;
		/// </summary>
		public virtual void  rightClick(int unitID, int x, int y)
		{
			doCommand(StarCraftCommand.rightClick, unitID, x, y, 0);
		}
		
		/// <summary> Tells the unit to right click (move) on the specified target unit 
		/// (Includes resources).
		/// 
		/// // virtual bool rightClick(Unit* target) = 0;
		/// </summary>
		public virtual void  rightClick(int unitID, int targetID)
		{
			doCommand(StarCraftCommand.rightClickUnit, unitID, targetID, 0, 0);
		}
		
		/// <summary> Tells the building to train the specified unit type.
		/// 
		/// // virtual bool train(UnitType type) = 0;
		/// </summary>
		public virtual void  train(int unitID, int typeID)
		{
			doCommand(StarCraftCommand.train, unitID, typeID, 0, 0);
		}
		
		/// <summary> Tells a worker unit to construct a building at the specified location.
		/// 
		/// // virtual bool build(TilePosition position, UnitType type) = 0;
		/// </summary>
		public virtual void  build(int unitID, int tx, int ty, int typeID)
		{
			doCommand(StarCraftCommand.build, unitID, tx, ty, typeID);
		}
		
		/// <summary> Tells the building to build the specified add on.
		/// 
		/// // virtual bool buildAddon(UnitType type) = 0;
		/// </summary>
		public virtual void  buildAddon(int unitID, int typeID)
		{
			doCommand(StarCraftCommand.buildAddon, unitID, typeID, 0, 0);
		}
		
		/// <summary> Tells the building to research the specified tech type.
		/// 
		/// // virtual bool research(TechType tech) = 0;
		/// </summary>
		public virtual void  research(int unitID, int techTypeID)
		{
			doCommand(StarCraftCommand.research, unitID, techTypeID, 0, 0);
		}
		
		/// <summary> Tells the building to upgrade the specified upgrade type.
		/// 
		/// // virtual bool upgrade(UpgradeType upgrade) = 0;
		/// </summary>
		public virtual void  upgrade(int unitID, int upgradeTypeID)
		{
			doCommand(StarCraftCommand.upgrade, unitID, upgradeTypeID, 0, 0);
		}
		
		/// <summary> Orders the unit to stop moving. The unit will chase enemies that enter its vision.
		/// 
		/// // virtual bool stop() = 0;
		/// </summary>
		public virtual void  stop(int unitID)
		{
			doCommand(StarCraftCommand.stop, unitID, 0, 0, 0);
		}
		
		/// <summary> Orders the unit to hold position. The unit will not chase enemies that enter its vision.
		/// 
		/// // virtual bool holdPosition() = 0;
		/// </summary>
		public virtual void  holdPosition(int unitID)
		{
			doCommand(StarCraftCommand.holdPosition, unitID, 0, 0, 0);
		}
		
		/// <summary> Orders the unit to patrol between its current location and the specified location.
		/// 
		/// // virtual bool patrol(Position position) = 0;
		/// </summary>
		public virtual void  patrol(int unitID, int x, int y)
		{
			doCommand(StarCraftCommand.patrol, unitID, x, y, 0);
		}
		
		/// <summary> Orders a unit to follow a target unit.
		/// 
		/// // virtual bool follow(Unit* target) = 0;
		/// </summary>
		public virtual void  follow(int unitID, int targetID)
		{
			doCommand(StarCraftCommand.follow, unitID, targetID, 0, 0);
		}
		
		/// <summary> Sets the rally location for a building. 
		/// 
		/// // virtual bool setRallyPosition(Position target) = 0;
		/// </summary>
		public virtual void  setRallyPosition(int unitID, int x, int y)
		{
			doCommand(StarCraftCommand.setRallyPosition, unitID, x, y, 0);
		}
		
		/// <summary> Sets the rally location for a building based on the target unit's current position.
		/// 
		/// // virtual bool setRallyUnit(Unit* target) = 0;
		/// </summary>
		public virtual void  setRallyUnit(int unitID, int targetID)
		{
			doCommand(StarCraftCommand.setRallyUnit, unitID, targetID, 0, 0);
		}
		
		/// <summary> Instructs an SCV to repair a target unit.
		/// 
		/// // virtual bool repair(Unit* target) = 0;
		/// </summary>
		public virtual void  repair(int unitID, int targetID)
		{
			doCommand(StarCraftCommand.repair, unitID, targetID, 0, 0);
		}
		
		/// <summary> Orders a zerg unit to morph to a different unit type.
		/// 
		/// // virtual bool morph(UnitType type) = 0;
		/// </summary>
		public virtual void  morph(int unitID, int typeID)
		{
			doCommand(StarCraftCommand.morph, unitID, typeID, 0, 0);
		}
		
		/// <summary> Tells a zerg unit to burrow. Burrow must be upgraded for non-lurker units.
		/// 
		/// // virtual bool burrow() = 0;
		/// </summary>
		public virtual void  burrow(int unitID)
		{
			doCommand(StarCraftCommand.burrow, unitID, 0, 0, 0);
		}
		
		/// <summary> Tells a burrowed unit to unburrow.
		/// 
		/// // virtual bool unburrow() = 0;
		/// </summary>
		public virtual void  unburrow(int unitID)
		{
			doCommand(StarCraftCommand.unburrow, unitID, 0, 0, 0);
		}
		
		/// <summary> Orders a siege tank to siege.
		/// 
		/// // virtual bool siege() = 0;
		/// </summary>
		public virtual void  siege(int unitID)
		{
			doCommand(StarCraftCommand.siege, unitID, 0, 0, 0);
		}
		
		/// <summary> Orders a siege tank to un-siege.
		/// 
		/// // virtual bool unsiege() = 0;
		/// </summary>
		public virtual void  unsiege(int unitID)
		{
			doCommand(StarCraftCommand.unsiege, unitID, 0, 0, 0);
		}
		
		/// <summary> Tells a unit to cloak. Works for ghost and wraiths. 
		/// 
		/// // virtual bool cloak() = 0;
		/// </summary>
		public virtual void  cloak(int unitID)
		{
			doCommand(StarCraftCommand.cloak, unitID, 0, 0, 0);
		}
		
		/// <summary> Tells a unit to decloak, works for ghosts and wraiths.
		/// 
		/// // virtual bool decloak() = 0;
		/// </summary>
		public virtual void  decloak(int unitID)
		{
			doCommand(StarCraftCommand.decloak, unitID, 0, 0, 0);
		}
		
		/// <summary> Commands a Terran building to lift off.
		/// 
		/// // virtual bool lift() = 0;
		/// </summary>
		public virtual void  lift(int unitID)
		{
			doCommand(StarCraftCommand.lift, unitID, 0, 0, 0);
		}
		
		/// <summary> Commands a terran building to land at the specified location.
		/// 
		/// // virtual bool land(TilePosition position) = 0;
		/// </summary>
		public virtual void  land(int unitID, int tx, int ty)
		{
			doCommand(StarCraftCommand.land, unitID, tx, ty, 0);
		}
		
		/// <summary> Orders the transport unit to load the target unit.
		/// 
		/// // virtual bool load(Unit* target) = 0;
		/// </summary>
		public virtual void  load(int unitID, int targetID)
		{
			doCommand(StarCraftCommand.load, unitID, targetID, 0, 0);
		}
		
		/// <summary> Orders a transport unit to unload the target unit at the current transport location.
		/// 
		/// // virtual bool unload(Unit* target) = 0;
		/// </summary>
		public virtual void  unload(int unitID, int targetID)
		{
			doCommand(StarCraftCommand.unload, unitID, targetID, 0, 0);
		}
		
		/// <summary> Orders a transport to unload all units at the current location.
		/// 
		/// // virtual bool unloadAll() = 0;
		/// </summary>
		public virtual void  unloadAll(int unitID)
		{
			doCommand(StarCraftCommand.unloadAll, unitID, 0, 0, 0);
		}
		
		/// <summary> Orders a unit to unload all units at the target location.
		/// 
		/// // virtual bool unloadAll(Position position) = 0;
		/// </summary>
		public virtual void  unloadAll(int unitID, int x, int y)
		{
			doCommand(StarCraftCommand.unloadAllPosition, unitID, x, y, 0);
		}
		
		/// <summary> Orders a being to stop being constructed.
		/// 
		/// // virtual bool cancelConstruction() = 0;
		/// </summary>
		public virtual void  cancelConstruction(int unitID)
		{
			doCommand(StarCraftCommand.cancelConstruction, unitID, 0, 0, 0);
		}
		
		/// <summary> Tells an scv to pause construction on a building.
		/// 
		/// // virtual bool haltConstruction() = 0;
		/// </summary>
		public virtual void  haltConstruction(int unitID)
		{
			doCommand(StarCraftCommand.haltConstruction, unitID, 0, 0, 0);
		}
		
		/// <summary> Orders a zerg unit to stop morphing.
		/// 
		/// // virtual bool cancelMorph() = 0;
		/// </summary>
		public virtual void  cancelMorph(int unitID)
		{
			doCommand(StarCraftCommand.cancelMorph, unitID, 0, 0, 0);
		}
		
		/// <summary> Tells a building to remove the last unit from its training queue.
		/// 
		/// // virtual bool cancelTrain() = 0;
		/// </summary>
		public virtual void  cancelTrain(int unitID)
		{
			doCommand(StarCraftCommand.cancelTrain, unitID, 0, 0, 0);
		}
		
		/// <summary> Tells a building to remove a specific unit from its queue.
		/// 
		/// // virtual bool cancelTrain(int slot) = 0;
		/// </summary>
		public virtual void  cancelTrain(int unitID, int slot)
		{
			doCommand(StarCraftCommand.cancelTrainSlot, unitID, slot, 0, 0);
		}
		
		/// <summary> Orders a Terran building to stop constructing an add on.
		/// 
		/// // virtual bool cancelAddon() = 0;
		/// </summary>
		public virtual void  cancelAddon(int unitID)
		{
			doCommand(StarCraftCommand.cancelAddon, unitID, 0, 0, 0);
		}
		
		/// <summary> 
		/// Tells a building cancel a research in progress. 
		/// 
		/// // virtual bool cancelResearch() = 0;
		/// </summary>
		public virtual void  cancelResearch(int unitID)
		{
			doCommand(StarCraftCommand.cancelResearch, unitID, 0, 0, 0);
		}
		
		/// <summary> 
		/// Tells a building cancel an upgrade  in progress. 
		/// 
		/// // virtual bool cancelUpgrade() = 0;
		/// </summary>
		public virtual void  cancelUpgrade(int unitID)
		{
			doCommand(StarCraftCommand.cancelUpgrade, unitID, 0, 0, 0);
		}
		
		/// <summary> Tells the unit to use the specified tech, (i.e. STEM PACKS)
		/// 
		/// // virtual bool useTech(TechType tech) = 0;
		/// </summary>
		public virtual void  useTech(int unitID, int techTypeID)
		{
			doCommand(StarCraftCommand.useTech, unitID, techTypeID, 0, 0);
		}
		
		/// <summary> Tells the unit to use tech at the target location.
		/// 
		/// Note: for AOE spells such as plague.
		/// 
		/// // virtual bool useTech(TechType tech, Position position) = 0;
		/// </summary>
		public virtual void  useTech(int unitID, int techTypeID, int x, int y)
		{
			doCommand(StarCraftCommand.useTechPosition, unitID, techTypeID, x, y);
		}
		
		/// <summary> Tells the unit to use tech on the target unit.
		/// 
		/// Note: for targeted spells such as irradiate.
		/// 
		/// // virtual bool useTech(TechType tech, Unit* target) = 0;
		/// </summary>
		public virtual void  useTech(int unitID, int techTypeID, int targetID)
		{
			doCommand(StarCraftCommand.useTechTarget, unitID, techTypeID, targetID, 0);
		}
	}
}