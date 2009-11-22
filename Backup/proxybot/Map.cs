using System;
namespace starcraftbot.proxybot
{
	/// <summary> Stores tile information about a map in StarCraft.
	/// 
	/// Note: internally in StarCraft, the height and walkable arrays have a higher
	/// resolution than the size tile in this class. Each tile is actually
	/// a 4x4 grid, but this has been abstracted away for simplicity for now.
	/// </summary>
	public class Map
	{
		virtual public System.String MapName
		{
			get
			{
				return mapName;
			}
			
		}
		virtual public int MapWidth
		{
			get
			{
				return mapWidth;
			}
			
		}
		virtual public int MapHeight
		{
			get
			{
				return mapHeight;
			}
			
		}
		
		/// <summary>the map name </summary>
		private System.String mapName;
		
		/// <summary>number of tiles wide </summary>
		private int mapWidth;
		
		/// <summary>number of tiles high </summary>
		private int mapHeight;
		
		/// <summary>height array (valid values are 0,1,2) </summary>
		private int[][] height;
		
		/// <summary>buildable array </summary>
		private bool[][] buildable;
		
		/// <summary>walkable array </summary>
		private bool[][] walkable;
		
		/// <summary> Returns true if the map is walkable and the given tile coordinates.</summary>
		public virtual bool isWalkable(int tx, int ty)
		{
			return walkable[ty][tx];
		}
		
		/// <summary> Returns true if the map is buildable and the given tile coordinates.</summary>
		public virtual bool isBuildable(int tx, int ty)
		{
			return buildable[ty][tx];
		}
		
		/// <summary> Returns the height of the map at the given tile coordinates.</summary>
		public virtual int getHeight(int tx, int ty)
		{
			return height[ty][tx];
		}
		
		/// <summary> Creates a map based on the string recieved from the AIModule.
		/// 
		/// </summary>
		/// <param name="mapData">- mapname:width:height:data
		/// 
		/// Data is a character array where each tile is represented by 3 characters, 
		/// which specific height, buildable, walkable.
		/// </param>
		public Map(System.String mapData)
		{
			System.String[] map = mapData.Split(":".ToCharArray());
			System.String data = map[3];
			
			mapName = map[0];
			mapWidth = System.Int32.Parse(map[1]);
			mapHeight = System.Int32.Parse(map[2]);
			
			height = new int[mapHeight][];
			for (int i = 0; i < mapHeight; i++)
			{
				height[i] = new int[mapWidth];
			}
			buildable = new bool[mapHeight][];
			for (int i2 = 0; i2 < mapHeight; i2++)
			{
				buildable[i2] = new bool[mapWidth];
			}
			walkable = new bool[mapHeight][];
			for (int i3 = 0; i3 < mapHeight; i3++)
			{
				walkable[i3] = new bool[mapWidth];
			}
			
			int total = mapWidth * mapHeight;
			for (int i = 0; i < total; i++)
			{
				int w = i % mapWidth;
				int h = i / mapWidth;
				
				System.String tile = data.Substring(3 * i, (3 * i + 3) - (3 * i));
				
				height[h][w] = System.Int32.Parse(tile.Substring(0, (1) - (0)));
				buildable[h][w] = (1 == System.Int32.Parse(tile.Substring(1, (2) - (1))));
				walkable[h][w] = (1 == System.Int32.Parse(tile.Substring(2, (3) - (2))));
			}
		}
		
		/// <summary> Displays the main properties.</summary>
		public virtual void  print()
		{
			System.Console.Out.WriteLine("Name: " + mapName);
			System.Console.Out.WriteLine("Size: " + mapWidth + " x " + mapHeight);
			
			System.Console.Out.WriteLine("\nBuildable");
			System.Console.Out.WriteLine("---------");
			for (int y = 0; y < mapHeight; y++)
			{
				for (int x = 0; x < mapWidth; x++)
				{
					System.Console.Out.Write(buildable[y][x]?" ":"X");
				}
				
				System.Console.Out.WriteLine();
			}
			
			System.Console.Out.WriteLine("\nWalkable");
			System.Console.Out.WriteLine("--------");
			for (int y = 0; y < mapHeight; y++)
			{
				for (int x = 0; x < mapWidth; x++)
				{
					System.Console.Out.Write(walkable[y][x]?" ":"X");
				}
				
				System.Console.Out.WriteLine();
			}
			
			System.Console.Out.WriteLine("\nHeight");
			System.Console.Out.WriteLine("------");
			for (int y = 0; y < mapHeight; y++)
			{
				for (int x = 0; x < mapWidth; x++)
				{
					switch (height[y][x])
					{
						
						case 2: 
							System.Console.Out.Write(" ");
							break;
						
						case 1: 
							System.Console.Out.Write("*");
							break;
						
						case 0: 
							System.Console.Out.Write("X");
							break;
						}
				}
				
				System.Console.Out.WriteLine();
			}
		}
	}
}