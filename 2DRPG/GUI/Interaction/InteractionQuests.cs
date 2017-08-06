using _2DRPG.Quests;
using _2DRPG.World;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI.Interaction {
	class InteractionQuests : InteractionChoice {

		public List<string> questTags = new List<string>(); //use dictionary to pair with path names
		public List<int> questInts = new List<int>(); //use dictionary to pair with the quest tags
		public string interactionID;
		public bool immediateMode = false;

		public InteractionQuests(List<InteractionPath> choices) : base(choices) {

		}

		public InteractionQuests(GameSave.InteractionObjectStorage store) : base(store) {
			interactionID = store.text;
			questTags = ((JArray)store.extraData[1]).ToObject<List<string>>();
			questInts = ((JArray)store.extraData[2]).ToObject<List<int>>();
			immediateMode = (bool)store.extraData[0];
		}

		public override void Setup() {
			int counter = 0;
			for (int i = 0; i < paths.Count; i++) {
				InteractionPath p = paths[i];
				bool check = true;
				if (QuestData.QuestDatabase.ContainsKey(questTags[i]))
					if (questInts[i] != QuestData.QuestDatabase[questTags[i]].CheckStatus())
						check = false;
				if (check)
					items.Add(new UIButton(0, 50 - counter * 20, 30, 10, 2, "button") {
						displayLabel = new UIText(0, 50 - counter * 20, .5f, 1, p.pathName),
						buttonAction = () => {
							Windows.InteractionWindow.InsertNodes(p.items);
							nextNode.Invoke();
						}
					}, counter++);
			}
			if (immediateMode) {
				Windows.InteractionWindow.InsertNodes(paths[0].items);
				nextNode.Invoke();
			}

		}

		public override GameSave.InteractionObjectStorage StoreObject() {
			GameSave.InteractionObjectStorage store = base.StoreObject();
			store.objectType = GameSave.InteractionObjectType.Quests;
			store.text = interactionID;
			store.extraData = new object[]{ immediateMode, questTags.ToArray(), questInts.ToArray() };
			return store;
		}

		public override string ToString() {
			return "Quest";
		}
	}
}
