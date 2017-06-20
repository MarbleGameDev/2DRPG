using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.World.Objects {
	class WorldObjectAnimated : WorldObjectBase {

		public WorldObjectAnimated() : base() { }
		public WorldObjectAnimated(string textureName) : base(textureName) { }
		public WorldObjectAnimated(float x, float y, string textureName) : base(textureName) {
			SetWorldPosition(x, y);
		}
		int counter = 0;
		//Put code to change sprites on animation intervals here
		public void SpriteUpdate() {
			if (counter == 19) {
				texturePosition[0] = 0f;
				texturePosition[2] = 0f;
				texturePosition[4] = 1f;
				texturePosition[6] = 1f;
				counter = 0;
			} else {
				texturePosition[0] = texturePosition[0] + .05f;
				texturePosition[2] = texturePosition[2] + .05f;
				texturePosition[4] = texturePosition[4] + .05f;
				texturePosition[6] = texturePosition[6] + .05f;
				counter++;
			}
		}

	}
}
