using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.World.Entities {
	class WorldEntityAnimated : Objects.WorldObjectAnimated, Objects.IMovable {
		public float MovementSpeed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		/// <summary>
		/// Complete Declaration for WorldEntityAnimated
		/// </summary>
		/// <param name="x">X position in the world</param>
		/// <param name="y">Y position in the world</param>
		/// <param name="layer">Render Layer</param>
		/// <param name="spritesAmt">Number of sprites on the spriteSheet</param>
		/// <param name="spriteWth">Width of each sprite</param>
		/// <param name="spriteHt">Height of each sprite</param>
		/// <param name="frameInt">Number of render calls between frame changes</param>
		/// <param name="textureName">Name of the texture</param>
		public WorldEntityAnimated(float x, float y, int layer, int spritesAmt, int spriteWth, int spriteHt, int frameInt, Texture textureName, float width = 16, float height = 16) : base(x, y, layer, spritesAmt, spriteWth, spriteHt, frameInt, textureName, width, height) {
			
		}

		public void MoveRelative(float x = 0, float y = 0) {
			throw new NotImplementedException();
		}
	}
}
