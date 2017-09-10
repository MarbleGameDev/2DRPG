using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.World.Objects {
	interface IInventory : IInteractable {

		void SetItems(List<Items.Item> list);
		List<Items.Item> GetItems();
	}
}
