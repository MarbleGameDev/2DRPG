using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.World.Objects {
	class WorldObjectAnimated : WorldObjectBase {

        int spritesAmount = 1;
        int spriteWidth = 16;
        int spriteHeight = 16;
        int frameInterval = 10;
		float sheetShiftHorizontal = 1f;
		float sheetShiftVertical = 1f;

        public WorldObjectAnimated(float x, float y, int layer, int spritesAmt, int spriteWth, int spriteHt, int frameInt, string textureName) : this(x, y, textureName) {
            spritesAmount = spritesAmt;
            spriteWidth = spriteWth;
            spriteHeight = spriteHt;
            frameInterval = frameInt;
			sheetShiftHorizontal = ((spritesAmount > 10) ? .1f : (1f / spritesAmount));
			sheetShiftVertical = 1f / ((spritesAmount / 10) + 1);
			SetLayer(layer);
        }
		public WorldObjectAnimated(string textureName) : base(textureName) {
			sheetShiftHorizontal = ((spritesAmount > 10) ? .1f : (1f / spritesAmount));
			sheetShiftVertical = 1f / ((spritesAmount / 10) + 1);
			SetLayer(layer);
		}
		public WorldObjectAnimated(float x, float y, string textureName) : this(textureName) {
			SetWorldPosition(x, y);
		}

        int frameCount = 0;
        public void SpriteUpdate() {
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
	}
}
