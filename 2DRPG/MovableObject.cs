using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG {
	class MovableObject : TexturedObject {

		public MovableObject(float x, float y, int layer, string textureName) : base(x, y, layer, textureName) {
			Input.InputCall += MoveOnKey;
		}
		public MovableObject() : base() {
			Input.InputCall += MoveOnKey;
		}
		public MovableObject(String textureName) : base(textureName) {
			Input.InputCall += MoveOnKey;
		}

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

		private void MoveOnKey(Input.KeyInputs k) {
			if (k.Equals(Input.KeyInputs.left))
				MoveRelative(-.03125f, 0);
			else if (k.Equals(Input.KeyInputs.right))
				MoveRelative(.03125f, 0);
			else if (k.Equals(Input.KeyInputs.up))
				MoveRelative(0f, .03125f);
			else if (k.Equals(Input.KeyInputs.down))
				MoveRelative(0f, -.03125f);
		}
	}
}
