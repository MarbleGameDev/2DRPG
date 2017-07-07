using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.World.Objects;

namespace _2DRPG.World.Regions {
	class Region_1x0 : IWorldRegion {
		public int RegionX { get { return -1; } }
		public int RegionY { get { return 0; } }

		public HashSet<WorldObjectBase> regionObjects = new HashSet<WorldObjectBase>() {
			new WorldObjectBase(900, 0, "default"),
			new WorldObjectBase(900, 16f, "default")
		};

		public ref HashSet<WorldObjectBase> LoadObjects() {

			return ref regionObjects;
		}

		string[] textureNames = new string[] {
			"default", "josh"
		};

		public void LoadTextures() {
			TextureManager.RegisterTextures(textureNames);
		}

		public void UnloadTextures() {
			TextureManager.UnRegisterTextures(textureNames);
		}
	}
}
