using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.Items;
using _2DRPG.Save;

namespace _2DRPG.Player {
	[Serializable]
	class StandardInventory {

		private List<Item> items = new List<Item>();

		private List<IEquippable> equipped = new List<IEquippable>();


		public void AddItem(Item i) {
			if (!Contains(i))
				items.Add(i);
			else {
				Item n = GetItem(i);
				if (n.Stackable) {
					n += i.Quantity;
					if (n == null)
						lock (items)
							items.Remove(n);
				} else {
					items.Add(i);
				}
			}

		}
		public void RemoveItem(Item i) {
			if (items.Contains(i))
				items.Remove(i);
		}

		public bool Contains(Item i) {
			Type t = i.GetType();
			lock(items)
				foreach (Item n in items) {
					if (n.GetType() == t && n.Name == i.Name)
						return true;
				}
			return false;
		}

		public Item GetItem(Item i) {
			Type t = i.GetType();
			lock(items)
				foreach (Item n in items) {
					if (n.GetType() == t)
						return n;
				}
			return null;
		}

		public void EquipItem(Item i) {
			if (i is IEquippable e)
				if (Contains(i)) {
					lock(items)
						items.Remove(i);
					lock(equipped)
						equipped.Add(e);
				}
		}
		public void EquipItem(int index) {
			if (index < items.Count)
				EquipItem(items[index]);
		}

		/// <summary>
		/// Returns the List of Items in the inventory
		/// </summary>
		/// <returns></returns>
		public List<Item> GetSet() {
			return items;
		}

		public void CleanupInventory() {
			for (int i = items.Count - 1; i >= 0; i--) {
				if (items[i] == null)
					items.RemoveAt(i--);
				else if (items[i].Quantity == 0)
					items.RemoveAt(i--);
			}
		}

	}
}
