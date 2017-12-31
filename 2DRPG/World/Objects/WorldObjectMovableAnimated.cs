using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.Save;

namespace _2DRPG.World.Objects {
	class WorldObjectMovableAnimated : WorldObjectAnimated, IMovable {

		public float MovementSpeed { get; set; }
		public float movementQueueX;
		public float movementQueueY;

		/// <summary>
		/// Complete Declaration for WorldObjectMovableAnimated
		/// Not used anywhere in the game yet
		/// </summary>
		/// <param name="x">X position in the world</param>
		/// <param name="y">Y position in the world</param>
		/// <param name="layer">Render Layer</param>
		/// <param name="spritesAmt">Number of sprites on the spriteSheet</param>
		/// <param name="spriteWth">Width of each sprite</param>
		/// <param name="spriteHt">Height of each sprite</param>
		/// <param name="frameInt">Number of render calls between frame changes</param>
		/// <param name="textureName">Name of the texture</param>
		public WorldObjectMovableAnimated(float x, float y, int layer, int spritesAmt, int spriteWth, int spriteHt, int frameInt, string textureName, float width = 16, float height = 16) : base(x, y, layer, spritesAmt, spriteWth, spriteHt, frameInt, textureName, width, height) { }

		public WorldObjectMovableAnimated(RegionSave.WorldObjectStorage store) : base(store) {	}

		public virtual void MoveRelative(float x = 0, float y = 0) {
			worldX += x;
			worldY += y;
			ShiftScreenPosition(x, y);
		}

		public override RegionSave.WorldObjectStorage StoreObject() {
			RegionSave.WorldObjectStorage store = base.StoreObject();
			store.objectType = RegionSave.WorldObjectType.MovableAnimated;
			return store;
		}
	}
}
