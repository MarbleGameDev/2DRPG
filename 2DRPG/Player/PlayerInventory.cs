using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.Items;

namespace _2DRPG.Player {
	class PlayerInventory {

		private List<Item> items = new List<Item>();

		private List<IEquippable> equipped = new List<IEquippable>();

		public List<GameSave.ItemStorage> ItemList{
			get {
				List<GameSave.ItemStorage> stores = new List<GameSave.ItemStorage>();
				foreach (Item i in items)
					stores.Add(i.SerializeObject());
				return stores;
			} set {
				foreach (GameSave.ItemStorage store in value) {
					items.Add((Item)Activator.CreateInstance(store.type, store));
				}
			}
		}

		public List<GameSave.ItemStorage> EquippedList {
			get {
				List<GameSave.ItemStorage> stores = new List<GameSave.ItemStorage>();
				foreach (Item i in equipped)
					stores.Add(i.SerializeObject());
				return stores;
			}
			set {
				foreach (GameSave.ItemStorage store in value)
					equipped.Add((IEquippable)Activator.CreateInstance(store.type, store));
			}
		}

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

		public List<Item> GetSet() {
			return items;
		}

		public void CleanupInventory() {
			for (int i = items.Count - 1; i >= 0; i--) {
				if (items[i] == null)
					items.RemoveAt(i--);
			}
		}

	}
}
