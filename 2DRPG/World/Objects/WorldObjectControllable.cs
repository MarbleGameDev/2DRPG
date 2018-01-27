using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.Save;
using System.Runtime.Serialization;

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
		public WorldObjectControllable(float x, float y, int layer, Texture textureName, float width1 = 16, float height1 = 16, float width2 = 16, float height2 = 16, float width3 = 16, float height3 = 16, float width4 = 16, float height4 = 16) : base(x, y, layer, textureName, width1, height1, width2, height2, width3, height3, width4, height4) {
			Input.InputCall += MoveOnKeys;
			MovementSpeed = 2f;
		}
		public WorldObjectControllable(Texture textureName) : base(textureName) {
			Input.InputCall += MoveOnKeys;
			MovementSpeed = 2f;
		}

		[OnDeserialized]
		private void SetupMovement(StreamingContext sc) {
			Input.InputCall += MoveOnKeys;
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
