using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG {
	class MovableObject : TexturedObject, IMovable {

		public MovableObject(float x, float y, int layer, string textureName) : base(x, y, layer, textureName) { }
		public MovableObject() : base() { }
		public MovableObject(String textureName) : base(textureName) { }

		/// <summary>
		/// Shifts the quad by the values given in x,y,z
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public void MoveRelative(float x = 0, float y = 0) {
			for (int i = 0; i < 4; i++) {
				arrayPosition[i * 3] += x;
				arrayPosition[i * 3 + 1] += y;
			}
		}

	}
}
