using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.Items;
using _2DRPG.Save;

namespace _2DRPG.World.Objects {
	[Serializable]
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
		public WorldObjectSimpleItem(float x, float y, int layer, Texture textureName, float width = 16, float height = 16) : base(x, y, layer, textureName, width, height){

		}


		public void Interact() {
			foreach (Item i in items)
				WorldData.player.Data.inventory.AddItem(i);
			WorldData.RemoveObject(this);
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
