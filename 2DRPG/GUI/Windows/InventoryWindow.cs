using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.Items;

namespace _2DRPG.GUI.Windows {
	class InventoryWindow : IWindow {

		static UIDropdownButton items = new UIDropdownButton(-100, 100, 80, 15, 3, "button", new UIText(0, 0, .5f, 1, ""), new UIButton[] { }) { showDrops = true, hideTop = true, displaySize = 8};

		static HashSet<UIBase> UIObjects = new HashSet<UIBase>() {
			new UIBase(0, 0, 200, 200, 5, "darkBack"),
			new UIText(0, 0, 1f, 4, "Inventory Window"),
			items
		};

		public HashSet<UIBase> GetScreenObjects() {
			return UIObjects;
		}

		public HashSet<UIBase> LoadObjects() {
			UpdateInventoryItems();
			return UIObjects;
		}

		public void UpdateInventoryItems() {
			List<UIButton> b = new List<UIButton>();
			for (int index = 0; index < Player.MCObject.Data.inventory.GetSet().Count; index++) {
				int ind = index;
				Item it = Player.MCObject.Data.inventory.GetSet()[ind];
				b.Add(new UIButton(() => {
					if (it is IConsumable) {
						Player.MCObject.Data.inventory.GetSet()[ind] -= 1;
						Player.MCObject.Data.inventory.CleanupInventory();
						UpdateInventoryItems();
					} else if (it is IEquippable) {
						Player.MCObject.Data.inventory.EquipItem(ind);
						UpdateInventoryItems();
					}
				}, it.Name + ((it.Quantity > 1) ? (" (" + it.Quantity + ")") : ("")), "button"));
			}
			items.SetDropdowns(b.ToArray());
		}

		static string[] textures = new string[] { "darkBack", "button"};

		public void LoadTextures() {
			Screen.CloseWindow("hud");
			Screen.WindowOpen = true;
			TextureManager.RegisterTextures(textures);
		}

		public void UnloadTextures() {
			Screen.OpenWindow("hud");
			Screen.WindowOpen = false;
			TextureManager.UnRegisterTextures(textures);
		}
	}
}
