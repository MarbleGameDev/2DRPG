using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.GUI.Interaction;
using Newtonsoft.Json.Linq;

namespace _2DRPG.World.Objects {
	public class WorldObjectInteractable : WorldObjectBase, IInteractable {

		public string InteractionTag = "";

		public List<InteractionBase> InterItems = new List<InteractionBase>();

		/// <summary>
		/// Complete Declaration for WorldObjectInteractable
		/// </summary>
		/// <param name="x">X position in the world</param>
		/// <param name="y">Y position in the world</param>
		/// <param name="layer">Render Layer</param>
		/// <param name="textureName">Name of the texture</param>
		public WorldObjectInteractable(float x, float y, int layer, string textureName = "default", float width = 16, float height = 16) : base(x, y, layer, textureName, width, height) {

		}
		public WorldObjectInteractable(GameSave.WorldObjectStorage store) : this(store.worldX, store.worldY, store.layer, store.textureName, store.width, store.height) {
			InteractionTag = (string)store.extraData[0];
			List<GameSave.InteractionObjectStorage> st = ((JArray)store.extraData[1]).ToObject<List<GameSave.InteractionObjectStorage>>();
			foreach (GameSave.InteractionObjectStorage s in st)
				InterItems.Add(GameSave.ConstructInteractionObject(s));
		}

		void OpenDialogue() {
			GUI.Windows.InteractionWindow.SetInteractionElements(InterItems);
			if (!Form1.devWin.IsDisposed) {
				DevWindow.Interaction.interactedObject = this;
				DevWindow.Interaction.SetupTree();
			}
			Screen.OpenWindow("interaction");
		}

		public void Interact() {
			OpenDialogue();
		}

		public override GameSave.WorldObjectStorage StoreObject() {
			List<GameSave.InteractionObjectStorage> st = new List<GameSave.InteractionObjectStorage>();
			foreach (InteractionBase b in InterItems) {
				st.Add(b.StoreObject());
			}
			GameSave.WorldObjectStorage store = new GameSave.WorldObjectStorage() {
				worldX = worldX, worldY = worldY, width = width, height = height, textureName = texName, layer = layer, objectType = GameSave.WorldObjectType.Interactable, extraData = new object[] { InteractionTag, st}
			};
			return store;
		}
	}
}
