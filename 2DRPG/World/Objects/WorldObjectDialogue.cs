using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.GUI.Interaction;
using Newtonsoft.Json.Linq;
using _2DRPG.Save;
using System.Runtime.Serialization;

namespace _2DRPG.World.Objects {
	[Serializable]
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
		public WorldObjectDialogue(float x, float y, int layer, Texture textureName, float width = 16, float height = 16) : base(x, y, layer, textureName, width, height) {
			
		}

		[OnSerialized]
		private void LoadInteractions(StreamingContext sc) {
			InterItems = SaveData.LoadInteractions(InteractionID);
		}
		[OnDeserializing]
		private void SaveInteractions(StreamingContext sc) {
			SaveData.SaveInteractions(InterItems, InteractionID);
		}


		public void Interact() {
			GUI.Windows.InteractionWindow.SetInteractionElements(InterItems);
			if (!Form1.devWin.IsDisposed) {
				DevWindow.Interaction.interactedObject = this;
				DevWindow.Interaction.SetupTree();
			}
			Screen.OpenWindow("interaction");
		}

		public string GetName() {
			return InteractionID;
		}
	}
}
