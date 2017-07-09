using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _2DRPG.GUI {
	class UIScrollBar : UIButton {

		private float mouseY;
		private float minY, maxY;
		private UIDraggable scrollKnob;
		public IScrollable scrollTarget;

		public UIScrollBar(float x, float y, float width, float height, int layer) : base(x, y, width, height, layer, "darkBack") {
			scrollKnob = new UIDraggable(x, y, width + 2, width + 2, 1, "lightBack");
			scrollKnob.SetButtonAction(Drag);
			buttonAction = new Action(SetScroll);
			maxY = y + height;
			minY = y - height;
		}


		public override bool CheckClick(float x, float y) {
			mouseY = scrollKnob.screenY - y;
			newMouse = y;
			return scrollKnob.CheckClick(x, y) || base.CheckClick(x, y);
		}

		public override void Render() {
			if (!Visible)
				return;
			base.Render();
			scrollKnob.Render();
		}

		private float newMouse;

		public void SetScroll() {
			if (scrollTarget != null) {
				if (newMouse <= maxY && newMouse >= minY) {
					scrollKnob.SetScreenPosition(scrollKnob.screenX, newMouse);
					float scrollAmount = ((int)maxY - (int)scrollKnob.screenY) / (float)((int)maxY - (int)minY);
					scrollTarget.ScrollTo(scrollAmount);
				}
			}
		}

		private void Drag() {
			Thread drag = new Thread(() => {
				while (Input.MouseHeld) {
					newMouse = Input.MouseY + mouseY;
					SetScroll();
					Thread.Sleep(10);
				}
			});
			drag.Start();
		}
	}
}
