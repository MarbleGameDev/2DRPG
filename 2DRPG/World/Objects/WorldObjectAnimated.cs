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

		//Put code to change sprites on animation intervals here
		public void SpriteUpdate() {

		}

	}
}
