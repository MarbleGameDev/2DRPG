using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI.Interaction {
	class InteractionQuests : InteractionBase {

		public string questTag = "";

		public override void Render() {

		}

		public InteractionQuests(GameSave.InteractionObjectStorage store) {
			questTag = store.text;
		}

		public override void Setup() {

		}

		public override GameSave.InteractionObjectStorage StoreObject() {
			GameSave.InteractionObjectStorage store = new GameSave.InteractionObjectStorage() {
				objectType = GameSave.InteractionObjectType.Quests, text = questTag
			};
			return store;
		}
	}
}
