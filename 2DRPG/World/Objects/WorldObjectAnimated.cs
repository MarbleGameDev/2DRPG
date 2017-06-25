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

        public WorldObjectAnimated(float x, float y, int layer, int spritesAmount, int spriteWidth, int spriteHeight, int frameInterval, string textureName) : base(textureName) {

            this.spritesAmount = spritesAmount;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;
            this.frameInterval = frameInterval;

        }
        public WorldObjectAnimated() : base() { }
		public WorldObjectAnimated(string textureName) : base(textureName) { }
		public WorldObjectAnimated(float x, float y, string textureName) : base(textureName) {
			SetWorldPosition(x, y);
		}

        
		int counter = 0;
        int frameCount = 1;
        public void SpriteUpdate()
        {

            if (frameCount == 1)
            {

                texturePosition[0] = 0;
                texturePosition[2] = 0;
                texturePosition[4] = ((spritesAmount > 10) ? .1f : (1f / spritesAmount));
                texturePosition[6] = ((spritesAmount > 10) ? .1f : (1f / spritesAmount));

            }

            if (frameCount < spritesAmount * frameInterval)
            {

                if (frameCount % frameInterval == 0)
                {

                    texturePosition[0] = texturePosition[0] + ((spritesAmount > 10) ? .1f : (1f / spritesAmount));
                    texturePosition[2] = texturePosition[2] + ((spritesAmount > 10) ? .1f : (1f / spritesAmount));
                    texturePosition[4] = texturePosition[4] + ((spritesAmount > 10) ? .1f : (1f / spritesAmount));
                    texturePosition[6] = texturePosition[6] + ((spritesAmount > 10) ? .1f : (1f / spritesAmount));

                }
                frameCount++;

            }
            else
            {

                frameCount = 1;

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
