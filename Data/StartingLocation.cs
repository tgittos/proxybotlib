using System;
using System.Collections;
namespace ProxyBotLib.Data
{
	/// <summary> Represents a starting location in StarCraft.
	/// 
	/// Note: x and y are in tile coordinates
	/// </summary>
	public class StartingLocation
	{
        public int X { get; private set; }
        public int Y { get; private set; }
		
		/// <summary> Parses the starting locations.</summary>
		public static ArrayList getLocations(String locationData)
		{
			ArrayList locations = new ArrayList();
			
			String[] locs = locationData.Split(':');
			bool first = true;
			
			foreach(String location in locs)
			{
				if (first)
				{
					first = false;
					continue;
				}
				
				System.String[] coords = location.Split(';');

                StartingLocation loc = new StartingLocation
                {
                    X = System.Int32.Parse(coords[0]) + 2,
                    Y = System.Int32.Parse(coords[1]) + 1
                };
				locations.Add(loc);
			}
			
			return locations;
		}
		
		public override System.String ToString()
		{
			return X + "," + Y;
		}
	}
}