using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.Save;

namespace _2DRPG.GUI.Interaction {
	class InteractionChoice : InteractionBase {

		protected static List<UIButton> items = new List<UIButton>();
		protected Dictionary<string, int> pathNames = new Dictionary<string, int>();
		public List<InteractionPath> paths = new List<InteractionPath>();
		/*
		 *TODO: Use List of InteractionPaths to hold data instead of current choices solution
		 * 
		 */

		public InteractionChoice(List<InteractionPath> choices) {
			PostConst(choices);
		}

		private void PostConst(List<InteractionPath> choices) {
			int counter = paths.Count;
			foreach (InteractionPath b in choices) {
				paths.Add(b);
				pathNames.Add(b.pathName, counter);
				
			}
		}

		public InteractionChoice(RegionSave.InteractionObjectStorage store) {
			List<InteractionPath> pa = new List<InteractionPath>();
			foreach (RegionSave.InteractionObjectStorage s in store.subObjects) {
				if (s.objectType == RegionSave.InteractionObjectType.Path)
					pa.Add((InteractionPath)RegionSave.ConstructInteractionObject(s));
			}
			PostConst(pa);
		}

		public void AddBase(string choice, InteractionBase b) {
			if (pathNames.ContainsKey(choice))
				paths[pathNames[choice]].items.Add(b);
		}

		public void AddChoice(string choice) {
			if (!pathNames.ContainsKey(choice))
				PostConst(new List<InteractionPath>() { new InteractionPath() { pathName = choice, items = new List<InteractionBase>() } });
		}

		public bool CheckClick(float x, float y) {
			foreach (UIButton b in items)
				if (b.CheckClick(x, y))
					return true;
			return false;
		}

		public override void Render() {
			foreach (UIButton b in items)
				b.Render();
		}
		public override void Setup() {
			items.Clear();
			int counter = 0;
			for (int i = 0; i < paths.Count; i++) {
				InteractionPath p = paths[i];
				items.Add(new UIButton(0, -100 - counter * 21, 30, 10, 2, "textBox") {
					displayLabel = new UIText(0, -98 - counter++ * 21, .5f, 1, p.pathName),
					buttonAction = () => {
						Windows.InteractionWindow.InsertNodes(p.items);
						nextNode.Invoke();
					}
				});
			}
		}
		public override void Takedown() {
		}

		public override RegionSave.InteractionObjectStorage StoreObject() {
			List<RegionSave.InteractionObjectStorage> subs = new List<RegionSave.InteractionObjectStorage>();
			foreach (InteractionBase b in paths) {
				subs.Add(b.StoreObject());
			}
			RegionSave.InteractionObjectStorage store = new RegionSave.InteractionObjectStorage() {
				subObjects = subs, objectType = RegionSave.InteractionObjectType.Choice
			};
			return store;
		}

		public override string ToString() {
			return "Choice";
		}
	}
}
