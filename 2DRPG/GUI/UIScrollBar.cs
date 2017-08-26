using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _2DRPG.GUI {
	class UIScrollBar : UIButton, IScrollable {

		private float mouseY;
		private float minY, maxY;
		private UIDraggable scrollKnob;
		public IScrollable scrollTarget;

		public UIScrollBar(float x, float y, float width, float height, int layer) : base(x, y, width, height, layer, "textBox") {
			scrollKnob = new UIDraggable(x, y, width + 2, width + 2, 1, "textBox") {
				positionUpdate = () => {
					newMouse = Input.MouseY + mouseY;
					SetScroll();
				}
			};
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
				} else if (newMouse > maxY) {
					scrollKnob.SetScreenPosition(scrollKnob.screenX, maxY);
					float scrollAmount = ((int)maxY - (int)scrollKnob.screenY) / (float)((int)maxY - (int)minY);
					scrollTarget.ScrollTo(scrollAmount);
				} else if (newMouse < minY) {
					scrollKnob.SetScreenPosition(scrollKnob.screenX, minY);
					float scrollAmount = ((int)maxY - (int)scrollKnob.screenY) / (float)((int)maxY - (int)minY);
					scrollTarget.ScrollTo(scrollAmount);
				}
			}
		}

		float scrollMod = 4f;
		public void ScrollWheel(int y, object sender) {
			float temp = 0;
			if (y < 0) {
				temp = (scrollKnob.screenY >= (minY + scrollMod)) ? -scrollMod : -((scrollKnob.screenY - minY) % scrollMod);
			} else if (y > 0) {
				temp = ((maxY - scrollKnob.screenY) >= scrollMod) ? scrollMod : ((maxY - scrollKnob.screenY) % scrollMod);
			}
			scrollKnob.ShiftScreenPosition(0, temp);
			float scrollAmount = ((int)maxY - (int)scrollKnob.screenY) / (float)((int)maxY - (int)minY);
			scrollTarget.ScrollTo(scrollAmount);
		}

		public bool CheckScrollWheel(float x, float y, int dir, object sender) {
			if (CheckCoords(x, y)) {
				ScrollWheel(dir, sender);
				return true;
			}
			return false;
		}

		public void ScrollTo(float amount) {
			newMouse = ((int)maxY - amount * ((int)maxY - (int)minY));
			SetScroll();
		}
	}
}
