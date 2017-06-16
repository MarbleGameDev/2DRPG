using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG {
	class ControllableObject : MovableAnimatedObject {

		public ControllableObject(float x, float y, int layer, string textureName) : base(x, y, layer, textureName) {
			Input.InputCall += MoveOnKey;
		}
		public ControllableObject() : base() {
			Input.InputCall += MoveOnKey;
		}
		public ControllableObject(String textureName) : base(textureName) {
			Input.InputCall += MoveOnKey;
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
