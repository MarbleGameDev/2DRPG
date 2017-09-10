using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.Items;

namespace _2DRPG.World.Objects {
	class WorldObjectSimpleItem : WorldObjectBase, IInventory {

		[Editable]
		public string Name = "Item";

		public List<Item> items = new List<Item>();


		/// <summary>
		/// Complete Declaration for WorldObjectSimpleItem
		/// </summary>
		/// <param name="x">X position in the world</param>
		/// <param name="y">Y position in the world</param>
		/// <param name="layer">Render Layer</param>
		/// <param name="textureName">Name of the texture</param>
		public WorldObjectSimpleItem(float x, float y, int layer, string textureName = "default", float width = 16, float height = 16) : base(x, y, layer, textureName, width, height){

		}

		public WorldObjectSimpleItem(GameSave.WorldObjectStorage store) : this(store.worldX, store.worldY, store.layer, store.textureName, store.width, store.height) {
			Name = (string)store.extraData[0];
			foreach (GameSave.ItemStorage st in ((JArray)store.extraData[1]).ToObject<List<GameSave.ItemStorage>>()) {
				items.Add((Item)Activator.CreateInstance(st.type, st));
			}
		}

		public void Interact() {
			foreach (Item i in items)
				Player.MCObject.Data.inventory.AddItem(i);
			WorldData.RemoveObject(this);
		}

		public override GameSave.WorldObjectStorage StoreObject() {
			GameSave.WorldObjectStorage store = base.StoreObject();
			List<GameSave.ItemStorage> itemstores = new List<GameSave.ItemStorage>();
			foreach (Item i in items)
				itemstores.Add(i.StoreObject());
			store.extraData = new object[] { Name, itemstores };
			store.objectType = GameSave.WorldObjectType.SimpleItem;
			return store;

		}

		public string GetName() {
			return Name;
		}

		public void SetItems(List<Item> list) {
			items = list;
		}

		public List<Item> GetItems() {
			return items;
		}
	}
}
