using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.World.Objects {
	class WorldObjectAnimated : WorldObjectBase {

        int spritesAmount;
        int spriteWidth;
        int spriteHeight;
        int frameInterval;

        public WorldObjectAnimated(float x, float y, int layer, int spritesAmount, int spriteWidth, int spriteHeight, int frameInterval, string textureName) : this(x, y, textureName) {

            this.spritesAmount = spritesAmount;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;
            this.frameInterval = frameInterval;
			SetLayer(layer);
			SetWorldPosition(x, y);
        }
        public WorldObjectAnimated() : base() { }
		public WorldObjectAnimated(string textureName) : base(textureName) { }
		public WorldObjectAnimated(float x, float y, string textureName) : base(textureName) {
			SetWorldPosition(x, y);
		}

        
		int counter = 0;
        int frameCount = 0;
        public void SpriteUpdate()
        {

            float sheetShiftHorizontal = ((spritesAmount > 10) ? .1f : (1f / spritesAmount));
            float sheetShiftVertical = 1f/((spritesAmount / 10) + 1);

            if (frameCount == 0)
            {

                texturePosition[0] = 0;
                texturePosition[1] = 0;
                texturePosition[2] = 0;
                texturePosition[3] = sheetShiftVertical;
                texturePosition[4] = sheetShiftHorizontal;
                texturePosition[5] = sheetShiftVertical;
                texturePosition[6] = sheetShiftHorizontal;
                texturePosition[7] = 0;

            }

            if (frameCount < spritesAmount * frameInterval)
            {

                if (frameCount % frameInterval == 0)
                {

                    texturePosition[0] += sheetShiftHorizontal;
                    texturePosition[2] += sheetShiftHorizontal;
                    texturePosition[4] += sheetShiftHorizontal;
                    texturePosition[6] += sheetShiftHorizontal;

                    if (/*spritesAmount > 10 &&*/ (frameCount / frameInterval) % 10 == 0)
                    {

                        texturePosition[1] += sheetShiftVertical;
                        texturePosition[3] += sheetShiftVertical;
                        texturePosition[5] += sheetShiftVertical;
                        texturePosition[7] += sheetShiftVertical;

                    }

                }

                frameCount++;

            }
            else
            {

                frameCount = 0;

            }




        }

        /*public void SpriteUpdate(int frameInterval)
        {

            if ((frameInterval == 0) ? (true) : counter % frameInterval == 0) {

                if (counter == 15) {
                    texturePosition[0] = 0f;
                    texturePosition[2] = 0f;
                    texturePosition[4] = 1f;
                    texturePosition[6] = 1f;
                    counter = 0;
                }
                else {
                    texturePosition[0] = texturePosition[0] + 0.0625f;
                    texturePosition[2] = texturePosition[2] + 0.0625f;
                    texturePosition[4] = texturePosition[4] + 0.0625f;
                    texturePosition[6] = texturePosition[6] + 0.0625f
                        ;
                    counter++;
                }

            } else
                counter++;

        }*/

	}
}
