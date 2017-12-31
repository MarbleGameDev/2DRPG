using _2DRPG.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.Save;

namespace _2DRPG.GUI.Interaction {
	class InteractionDialogue : InteractionBase {

		[Editable]
		public string displayText;

		public string DialogueText { get { return displayText; } set { displayText = value; } }

		static UITextBox textBox = new UITextBox(0, -94, .5f, 280, 1, 6, "");

		public InteractionDialogue(string text) {
			displayText = text;
		}
		public InteractionDialogue(RegionSave.InteractionObjectStorage store) {
			displayText = store.text;
		}

		public override void Render() {
			textBox.Render();
		}
		public override void Setup() {
			textBox.Setup();
			textBox.SetText(displayText);
		}
		public override void Takedown() {
			textBox.Cleanup();
		}

		public override RegionSave.InteractionObjectStorage StoreObject() {
			RegionSave.InteractionObjectStorage store = new RegionSave.InteractionObjectStorage() {
				objectType = RegionSave.InteractionObjectType.Dialogue, text = displayText
			};
			return store;
		}

		public override string ToString() {
			return "Dialogue";
		}

		public override void ModificationAction() {
			textBox.SetText(displayText);
		}
	}
}
