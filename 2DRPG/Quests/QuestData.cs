using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.Quests {
	static class QuestData {
		
		//String names for storage are retrieved via ToString() on the IQuest interface
		//Subclass implementations may use a string variable for name storage

		public static Dictionary<string, IQuest> QuestDatabase = new Dictionary<string, IQuest>();

		public static HashSet<string> ActiveQuests = new HashSet<string>();

		public static void SetQuestActive(string questName, bool active) {
			if (active) {
				if (!ActiveQuests.Contains(questName))
					ActiveQuests.Add(questName);
			} else {
				if (ActiveQuests.Contains(questName))
					ActiveQuests.Remove(questName);
			}
		}

	}
}
