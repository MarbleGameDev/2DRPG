using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.Items;
using _2DRPG.Save;

namespace _2DRPG.World.Objects {
	class WorldObjectInventory : WorldObjectBase, IInventory {

		[Editable]
		public string InventoryName = "inventory";

		private List<Item> items = new List<Item>();

		/// <summary>
		/// Complete Declaration for WorldObjectInventory
		/// </summary>
		/// <param name="x">X position in the world</param>
		/// <param name="y">Y position in the world</param>
		/// <param name="layer">Render Layer</param>
		/// <param name="textureName">Name of the texture</param>
		public WorldObjectInventory(float x, float y, int layer, string textureName = "default", float width = 16, float height = 16) : base(x, y, layer, textureName, width, height) {

		}

		public WorldObjectInventory(RegionSave.WorldObjectStorage store) : this(store.worldX, store.worldY, store.layer, store.textureName, store.width, store.height) {
			InventoryName = (string)store.extraData[0];
			foreach (RegionSave.ItemStorage st in ((JArray)store.extraData[1]).ToObject<List<RegionSave.ItemStorage>>()) {
				items.Add((Item)Activator.CreateInstance(st.type, st));
			}
		}

		public void Interact() {
			GUI.Windows.ChestWindow.displayItems = items;
			Screen.OpenWindow("chest");
		}

		public override RegionSave.WorldObjectStorage StoreObject() {
			RegionSave.WorldObjectStorage store = base.StoreObject();
			List<RegionSave.ItemStorage> itemstores = new List<RegionSave.ItemStorage>();
			foreach (Item i in items)
				itemstores.Add(i.StoreObject());
			store.extraData = new object[] { InventoryName, itemstores };
			store.objectType = RegionSave.WorldObjectType.Inventory;
			return store;
		}

		public string GetName() {
			return InventoryName;
		}

		public void SetItems(List<Item> list) {
			items = list;
		}

		public List<Item> GetItems() {
			return items;
		}
	}
}
