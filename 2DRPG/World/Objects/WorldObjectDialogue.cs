using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.GUI.Interaction;
using Newtonsoft.Json.Linq;
using _2DRPG.Save;

namespace _2DRPG.World.Objects {
	public class WorldObjectDialogue : WorldObjectBase, IInteractable {

		[Editable]
		public string InteractionID = "Dialogue";

		public List<InteractionBase> InterItems = new List<InteractionBase>();

		/// <summary>
		/// Complete Declaration for WorldObjectInteractable
		/// </summary>
		/// <param name="x">X position in the world</param>
		/// <param name="y">Y position in the world</param>
		/// <param name="layer">Render Layer</param>
		/// <param name="textureName">Name of the texture</param>
		public WorldObjectDialogue(float x, float y, int layer, string textureName = "default", float width = 16, float height = 16) : base(x, y, layer, textureName, width, height) {
			
		}
		public WorldObjectDialogue(RegionSave.WorldObjectStorage store) : this(store.worldX, store.worldY, store.layer, store.textureName, store.width, store.height) {
			InteractionID = (string)store.extraData[0];
			List<RegionSave.InteractionObjectStorage> st = ((JArray)store.extraData[1]).ToObject<List<RegionSave.InteractionObjectStorage>>();
			foreach (RegionSave.InteractionObjectStorage s in st)
				InterItems.Add(RegionSave.ConstructInteractionObject(s));
		}

		public void Interact() {
			GUI.Windows.InteractionWindow.SetInteractionElements(InterItems);
			if (!Form1.devWin.IsDisposed) {
				DevWindow.Interaction.interactedObject = this;
				DevWindow.Interaction.SetupTree();
			}
			Screen.OpenWindow("interaction");
		}

		public override RegionSave.WorldObjectStorage StoreObject() {
			List<RegionSave.InteractionObjectStorage> st = new List<RegionSave.InteractionObjectStorage>();
			foreach (InteractionBase b in InterItems) {
				st.Add(b.StoreObject());
			}
			RegionSave.WorldObjectStorage store = base.StoreObject();
			store.objectType = RegionSave.WorldObjectType.Dialogue;
			store.extraData = new object[] { InteractionID, st };
			return store;
		}

		public string GetName() {
			return InteractionID;
		}
	}
}
