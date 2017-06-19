using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG {
	class WorldObjectControllable : World.Objects.WorldObjectMovableAnimated {
		
		public WorldObjectControllable(float x, float y, int layer, string textureName) : base(x, y, textureName) {
			Input.InputCall += MoveOnKey;
			MovementSpeed = .03125f;
		}
		public WorldObjectControllable() : base() {
			Input.InputCall += MoveOnKey;
			MovementSpeed = .03125f;
		}
		public WorldObjectControllable(String textureName) : base(textureName) {
			Input.InputCall += MoveOnKey;
			MovementSpeed = .03125f;
		}

		private void MoveOnKey(Input.KeyInputs k) {
			if (k.Equals(Input.KeyInputs.left))
				MoveRelative(-MovementSpeed, 0);
			else if (k.Equals(Input.KeyInputs.right))
				MoveRelative(MovementSpeed, 0);
			else if (k.Equals(Input.KeyInputs.up))
				MoveRelative(0f, MovementSpeed);
			else if (k.Equals(Input.KeyInputs.down))
				MoveRelative(0f, -MovementSpeed);
		}

		public override void MoveRelative(float x = 0, float y = 0) {
			worldX += x;
			worldY += y;
			WorldData.MoveCenter(x, y);
		}

	}
}
