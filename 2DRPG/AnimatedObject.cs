using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG {
	class AnimatedObject : TexturedObject, IAnimated {

		public AnimatedObject(float x, float y, int layer, string textureName) : base(x, y, layer, textureName) { }
		public AnimatedObject() : base() { }
		public AnimatedObject(String textureName) : base(textureName) { }

		public void SpriteUpdate() {

		}
	}
}
