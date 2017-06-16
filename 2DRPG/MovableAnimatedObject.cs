using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG {
	class MovableAnimatedObject : AnimatedObject, IMovable {

		public MovableAnimatedObject(float x, float y, int layer, string textureName) : base(x, y, layer, textureName) { }
		public MovableAnimatedObject() : base() { }
		public MovableAnimatedObject(String textureName) : base(textureName) { }

		public void MoveRelative(float x = 0, float y = 0) {
			for (int i = 0; i < 4; i++) {
				arrayPosition[i * 3] += x;
				arrayPosition[i * 3 + 1] += y;
			}
		}
	}
}
