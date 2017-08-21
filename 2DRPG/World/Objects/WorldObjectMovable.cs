using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.World.Objects {
	class WorldObjectMovable : WorldObjectBase, IMovable {

		public float MovementSpeed { get; set; }

		/// <summary>
		/// Complete Declaration for WorldObjectMovable
		/// </summary>
		/// <param name="x">X position in world</param>
		/// <param name="y">Y position in world</param>
		/// <param name="textureName">Name of the texture</param>
		public WorldObjectMovable(float x, float y, string textureName, float width = 16, float height = 16) : base(x, y, textureName, width, height) { }

		public WorldObjectMovable() : base() { }
		public WorldObjectMovable(string textureName) : base(textureName) { }

		public WorldObjectMovable(GameSave.WorldObjectStorage store) : this(store.worldX, store.worldY, store.textureName, store.width, store.height) {
			SetLayer(store.layer);
		}

		public virtual void MoveRelative(float x = 0, float y = 0) {
			worldX += x;
			worldY += y;
			ShiftScreenPosition(x, y);
		}

		public float movementQueueX;
		public float movementQueueY;

		public virtual void UpdatePosition() {
			SetWorldPosition(worldX + movementQueueX, worldY + movementQueueY);
			movementQueueX = 0;
			movementQueueY = 0;
		}


		public override GameSave.WorldObjectStorage StoreObject() {
			GameSave.WorldObjectStorage store = new GameSave.WorldObjectStorage() {
				worldX = worldX, worldY = worldY, width = width, height = height, textureName = texName, layer = layer, objectType = GameSave.WorldObjectType.Movable, extraData = new object[] { MovementSpeed}
			};
			return store;
		}
	}
}
