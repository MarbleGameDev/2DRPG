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
		/// Returns 0 for fresh quest, -1 for completed, otherwise the number of ItemPickups completed
		/// </summary>
		public int CheckStatus() {
			int i = 0;
			foreach (ItemPickup p in taskItems)
				if (p.complete)
					i++;
			if (completed)
				i = -1;
			return i;
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
