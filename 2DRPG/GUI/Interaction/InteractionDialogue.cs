using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI.Interaction {
	class InteractionDialogue : InteractionBase {

		UITextBox textBox = new UITextBox(-100, 0, .5f, 200, 1, 6, "");

		public InteractionDialogue(string text) {
			textBox.SetText(text);
		}
		public InteractionDialogue(GameSave.InteractionObjectStorage store) {
			textBox.SetText(store.text);
		}

		public override void Render() {
			textBox.Render();
		}
		public override void Setup() {

		}

		public override GameSave.InteractionObjectStorage StoreObject() {
			GameSave.InteractionObjectStorage store = new GameSave.InteractionObjectStorage() {
				objectType = GameSave.InteractionObjectType.Dialogue, text = textBox.GetText()
			};
			return store;
		}
	}
}
