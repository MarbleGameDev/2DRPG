using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.World.Regions {
	/// <summary>
	/// Region Files hold the data for a 100x100 region that is loaded dynamically into the world system by WorldData when needed
	/// Coordinates specified inside the region are multiplied by the region number to get real world coordinates using 0,0 as the bottom left, so use a range of 0 to 100 for all region files
	/// </summary>
	interface IWorldRegion {
		int RegionX { get; }
		int RegionY { get; }
		void LoadTextures();
		void UnloadTextures();
		HashSet<Objects.WorldObjectBase> LoadObjects();

	}
}
