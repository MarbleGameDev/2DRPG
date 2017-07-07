using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.World.Objects {
	class WorldObjectCollidable : WorldObjectBase, ICollidable{
		public WorldObjectCollidable(float x, float y, string textureName = "default") : base(x, y, textureName) {

		}
		public WorldObjectCollidable(RegionSave.WorldObjectStorage store) : this(store.worldX, store.worldY, store.textureName) {
			SetLayer(store.layer);
		}

		public override RegionSave.WorldObjectStorage StoreObject() {
			RegionSave.WorldObjectStorage store = new RegionSave.WorldObjectStorage() {
				worldX = worldX, worldY = worldY, layer = layer, textureName = texName, objectType = RegionSave.WorldObjectType.Collidable
			};
			return store;
		}
	}
}
