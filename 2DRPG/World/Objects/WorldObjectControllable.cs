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
		public WorldObjectControllable() : base() {
			Input.InputCall += MoveOnKeys;
			MovementSpeed = .01f;
		}
		public WorldObjectControllable(String textureName) : base(textureName) {
			Input.InputCall += MoveOnKeys;
			MovementSpeed = .01f;
		}

		private void MoveOnKeys(Input.KeyInputs[] k) {
			if (k.Contains(Input.KeyInputs.left))
				MoveRelative(-MovementSpeed, 0);
			if (k.Contains(Input.KeyInputs.right))
				MoveRelative(MovementSpeed, 0);
			if (k.Contains(Input.KeyInputs.up))
				MoveRelative(0f, MovementSpeed);
			if (k.Contains(Input.KeyInputs.down))
				MoveRelative(0f, -MovementSpeed);
		}

		public override void MoveRelative(float x = 0, float y = 0) {
			WorldData.MoveCenter(x, y);
			SetScreenPosition(WorldData.CurrentX - worldX, WorldData.CurrentY - worldY);
		}

	}
}
