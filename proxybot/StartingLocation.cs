using System;
namespace starcraftbot.proxybot
{
	/// <summary> Represents a starting location in StarCraft.
	/// 
	/// Note: x and y are in tile coordinates
	/// </summary>
	public class StartingLocation
	{
		virtual public int X
		{
			get
			{
				return x;
			}
			
		}
		virtual public int Y
		{
			get
			{
				return y;
			}
			
		}
		
		private int x;
		
		private int y;
		
		/// <summary> Parses the starting locations.</summary>
		public static System.Collections.ArrayList getLocations(System.String locationData)
		{
			System.Collections.ArrayList locations = new System.Collections.ArrayList();
			
			System.String[] locs = locationData.Split(":".ToCharArray());
			bool first = true;
			
			foreach(String location in locs)
			{
				if (first)
				{
					first = false;
					continue;
				}
				
				System.String[] coords = location.Split(";".ToCharArray());
				
				StartingLocation loc = new StartingLocation();
				loc.x = System.Int32.Parse(coords[0]) + 2;
				loc.y = System.Int32.Parse(coords[1]) + 1;
				locations.Add(loc);
			}
			
			return locations;
		}
		
		public override System.String ToString()
		{
			return x + "," + y;
		}
	}
}