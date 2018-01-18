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
		public WorldObjectInventory(float x, float y, int layer, Texture textureName, float width = 16, float height = 16) : base(x, y, layer, textureName, width, height) {

		}

		public void Interact() {
			GUI.Windows.ChestWindow.displayItems = items;
			Screen.OpenWindow("chest");
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
