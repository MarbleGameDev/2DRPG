using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI.Interaction {
	class InteractionChoice : InteractionBase {

		Dictionary<UIButton, List<InteractionBase>> items = new Dictionary<UIButton, List<InteractionBase>>();
		Dictionary<string, List<InteractionBase>> choices = new Dictionary<string, List<InteractionBase>>();

		public InteractionChoice(Dictionary<string, List<InteractionBase>> choices) {
			this.choices = choices;
			int counter = 0;
			foreach (KeyValuePair<string, List<InteractionBase>> k in choices) {
				items.Add(new UIButton(0, 50 - counter * 20, 30, 10, 2, "button") { displayLabel = new UIText(0, 50 - counter * 20, .5f, 1, k.Key)}, k.Value);
				counter++;
			}
		}

		public InteractionChoice(GameSave.InteractionObjectStorage store) {
			for (int i = 0; i < store.extraData.Length; i++) {
				List<InteractionBase> d = new List<InteractionBase>();
				foreach (GameSave.InteractionObjectStorage s in store.subObjects[i])
					d.Add(GameSave.ConstructInteractionObject(s));
				choices.Add((string)store.extraData[i], d);
			}
			int counter = 0;
			foreach (KeyValuePair<string, List<InteractionBase>> k in choices) {
				items.Add(new UIButton(0, 50 - counter * 20, 30, 10, 2, "button") { displayLabel = new UIText(0, 50 - counter * 20, .5f, 1, k.Key) }, k.Value);
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
				b.SetButtonAction(() => {
					Windows.InteractionWindow.InsertNodes(items[b]);
					nextNode.Invoke();
				});
			}
		}

		public override GameSave.InteractionObjectStorage StoreObject() {
			List<List<GameSave.InteractionObjectStorage>> subs = new List<List<GameSave.InteractionObjectStorage>>();
			foreach (List<InteractionBase> b in choices.Values) {
				List<GameSave.InteractionObjectStorage> s = new List<GameSave.InteractionObjectStorage>();
				foreach(InteractionBase ba in b)
					s.Add(ba.StoreObject());
				subs.Add(s);
			}
			GameSave.InteractionObjectStorage store = new GameSave.InteractionObjectStorage() {
				extraData = choices.Keys.ToArray(), subObjects = subs.ToArray(), objectType = GameSave.InteractionObjectType.Choice
			};
			return store;
		}
	}
}
