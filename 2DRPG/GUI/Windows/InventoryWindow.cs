﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.Items;

namespace _2DRPG.GUI.Windows {
	class InventoryWindow : IWindow {

		static UIGridLayout items = new UIGridLayout(-100, 30, 3);

		static UITextBox description = new UITextBox(100, 100, .5f, 100, 3, 6, "Placeholder text");

		static HashSet<UIBase> UIObjects = new HashSet<UIBase>() {
			new UIBase(0, 0, 200, 160, 5, TextureManager.TextureNames.darkBack),
			new UIText(0, 150, .5f, 4, "Inventory Window"),
			new UIBase(-100, 30, 88, 104, 4, TextureManager.TextureNames.lightBack),
			items, description
		};

		public HashSet<UIBase> GetScreenObjects() {
			return UIObjects;
		}

		public HashSet<UIBase> LoadObjects() {
			UpdateInventoryItems();
			return UIObjects;
		}

		public void UpdateInventoryItems() {
			Random rnd = new Random();
			List<UIItem> b = new List<UIItem>();
			for (int index = 0; index < WorldData.player.Data.inventory.GetSet().Count; index++) {
				int ind = index;
				Item it = WorldData.player.Data.inventory.GetSet()[ind];
				//b.Add(new UIItem(() => {
				//	description.SetText(it.Name);
				/*
				if (it is IConsumable) {
					Player.MCObject.Data.inventory.GetSet()[ind] -= 1;
					Player.MCObject.Data.inventory.CleanupInventory();
					UpdateInventoryItems();
				} else if (it is IEquippable) {
					Player.MCObject.Data.inventory.EquipItem(ind);
					UpdateInventoryItems();
				}
				*/
				//}, "button"));
				Texture texture = TextureManager.TextureNames.DEFAULT;
				UIItem i = new UIItem(texture, it) { NineSliceRendering = false};
				i.SetButtonAction(() => {
					description.SetText(it.Name);
					items.SelectGridItem(ind);
				});
				b.Add(i);
			}
			items.SetGridItems(b.ToArray());
		}

		static Texture[] textures = new Texture[] {
			TextureManager.TextureNames.darkBack, TextureManager.TextureNames.button, TextureManager.TextureNames.lightBack, TextureManager.TextureNames.selected
		};

		public void LoadTextures() {
			Screen.CloseWindow("hud");
			Screen.WindowOpen = true;
			TextureManager.RegisterTextures(textures);
			items.ScrollTo(0);
		}

		public void UnloadTextures() {
			Screen.OpenWindow("hud");
			Screen.WindowOpen = false;
			TextureManager.UnRegisterTextures(textures);
		}
	}
}
