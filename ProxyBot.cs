using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Collections;
using ProxyBotLib.Agent;
using ProxyBotLib.Types;
using ProxyBotLib.Data;

namespace ProxyBotLib
{
	/// <summary> StarCraft AI Interface.
	/// 
	/// Maintains StarCraft state and provides hooks for StarCraft commands.
	/// 
	/// Note: all coordinates are specified in tile coordinates.
	/// </summary>
	public class ProxyBot
	{
		virtual public MapData Map
		{
			get
			{
				return map;
			}
			
		}
		virtual public ArrayList StartingLocations
		{
			get
			{
				return startingLocations;
			}
			
		}
		virtual public ArrayList Units
		{
			get
			{
				return units;
			}
			
		}
		virtual public ArrayList TechTypes
		{
			get
			{
				return techTypes;
			}
			
		}
		virtual public ArrayList UpgradeTypes
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
		public bool allowUserControl = true;
		
		/// <summary>turn on complete information </summary>
		public bool completeInformation = true;

        public bool logCommands = true;

        public bool terrainAnalysis = false;
		
		/// <summary>log a verbose amount of data </summary>
		public bool verboseLogging = false;
		
		/// <summary>bring up the ProxyBot GUI </summary>
		public bool showGUI = true;
		
		/// <summary>Start up the StarCraft agent class </summary>
		public bool runAgent = true;
		
		/// <summary>port to start the server socket on </summary>
		public int port = 13337;
		
		/// <summary>map information </summary>
		private MapData map;
		
		/// <summary>player (bot) attributes </summary>
		private Player player = new Player();
		
		/// <summary>enemy attributes </summary>
		private Player enemy = new Player();
		
		/// <summary>StarCraft unit types </summary>
        private Dictionary<int, UnitType> unitTypes;
		
		/// <summary>a list of the starting locations </summary>
		private System.Collections.ArrayList startingLocations;
		
		/// <summary>a list of the units </summary>
		private System.Collections.ArrayList units;
		
		/// <summary>list of tech types </summary>
		private System.Collections.ArrayList techTypes;
		
		/// <summary>list of upgrade types </summary>
		private System.Collections.ArrayList upgradeTypes;
		
		/// <summary>queued up commands (orders) to send to StarCraft </summary>
		public System.Collections.ArrayList commandQueue = new System.Collections.ArrayList();
		
		/// <summary>message number of commands to send to starcraft per response </summary>
		private int maxCommandsPerMessage = 20;

        private IAgent agent;

        private ProxyBotLib.GUI.Map guiMap;

		public ProxyBot(IAgent pAgent)
		{
            agent = pAgent;
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
            //Start listening for StarCraft
            TcpListener serverSocket;
            serverSocket = new TcpListener(port);
            serverSocket.Start();
			
            //Accept from the connection from StarCraft
			Console.Out.WriteLine("Waiting for client");
			TcpClient clientSocket = serverSocket.AcceptTcpClient();
			Console.Out.WriteLine("Client connected");

            //Get a reference to the StarCraft stream
            NetworkStream outputStream = clientSocket.GetStream();
            StreamReader starcraftStream = new StreamReader(outputStream, Encoding.Default);
			
			Console.Out.WriteLine("Waiting for client ACK message");
			String playerData = starcraftStream.ReadLine();
			
			//Send bot options to StarCraft
			String botOptions = (allowUserControl ? "1" : "0") + 
                                (completeInformation ? "1" : "0") +
                                (logCommands ? "1" : "0") +
                                (terrainAnalysis ? "1" : "0");
            byte[] optionsData = Encoding.ASCII.GetBytes(botOptions);
            outputStream.Write(optionsData, 0, optionsData.Length);

            configurePlayers(playerData);

			//Read data from StarCraft
			String unitTypeData = starcraftStream.ReadLine();
			String locationData = starcraftStream.ReadLine();
			String mapData = starcraftStream.ReadLine();
            if (terrainAnalysis)
            {
                //Analyse terrain
                String chokeData = starcraftStream.ReadLine();
                String basesData = starcraftStream.ReadLine();
            }
			String techTypeData = starcraftStream.ReadLine();
			String upgradeTypeData = starcraftStream.ReadLine();

            //Set data
			startingLocations = StartingLocation.getLocations(locationData);
			unitTypes = UnitType.getUnitTypes(unitTypeData);
			map = new MapData(mapData);
			techTypes = TechType.getTechTypes(techTypeData);
			upgradeTypes = UpgradeType.getUpgradeTypes(upgradeTypeData);
			
			// show information received from the StarCraft client
			if (verboseLogging)
			{
                logStartStateToConsole();
			}
			
			// display game state?
			if (showGUI)
			{
                guiMap = new ProxyBotLib.GUI.Map(this);
                guiMap.Run();
			}

            //Start the agent
            if (runAgent)
            {
                new ThreadedAgent(agent, this).Start();
            }

            DateTime lastRedraw = DateTime.Now;
            TimeSpan redrawPeriod = new TimeSpan(0, 0, 1);
			
			// begin the communication loop
			while (playerData != null)
			{
				String unitUpdateData = starcraftStream.ReadLine();
                String playerUpdateData = unitUpdateData.Split(':')[0];
				
                // update game state
                if (lastRedraw.Add(redrawPeriod) < DateTime.Now && guiMap != null)
                {
                    guiMap.Refresh();
                    lastRedraw = DateTime.Now;
                }

				player.update(playerUpdateData);
				units = Unit.getUnits(unitUpdateData, unitTypes);
				if (verboseLogging)
				{
                    logAllUnitsToConsole();
				}
				
				// build the command send
				StringBuilder commandData = new StringBuilder("commands");
				lock (commandQueue.SyncRoot)
				{
					int commandsAdded = 0;
					while (commandQueue.Count > 0 && commandsAdded < maxCommandsPerMessage)
					{
						commandsAdded++;
                        //Remove the next command
                        int nextCommandIndex = commandQueue.Count - 1;
                        var command = (Commands.Command)commandQueue[nextCommandIndex];
						commandQueue.RemoveAt(nextCommandIndex);
                        //Add it to the data to be sent to StarCraft
                        commandData.Append(command.ToString());
					}
				}
				
				// send commands to the starcraft client
                byte[] commandBytes = Encoding.ASCII.GetBytes(commandData.ToString());
                outputStream.Write(commandBytes, 0, commandBytes.Length);
			}
		}

        private void logAllUnitsToConsole()
        {
            foreach (Unit unit in units)
            {
                //UPGRADE_TODO: Method 'java.io.PrintStream.println' was converted to 'System.Console.Out.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintStreamprintln_javalangObject'"
                Console.Out.WriteLine(unit);
            }
        }
        private void logStartStateToConsole()
        {
            Console.Out.WriteLine(startingLocations.ToStarcraftString());
            map.print();

            foreach (UnitType type in unitTypes.Values)
            {
                Console.Out.WriteLine(type);
            }

            foreach (TechType type in techTypes)
            {
                System.Console.Out.WriteLine("Type: " + type);
            }

            foreach (UpgradeType type in upgradeTypes)
            {
                System.Console.Out.WriteLine("Upgrade: " + type);
            }
        }

        private void configurePlayers(String playerData)
        {
            //Get player data
            String[] players = playerData.Split(':');
            int playerID = Int32.Parse(players[1]);
            int playerRace = Int32.Parse(players[2]);
            player.PlayerID = playerID;
            player.Race = playerRace;

            //Get enemy data
            int enemyID = Int32.Parse(players[3]);
            int enemyRace = Int32.Parse(players[4]);
            enemy.PlayerID = enemyID;
            enemy.Race = enemyRace;
        }
	}
}