using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.World.Objects {
	public class WorldObjectInteractable : WorldObjectBase, IInteractable {

		public Action interAction;

		public WorldObjectInteractable(float x, float y, string textureName = "default") : base(x, y, textureName) {

		}
		public WorldObjectInteractable(RegionSave.WorldObjectStorage store) : this(store.worldX, store.worldY, store.textureName) {
			SetLayer(store.layer);
		}

		public void Interact() {
			if (interAction != null)
				interAction.Invoke();
		}

		public override RegionSave.WorldObjectStorage StoreObject() {
			RegionSave.WorldObjectStorage store = new RegionSave.WorldObjectStorage() {
				worldX = worldX, worldY = worldY, textureName = texName, layer = layer, objectType = RegionSave.WorldObjectType.Interactable
			};
			return store;
		}
	}
}
