using _2DRPG.Quests;
using _2DRPG.World;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.Save;

namespace _2DRPG.GUI.Interaction {
	[Serializable]
	class InteractionQuests : InteractionChoice {

		public List<string> questTags = new List<string>(); //use dictionary to pair with path names
		public List<int> questInts = new List<int>(); //use dictionary to pair with the quest tags
		public string interactionID = "interaction";
		public bool immediateMode = false;

		public InteractionQuests(List<InteractionPath> choices) : base(choices) {

		}

		public override void Setup() {
			items.Clear();
			int counter = 0;
			for (int i = 0; i < paths.Count; i++) {
				InteractionPath p = paths[i];
				bool check = true;
				if (QuestData.QuestDatabase.ContainsKey(questTags[i]))
					if (questInts[i] != QuestData.QuestDatabase[questTags[i]].CheckStatus())
						check = false;
				if (check)
					items.Add(new UIButton(0, -100 - counter * 21, 30, 10, 2, TextureManager.TextureNames.textBox) {
						displayLabel = new UIText(0, -98 - counter++ * 21, .5f, 1, p.pathName),
						buttonAction = () => {
							Windows.InteractionWindow.InsertNodes(p.items);
							nextNode.Invoke();
						}
					});
			}
			if (immediateMode) {
				Windows.InteractionWindow.InsertNodes(paths[0].items);
				nextNode.Invoke();
			}

		}
		public override void Takedown() {

		}

		public override string ToString() {
			return "Quest";
		}
	}
}
