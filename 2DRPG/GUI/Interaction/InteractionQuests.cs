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

		[Editable]
		public List<string> questTags = new List<string>();	//use dictionary to pair with path names
		public string interactionID;
		public bool immediateMode = false;

		public InteractionQuests(List<InteractionPath> choices) : base(choices) {

		}

		public InteractionQuests(GameSave.InteractionObjectStorage store) : base(store) {
			interactionID = store.text;
			questTags = ((JArray)store.extraData[1]).ToObject<List<string>>();
			immediateMode = (bool)store.extraData[0];
		}

		public override void Setup() {
			/*
			for (int i = 0; i < questTags.Count; i++) {
				if (QuestData.QuestDatabase.ContainsKey(questTags[i])) {
					if (!QuestData.QuestDatabase[questTags[i]].CheckPaths(interactionID, i))
						items.Remove(items.Keys.ElementAt(i));	//TODO: Fix sync error in itteration
				}
			}
			*/
			base.Setup();
			if (immediateMode) {
				Windows.InteractionWindow.InsertNodes(paths[0].items);
				nextNode.Invoke();
			}

		}

		public override GameSave.InteractionObjectStorage StoreObject() {
			GameSave.InteractionObjectStorage store = base.StoreObject();
			store.objectType = GameSave.InteractionObjectType.Quests;
			store.text = interactionID;
			store.extraData = new object[]{ immediateMode, questTags.ToArray() };
			return store;
		}

		public override string ToString() {
			return "Quest";
		}
	}
}
