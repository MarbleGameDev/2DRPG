using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.World.Objects {
	class WorldObjectMovable : WorldObjectBase, IMovable {

		public float MovementSpeed { get; set; }
		public float VelocityX { get; set; }
		public float VelocityY { get; set; }

		public WorldObjectMovable() : base() { }
		public WorldObjectMovable(string textureName) : base(textureName) { }

		/// <summary>
		/// Complete Declaration for WorldObjectMovable
		/// </summary>
		/// <param name="x">X position in world</param>
		/// <param name="y">Y position in world</param>
		/// <param name="textureName">Name of the texture</param>
		public WorldObjectMovable(float x, float y, string textureName) : base(x, y, textureName) {	}

		public WorldObjectMovable(RegionSave.WorldObjectStorage store) : this(store.worldX, store.worldY, store.textureName) {
			SetLayer(store.layer);
		}

		public virtual void MoveRelative(float x = 0, float y = 0) {
			worldX += x;
			worldY += y;
			ShiftScreenPosition(x, y);
		}

		public override RegionSave.WorldObjectStorage StoreObject() {
			RegionSave.WorldObjectStorage store = new RegionSave.WorldObjectStorage() {
				worldX = worldX, worldY = worldY, textureName = texName, layer = layer, objectType = RegionSave.WorldObjectType.Movable
			};
			return store;
		}
	}
}
