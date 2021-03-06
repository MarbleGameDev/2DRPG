﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI {
	class UIItem : UIButton {

		public bool IsSelected = false;

		public Items.Item backedItem;

		public UIItem(Texture textureName, Items.Item itm) : base(textureName) {
			backedItem = itm;
		}

		public UIItem(Action click, Texture textureName, Items.Item itm) : base (textureName) {
			SetButtonAction(click);
			backedItem = itm;
		}


		public override void Render() {
			base.Render();
			if (IsSelected) {
				Texture tmp = texName;
				texName = TextureManager.TextureNames.selected;
				base.Render();
				texName = tmp;
			}
		}
	}
}
