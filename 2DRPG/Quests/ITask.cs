using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.Quests {
	interface ITask {
		List<GUI.Interaction.InteractionBase> CheckQuestElements(string InteractableID);
	}
}
