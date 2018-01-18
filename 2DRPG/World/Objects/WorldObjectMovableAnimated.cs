using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.Save;

namespace _2DRPG.World.Objects {
	[Serializable]
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
		public WorldObjectMovableAnimated(float x, float y, int layer, int spritesAmt, int spriteWth, int spriteHt, int frameInt, Texture textureName, float width = 16, float height = 16) : base(x, y, layer, spritesAmt, spriteWth, spriteHt, frameInt, textureName, width, height) { }

		public virtual void MoveRelative(float x = 0, float y = 0) {
			worldX += x;
			worldY += y;
			ShiftScreenPosition(x, y);
		}

		public void QueueMovement(float x, float y) {
			movementQueueX = x;
			movementQueueY = y;
		}

		/// <summary>
		/// Method called every logic tick to update the position changes queued
		/// </summary>
		public virtual void UpdatePosition() {
			if (movementQueueX != 0 || movementQueueY != 0)
				MoveRelative(movementQueueX, movementQueueY);
			movementQueueX = 0;
			movementQueueY = 0;
		}
	}
}
