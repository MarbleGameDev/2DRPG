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

		public override void Render() {
			textBox.Render();
		}
		public override void Setup() {

		}
	}
}
