using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.Save;

namespace _2DRPG.World.Objects {
	class WorldObjectCollidable : WorldObjectBase, ICollidable{

		/// <summary>
		/// Complete Declaration for WorldObjectCollidable
		/// </summary>
		/// <param name="x">X position in the world</param>
		/// <param name="y">Y position in the world</param>
		/// <param name="layer">Render Layer</param>
		/// <param name="textureName">Name of the texture</param>
		public WorldObjectCollidable(float x, float y, int layer, string textureName = "default", float width = 16, float height = 16) : base(x, y, textureName, width, height) { }

		public WorldObjectCollidable(RegionSave.WorldObjectStorage store) : this(store.worldX, store.worldY, store.layer, store.textureName, store.width, store.height) { }

		public override RegionSave.WorldObjectStorage StoreObject() {
			RegionSave.WorldObjectStorage store = base.StoreObject();
			store.objectType = RegionSave.WorldObjectType.Collidable;
			return store;
		}
	}
}
