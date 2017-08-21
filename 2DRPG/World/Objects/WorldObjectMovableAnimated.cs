using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.World.Objects {
	class WorldObjectMovableAnimated : WorldObjectAnimated, IMovable {

		public float MovementSpeed { get; set; }


		/// <summary>
		/// Complete Declaration for WorldObjectAnimated
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

		public WorldObjectMovableAnimated(GameSave.WorldObjectStorage store) : this(store.worldX, store.worldY, store.layer, Convert.ToInt32(store.extraData[0]), Convert.ToInt32(store.extraData[1]), Convert.ToInt32(store.extraData[2]), Convert.ToInt32(store.extraData[3]), store.textureName, store.width, store.height) {	}

		public virtual void MoveRelative(float x = 0, float y = 0) {
			worldX += x;
			worldY += y;
			ShiftScreenPosition(x, y);
		}

		public override GameSave.WorldObjectStorage StoreObject() {
			GameSave.WorldObjectStorage store = new GameSave.WorldObjectStorage() {
				worldX = worldX, worldY = worldY, layer = layer, width = width, height = height, textureName = texName, extraData = new object[] { spritesAmount, spriteWidth, spriteHeight, frameInterval}, objectType = GameSave.WorldObjectType.MovableAnimated
			};
			return store;
		}
	}
}
