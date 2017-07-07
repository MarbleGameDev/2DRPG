using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;

namespace _2DRPG.GUI {
	class UIDropdownButton : UIButton, IScrollable {

		private List<UIButton> drops = new List<UIButton>();
		private bool showDrops = false;
		public float spacing = 1f; //gap between buttons displayed in the dropdown
		public float displaySize = 1f;  //number of dropdown elements displayed at a time
		private int[] scissorMask = new int[4];
		private float scrollAmount = 0, scrollMax;

		public UIDropdownButton(float x, float y, float width, float height, UIButton[] dropdowns) : this(x, y, width, height, null, dropdowns) { }
		public UIDropdownButton(float x, float y, float width, float height, UIText label, UIButton[] dropdowns) : base(x, y, width, height, 2, "button") {
			displayLabel = label;
			SetDropdowns(dropdowns);
			buttonAction = new Action(ToggleDropdowns);
			SetScissorMask();
		}

		public void SetDropdowns(UIButton[] dropdowns) {
			if (dropdowns == null)
				return;
			int counter = 1;
			foreach (UIButton b in dropdowns) {
				b.width = width;
				b.height = height;
				if (displayLabel != null && b.displayLabel != null) {
					b.displayLabel.SetTextSize(displayLabel.textSize);
				}
				b.SetScreenPosition(screenX, screenY - counter++ * (height * 2 + spacing));

				drops.Add(b);
			}
			scrollMax = (drops.Count - displaySize) * (height * 2 + spacing);
		}

		public void SetScissorMask() {
			scissorMask[0] = (int)(Screen.PixeltoNormalizedWidth(screenX - width) * Screen.screenWidth) + Screen.screenX;
			scissorMask[1] = (int)(Screen.PixeltoNormalizedHeight(screenY - height - displaySize * (height * 2 + spacing)) * Screen.screenHeight) + Screen.screenY;
			scissorMask[2] = -(int)(Screen.PixeltoNormalizedWidth(screenX - width) * Screen.screenWidth) + (int)(Screen.PixeltoNormalizedWidth(screenX + width) * Screen.screenWidth);
			scissorMask[3] = (int)(Screen.PixeltoNormalizedHeight(screenY - height) * Screen.screenHeight) - (int)(Screen.PixeltoNormalizedHeight(screenY - height - displaySize * (height * 2 + spacing)) * Screen.screenHeight);
		}

		private int scrollMod = 4;	//Amount of pixels each mouse turn moves
		public void ScrollWheel(int y) {
			float temp = 0;
			if (y > 0) {
				temp = (scrollAmount > scrollMod) ? -scrollMod : -(scrollAmount % scrollMod);
			} else if (y < 0) {
				temp = (scrollMax - scrollAmount > scrollMod) ? scrollMod : ((scrollMax - scrollAmount) % scrollMod);
			}
			foreach (UIButton b in drops) {
				b.ShiftScreenPosition(0, temp);
			}
			scrollAmount += temp;
		}

		public override bool CheckClick(float x, float y) {
			if (base.CheckClick(x, y)) {
				SetScissorMask();
				return true;
			}
			if (showDrops) {
				foreach (UIButton b in drops)
					if (b.CheckClick(x, y))
						return true;
			}
			return false;
		}

		public new bool CheckCoords(float x, float y) {
			if (showDrops) {
				float[] testCoords = new float[12];
				arrayPosition.CopyTo(testCoords, 0);
				testCoords[1] -= displaySize * (height * 2 + spacing);
				testCoords[10] -= displaySize * (height * 2 + spacing);
				return LogicUtils.Logic.CheckIntersection(testCoords, x, y);
			} else {
				return base.CheckCoords(x, y);
			}
		}

		public override void Render() {
			base.Render();
			if (displayLabel != null)
				displayLabel.Render();
			if (showDrops) {
				Gl.PushAttrib(AttribMask.ScissorBit);
				Gl.Scissor(scissorMask[0], scissorMask[1], scissorMask[2], scissorMask[3]);
				foreach (UIButton b in drops)
					b.Render();
			}
			Gl.PopAttrib();
		}


		public void ToggleDropdowns() {
			showDrops = !showDrops;
		}
	}
}
