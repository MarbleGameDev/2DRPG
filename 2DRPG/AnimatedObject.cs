using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG {
	class AnimatedObject : TexturedObject {

        int spritesAmount;
        int spriteWidth;
        int spriteHeight;
        int frameInterval;

		public AnimatedObject(float x, float y, int layer, int spritesAmount, int spriteWidth, int spriteHeight, int frameInterval, string textureName) : base(x, y, layer, textureName) {

            this.spritesAmount = spritesAmount;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;
            this.frameInterval = frameInterval;

        }
		public AnimatedObject() : base() { }
		public AnimatedObject(String textureName) : base(textureName) { }

        int frameCount = 1;
		//All Sprite sheets will loop back to where they start
		public void SpriteUpdate()
        {

            float pixelSize = (1/((float)spriteWidth));

            if (frameCount < spritesAmount * frameInterval)
            {

                if (frameCount % frameInterval == 0)
                {

                    texturePosition[0] = texturePosition[0] + spriteWidth * pixelSize;
                    texturePosition[2] = texturePosition[2] + spriteWidth * pixelSize;
                    texturePosition[4] = texturePosition[4] + spriteWidth * pixelSize;
                    texturePosition[6] = texturePosition[6] + spriteWidth * pixelSize;

                }
                frameCount++;

            }
            else
            {

                texturePosition[0] = 0f;
                texturePosition[2] = 0f;
                texturePosition[4] = spriteWidth * pixelSize;
                texturePosition[6] = spriteWidth * pixelSize;

            }

		}
	}
}
