using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.World.Objects {
	public class WorldObjectInteractable : WorldObjectBase, IInteractable {

		public Action interAction;

		/// <summary>
		/// Complete Declaration for WorldObjectInteractable
		/// </summary>
		/// <param name="x">X position in the world</param>
		/// <param name="y">Y position in the world</param>
		/// <param name="layer">Render Layer</param>
		/// <param name="textureName">Name of the texture</param>
		public WorldObjectInteractable(float x, float y, int layer, string textureName = "default") : base(x, y, layer, textureName) {

		}
		public WorldObjectInteractable(RegionSave.WorldObjectStorage store) : this(store.worldX, store.worldY, store.layer, store.textureName) {
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
