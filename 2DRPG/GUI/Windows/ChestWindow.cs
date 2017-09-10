using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI.Windows {
	class ChestWindow : IWindow {

		static UIGridLayout items = new UIGridLayout(0, 0, 3) { gridDisplayHeight = 3, gridWidth = 3 };

		static HashSet<UIBase> UIItems = new HashSet<UIBase>() {
			new UIBase(0, 0, 80, 80, 5, "darkBack"),
			items
		};

		public static List<Items.Item> displayItems = new List<Items.Item>();

		public HashSet<UIBase> GetScreenObjects() {
			return UIItems;
		}

		public HashSet<UIBase> LoadObjects() {
			UpdateItemGrid();
			return UIItems;
		}


		private void UpdateItemGrid() {
			List<UIItem> b = new List<UIItem>();
			for (int index = 0; index < displayItems.Count; index++) {
				int ind = index;
				Items.Item it = displayItems[ind];
				string texture = "random1";
				UIItem i = new UIItem(texture, it) { NineSliceRendering = false };
				i.SetButtonAction(() => {
					items.SelectGridItem(ind);
				});
				b.Add(i);
			}
			items.SetGridItems(b.ToArray());
		}

		string[] textureNames = new string[] { "darkBack", "random1", "selected" };

		public void LoadTextures() {
			Screen.WindowOpen = true;
			TextureManager.RegisterTextures(textureNames);
		}

		public void UnloadTextures() {
			Screen.WindowOpen = false;
			TextureManager.UnRegisterTextures(textureNames);
		}
	}
}
