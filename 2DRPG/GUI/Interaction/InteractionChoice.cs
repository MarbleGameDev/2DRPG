﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI.Interaction {
	class InteractionChoice : InteractionBase {

		protected Dictionary<UIButton, int> items = new Dictionary<UIButton, int>();
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

		public InteractionChoice(GameSave.InteractionObjectStorage store) {
			List<InteractionPath> pa = new List<InteractionPath>();
			foreach (GameSave.InteractionObjectStorage s in store.subObjects) {
				if (s.objectType == GameSave.InteractionObjectType.Path)
					pa.Add((InteractionPath)GameSave.ConstructInteractionObject(s));
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
			int counter = 0;
			for (int i = 0; i < paths.Count; i++) {
				InteractionPath p = paths[i];
				items.Add(new UIButton(0, 50 - counter * 20, 30, 10, 2, "button") { displayLabel = new UIText(0, 50 - counter * 20, .5f, 1, p.pathName), buttonAction = () => {
					Windows.InteractionWindow.InsertNodes(p.items);
					nextNode.Invoke();
				} }, counter++);
			}
		}

		public override GameSave.InteractionObjectStorage StoreObject() {
			List<GameSave.InteractionObjectStorage> subs = new List<GameSave.InteractionObjectStorage>();
			foreach (InteractionBase b in paths) {
				subs.Add(b.StoreObject());
			}
			GameSave.InteractionObjectStorage store = new GameSave.InteractionObjectStorage() {
				subObjects = subs, objectType = GameSave.InteractionObjectType.Choice
			};
			return store;
		}

		public override string ToString() {
			return "Choice";
		}
	}
}
