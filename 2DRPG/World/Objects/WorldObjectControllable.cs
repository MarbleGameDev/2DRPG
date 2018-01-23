using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.Save;

namespace _2DRPG.World.Objects {
	[Serializable]
	class WorldObjectControllable : WorldObjectMovable {


		/// <summary>
		/// Not Correctly Implemented Yet
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="layer"></param>
		/// <param name="textureName"></param>
		public WorldObjectControllable(float x, float y, int layer, Texture textureName, float width = 16, float height = 16) : base(x, y, layer, textureName, width, height) {
			Input.InputCall += MoveOnKeys;
			MovementSpeed = 2f;
		}
		public WorldObjectControllable(Texture textureName) : base(textureName) {
			Input.InputCall += MoveOnKeys;
			MovementSpeed = 2f;
		}

		private void MoveOnKeys(Input.KeyInputs[] k) {
			if (k.Contains(Input.KeyInputs.left))
				movementQueueX = -MovementSpeed;
			if (k.Contains(Input.KeyInputs.right))
				movementQueueX = MovementSpeed;
			if (k.Contains(Input.KeyInputs.up))
				movementQueueY = MovementSpeed;
			if (k.Contains(Input.KeyInputs.down))
				movementQueueY = -MovementSpeed;
		}

	}
}
