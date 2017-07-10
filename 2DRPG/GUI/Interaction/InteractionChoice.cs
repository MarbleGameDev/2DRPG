using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI.Interaction {
	class InteractionChoice : InteractionBase {

		Dictionary<UIButton, List<InteractionBase>> items = new Dictionary<UIButton, List<InteractionBase>>();

		public InteractionChoice(Dictionary<string, List<InteractionBase>> choices) {
			int counter = 0;
			foreach (KeyValuePair<string, List<InteractionBase>> k in choices) {
				items.Add(new UIButton(0, 50 - counter * 20, 30, 10, 2, "button") { displayLabel = new UIText(0, 50 - counter * 20, .5f, 1, k.Key)}, k.Value);
				counter++;
			}
		}

		public bool CheckClick(float x, float y) {
			foreach (UIButton b in items.Keys)
				if (b.CheckClick(x, y))
					return true;
			return false;
		}

		public override void Render() {
			foreach (UIButton b in items.Keys)
				b.Render();
		}
		public override void Setup() {
			foreach (UIButton b in items.Keys) {
				System.Diagnostics.Debug.WriteLine(b);
				b.SetButtonAction(() => {
					System.Diagnostics.Debug.WriteLine(b.displayLabel.GetText());
					Windows.InteractionWindow.InsertNodes(items[b]);
					nextNode.Invoke();
				});
			}
		}
	}
}
