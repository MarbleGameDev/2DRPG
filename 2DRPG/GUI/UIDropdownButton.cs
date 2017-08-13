using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;

namespace _2DRPG.GUI {
	class UIDropdownButton : UIButton, IScrollable {
		/// <summary>
		/// List of UIButtons that make up the dropdown items
		/// </summary>
		public List<UIButton> drops = new List<UIButton>();
		public bool showDrops = false;
		public bool hideTop = false;
		private float spacing = 1f; //gap between buttons displayed in the dropdown
									/// <summary>
									/// Number of dropdown elements displayed at a time
									/// </summary>
		public float displaySize = 2f;
		private int[] scissorMask = new int[4];
		private float scrollAmount = 0, scrollMax;

		public IScrollable scrollbar = null;

		public UIDropdownButton(float x, float y, float width, float height, int layer, string texName, UIButton[] dropdowns) : this(x, y, width, height, layer, texName, null, dropdowns) { }

		/// <summary>
		/// Complete Declaration for UIDropdownButton
		/// </summary>
		/// <param name="x">X position on the screen</param>
		/// <param name="y">Y position on the screen</param>
		/// <param name="width">Distance to the left and right</param>
		/// <param name="height">Distance to the top and bottom</param>
		/// <param name="layer">Render layer</param>
		/// <param name="texName">Name of the texture</param>
		/// <param name="label">UIText label to be displayed on the dropdown button</param>
		/// <param name="dropdowns">array of UIButtons to consist of the dropdown options</param>
		public UIDropdownButton(float x, float y, float width, float height, int layer, string texName, UIText label, UIButton[] dropdowns) : base(x, y, width, height, layer, texName) {
			displayLabel = label;
			SetDropdowns(dropdowns);
			buttonAction = new Action(ToggleDropdowns);
		}
		public UIDropdownButton(float x, float y, float width, float height, int layer, string texName, UIText label) : this(x, y, width, height, layer, texName, label, null) { }

		public void SetDropdowns(UIButton[] dropdowns) {
			if (dropdowns == null)
				return;
			drops.Clear();
			int counter = 1;
			foreach (UIButton b in dropdowns) {
				b.width = width;
				b.height = height;
				if (displayLabel != null && b.displayLabel != null) {
					b.displayLabel.SetTextSize(displayLabel.textSize);
				}
				b.SetScreenPosition(screenX, screenY - counter++ * (height * 2 + spacing), layer - 2);

				drops.Add(b);
			}
			scrollMax = (drops.Count - displaySize) * (height * 2 + spacing);
			SetScissorMask();
		}

		public void SetScissorMask() {
			scissorMask[0] = (int)(Screen.PixeltoNormalizedWidth(screenX - width) * Screen.screenWidth) + Screen.screenX;
			scissorMask[1] = (int)(Screen.PixeltoNormalizedHeight(screenY - height - displaySize * (height * 2 + spacing)) * Screen.screenHeight) + Screen.screenY;
			scissorMask[2] = -(int)(Screen.PixeltoNormalizedWidth(screenX - width) * Screen.screenWidth) + (int)(Screen.PixeltoNormalizedWidth(screenX + width) * Screen.screenWidth);
			scissorMask[3] = (int)(Screen.PixeltoNormalizedHeight(screenY - height) * Screen.screenHeight) - (int)(Screen.PixeltoNormalizedHeight(screenY - height - displaySize * (height * 2 + spacing)) * Screen.screenHeight);
		}

		private int scrollMod = 4;  //Amount of pixels each mouse turn moves
		public void ScrollWheel(int y, object sender) {
			if (scrollbar != null && sender != scrollbar) {
				scrollbar.ScrollWheel(y, this);
			}
			float temp = 0;
			if (y > 0) {
				temp = (scrollAmount >= scrollMod) ? -scrollMod : -(scrollAmount % scrollMod);
			} else if (y < 0) {
				temp = (scrollMax - scrollAmount >= scrollMod) ? scrollMod : ((scrollMax - scrollAmount) % scrollMod);
			}
			foreach (UIButton b in drops) {
				b.ShiftScreenPosition(0, temp);
			}
			scrollAmount += temp;
		}

		public void ScrollTo(float val) {
			float desiredScroll = val * scrollMax;
			desiredScroll = desiredScroll - scrollAmount;
			foreach (UIButton b in drops)
				b.ShiftScreenPosition(0, desiredScroll);
			scrollAmount += desiredScroll;
		}

		public override bool CheckClick(float x, float y) {
			if (!Visible)
				return false;
			if (!hideTop && base.CheckClick(x, y)) {
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
			if (!Visible)
				return false;
			if (showDrops) {
				float[] testCoords = new float[12];
				quadPosition.CopyTo(testCoords, 0);
				testCoords[1] -= displaySize * (height * 2 + spacing);
				testCoords[10] -= displaySize * (height * 2 + spacing);
				return LogicUtils.Logic.CheckIntersection(testCoords, x, y);
			} else {
				if (!hideTop)
					return base.CheckCoords(x, y);
				else
					return false;
			}
		}

		public override void Render() {
			if (!Visible)
				return;
			if (!hideTop)
				base.Render();
			if (displayLabel != null && !hideTop)
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
			Screen.SelectionEvent -= HideDropdowns;
			Screen.InvokeSelection();
			Screen.SelectionEvent += HideDropdowns;
			showDrops = !showDrops;
		}
		/// <summary>
		/// If the top is hidden, it won't ever be hidden by selections
		/// </summary>
		public void HideDropdowns() {
			if (!hideTop)
				showDrops = false;
		}

		public override void Cleanup() {
			foreach (UIButton b in drops)
				b.Cleanup();
			Screen.ResizeEvent -= SetScissorMask;
			Screen.SelectionEvent -= HideDropdowns;
		}

		public override void Setup() {
			SetScissorMask();
			Screen.ResizeEvent += SetScissorMask;
			Screen.SelectionEvent += HideDropdowns;
		}
	}
}
