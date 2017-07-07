using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace _2DRPG.GUI {
	class UIDraggable : UIButton{

		private float mouseX, mouseY;

		public UIDraggable(float x, float y, float width, float height, int layer = 0, string textureName = "button") : base(x, y, width, height, layer, textureName) {
			buttonAction = new Action(Drag);
		}

		public override bool CheckClick(float x, float y) {
			mouseX = screenX - x;
			mouseY = screenY - y;
			return base.CheckClick(x, y);
		}

		private void Drag() {
			Thread drag = new Thread(() => {
				while (Input.MouseHeld) {
					SetScreenPosition(Input.MouseX + mouseX, Input.MouseY + mouseY);
					Thread.Sleep(10);
				}
			});
			drag.Start();
		}
	}
}
