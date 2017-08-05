using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.Quests {
	class TaskBase : IQuest {

		public string taskName = "default";

		public bool completed = false;

		public List<ItemPickup> taskItems = new List<ItemPickup>();

		/// <summary>
		/// It's assumed that tasks will only involve 1 person for checking dialogue, therefore all interactionID and pathNum return task logic
		/// </summary>
		public bool CheckPaths(string interactionID, int pathNum) {
			bool check = true;
			foreach (ItemPickup p in taskItems)
				if (!p.complete)
					check = false;
			return check;
		}

		public void UpdateItem(string itemName, int quantity) {
			foreach (ItemPickup p in taskItems) {
				if (p.itemName.Equals(itemName)) {
					if (p.itemQuantity <= quantity)
						p.complete = true;
				}
			}
		}

		public override string ToString() {
			return taskName;
		}
	}
}
