using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.Save;

namespace _2DRPG.World.Objects {
	class WorldObjectControllable : WorldObjectMovable {


		/// <summary>
		/// Not Correctly Implemented Yet
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="layer"></param>
		/// <param name="textureName"></param>
		public WorldObjectControllable(float x, float y, int layer, string textureName, float width = 16, float height = 16) : base(x, y, textureName, width, height) {
			Input.InputCall += MoveOnKeys;
			MovementSpeed = 2f;
			SetLayer(layer);
		}
		public WorldObjectControllable(String textureName) : base(textureName) {
			Input.InputCall += MoveOnKeys;
			MovementSpeed = 2f;
		}
		public WorldObjectControllable(RegionSave.WorldObjectStorage store) : this(store.worldX, store.worldY, store.layer, store.textureName, store.width, store.height) {
			SetUID(store.uid);
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

		public override RegionSave.WorldObjectStorage StoreObject() {
			RegionSave.WorldObjectStorage store = base.StoreObject();
			store.objectType = RegionSave.WorldObjectType.Controllable;
			return store;
		}

	}
}
