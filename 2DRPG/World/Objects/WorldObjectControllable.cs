using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG {
	class WorldObjectControllable : World.Objects.WorldObjectMovableAnimated {
		
		public WorldObjectControllable(float x, float y, int layer, string textureName) : base(x, y, textureName) {
			Input.InputCall += MoveOnKeys;
			MovementSpeed = .01f;
		}
		public WorldObjectControllable(String textureName) : base(textureName) {
			Input.InputCall += MoveOnKeys;
			MovementSpeed = .01f;
		}

		private void MoveOnKeys(Input.KeyInputs[] k) {
			if (k.Contains(Input.KeyInputs.left))
				movementX = -MovementSpeed;
			if (k.Contains(Input.KeyInputs.right))
				movementX = MovementSpeed;
			if (k.Contains(Input.KeyInputs.up))
				movementY = MovementSpeed;
			if (k.Contains(Input.KeyInputs.down))
				movementY = -MovementSpeed;
		}
		float movementX;
		float movementY;

		public override void MoveRelative(float x = 0, float y = 0) {
			WorldData.MoveCenter(movementX, movementY);
			SetWorldPosition(worldX + movementX, worldY + movementY);
			movementX = 0;
			movementY = 0;
		}

	}
}
