using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using StarCraftBot_net.proxybot;
using starcraftbot.proxybot;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Collections;
using StarCraftBot_net.proxybot.Agent;

namespace StarCraftBot_net
{
	/// <summary> StarCraft AI Interface.
	/// 
	/// Maintains StarCraft state and provides hooks for StarCraft commands.
	/// 
	/// Note: all coordinates are specified in tile coordinates.
	/// </summary>
	public class ProxyBot
	{
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
			String botOptions = (allowUserControl ? "1" : "0") + (completeInformation ? "1" : "0");
            byte[] optionsData = Encoding.ASCII.GetBytes(botOptions);
            outputStream.Write(optionsData, 0, optionsData.Length);

            configurePlayers(playerData);

			//Read data from StarCraft
			String unitTypeData = starcraftStream.ReadLine();
			String locationData = starcraftStream.ReadLine();
			String mapData = starcraftStream.ReadLine();
			String techTypeData = starcraftStream.ReadLine();
			String upgradeTypeData = starcraftStream.ReadLine();

            //Set data
			startingLocations = StartingLocation.getLocations(locationData);
			unitTypes = UnitType.getUnitTypes(unitTypeData);
			map = new Map(mapData);
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
                new StarCraftBot_net.proxybot.GUI.Map().Run();
			}

            //Start the agent
            StarCraftAgent agent = new StarCraftAgent(this);
            new ThreadedAgent(agent).Start();
			
			// begin the communication loop
			while (playerData != null)
			{
				String unitData = starcraftStream.ReadLine();
				
				// update game state
				player.update(unitData);
				units = Unit.getUnits(unitData, unitTypes);
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
						System.Object tempObject;
						tempObject = commandQueue[commandQueue.Count - 1];
						commandQueue.RemoveAt(commandQueue.Count - 1);
						StarCraftBot_net.Commands.Command command = (StarCraftBot_net.Commands.Command)tempObject;
                        string commandString = ":" + (int)command.CommandID + ";" + command.UnitID + ";" + command.Arg0 + ";" + command.Arg1 + ";" + command.Arg2;
                        commandData.Append(commandString);
					}
				}
				
				// send commands to the starcraft client
				sbyte[] temp_sbyteArray2;
				temp_sbyteArray2 = SupportClass.ToSByteArray(SupportClass.ToByteArray(commandData.ToString()));
				clientSocket.GetStream().Write(SupportClass.ToByteArray(temp_sbyteArray2), 0, temp_sbyteArray2.Length);
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
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
        public Dictionary<int, UnitType> getUnitTypes()
        {
            return unitTypes;
        }
	}
}