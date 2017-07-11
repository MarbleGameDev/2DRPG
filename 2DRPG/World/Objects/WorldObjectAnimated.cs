using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace _2DRPG.World.Objects {
	class WorldObjectAnimated : WorldObjectBase {
		[Editable]
        public int spritesAmount = 1;
		[Editable]
        public int spriteWidth = 16;
		[Editable]
        public int spriteHeight = 16;
		[Editable]
        public int frameInterval = 10;
		float sheetShiftHorizontal = 1f;
		float sheetShiftVertical = 1f;

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
        public WorldObjectAnimated(float x, float y, int layer, int spritesAmt, int spriteWth, int spriteHt, int frameInt, string textureName) : base(x, y, textureName) {
            spritesAmount = spritesAmt;
            spriteWidth = spriteWth;
            spriteHeight = spriteHt;
            frameInterval = frameInt;
			sheetShiftHorizontal = ((spritesAmount > 10) ? .1f : (1f / spritesAmount));
			sheetShiftVertical = 1f / ((spritesAmount / 10) + 1);
			SetLayer(layer);
        }

		public WorldObjectAnimated(GameSave.WorldObjectStorage store) : this(store.worldX, store.worldY, store.layer, Convert.ToInt32(store.extraData[0]), Convert.ToInt32(store.extraData[1]), Convert.ToInt32(store.extraData[2]), Convert.ToInt32(store.extraData[3]), store.textureName) { }

        int frameCount = 0;
        public void SpriteUpdate() {
			if (frameInterval == 0)
				return;
			if (frameCount % frameInterval == 0) {
				int frameNum = frameCount / frameInterval;
				texturePosition[0] = 0 + sheetShiftHorizontal * (frameNum % 10);
				texturePosition[2] = 0 + sheetShiftHorizontal * (frameNum % 10);
				texturePosition[4] = sheetShiftHorizontal + sheetShiftHorizontal * (frameNum % 10);
				texturePosition[6] = sheetShiftHorizontal + sheetShiftHorizontal * (frameNum % 10);
				texturePosition[1] = 0 + sheetShiftVertical * (frameNum / 10);
				texturePosition[3] = sheetShiftVertical + sheetShiftVertical * (frameNum / 10);
				texturePosition[5] = sheetShiftVertical + sheetShiftVertical * (frameNum / 10);
				texturePosition[7] = 0 + sheetShiftVertical * (frameNum / 10);
			}
			frameCount++;
			if (frameCount / frameInterval == spritesAmount)
				frameCount = 0;
			return;
        }

		public override GameSave.WorldObjectStorage StoreObject() {
			GameSave.WorldObjectStorage store = new GameSave.WorldObjectStorage() {
				worldX = worldX, worldY = worldY, layer = layer, textureName = texName, extraData = new object[] { spritesAmount, spriteWidth, spriteHeight, frameInterval}, objectType = GameSave.WorldObjectType.Animated
			};
			return store;
		}

		public override void ModificationAction() {
			base.ModificationAction();
			sheetShiftHorizontal = ((spritesAmount > 10) ? .1f : (1f / spritesAmount));
			sheetShiftVertical = 1f / ((spritesAmount / 10) + 1);
		}
	}
}
