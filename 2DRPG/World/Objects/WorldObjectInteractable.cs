using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.GUI.Interaction;

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
			interAction = OpenDialogue;
		}
		public WorldObjectInteractable(RegionSave.WorldObjectStorage store) : this(store.worldX, store.worldY, store.layer, store.textureName) {
			interAction = OpenDialogue;
		}

		void OpenDialogue() {
			if (Screen.currentWindows.ContainsKey("interaction")) {
				Screen.CloseWindow("interaction");
				return;
			}
			List<InteractionBase> items = new List<InteractionBase>(){
			new InteractionDialogue("Plenty 'O Peanuts, Eh?"),
			new InteractionDialogue("So, what can I help you with?"),
			new InteractionChoice(new Dictionary<string, List<InteractionBase>>{
				{ "choice A", new List<InteractionBase> {
					new InteractionDialogue("Blah Blah Blah"),
					new InteractionDialogue("Ho Hum, fiddly dee")} }, 
				{ "choice B", new List<InteractionBase>{
					new InteractionDialogue("Interesting, but I'm afraid I can't help you"),
					new InteractionDialogue("What you want is a fresh start on life")
				} }
			})
			};
			GUI.Windows.InteractionWindow.SetInteractionElements(items);
			Screen.OpenWindow("interaction");
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
