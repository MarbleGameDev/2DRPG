using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.Save;

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

		public WorldObjectMovable(RegionSave.WorldObjectStorage store) : this(store.worldX, store.worldY, store.textureName, store.width, store.height) {
			SetLayer(store.layer);
			SetUID(store.uid);
		}

		/// <summary>
		/// Shifts the position of the object by the values passed
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public virtual void MoveRelative(float x = 0, float y = 0) {
			ShiftPosition(quadPosition, x, y);
			worldX += x;
			worldY += y;
			SendPacket();
		}

		public float movementQueueX;
		public float movementQueueY;

		/// <summary>
		/// Method called every logic tick to update the position changes queued
		/// </summary>
		public virtual void UpdatePosition() {
			if (movementQueueX != 0 || movementQueueY != 0)
				MoveRelative(movementQueueX, movementQueueY);
			movementQueueX = 0;
			movementQueueY = 0;
		}


		public override RegionSave.WorldObjectStorage StoreObject() {
			RegionSave.WorldObjectStorage store = base.StoreObject();
			store.objectType = RegionSave.WorldObjectType.Movable;
			store.extraData = new object[] { MovementSpeed };
			return store;
		}
	}
}
