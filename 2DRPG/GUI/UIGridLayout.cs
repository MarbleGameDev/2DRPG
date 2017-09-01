using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI {
	class UIGridLayout : UIButton, IScrollable {

		List<UIItem> gridItems = new List<UIItem>();

		private int selectedIndex = -1;

		public int gridWidth = 5;
		public int gridDisplayHeight = 6;
		public float gridSize = 32;

		private int gridSpacing = 2;

		private int[] scissorMask = new int[4];
		private float scrollAmount = 0, scrollMax = 0;

		public IScrollable scrollbar = null;

		public UIGridLayout(float x, float y, int layer) : base(x, y, 16, 16, null) {
			SetLayer(layer);
		}

		public void SelectGridItem(int index) {
			if (index >= gridItems.Count)
				return;
			foreach (UIItem i in gridItems) {
				i.IsSelected = false;
			}
			gridItems[index].IsSelected = true;
			selectedIndex = index;
		}

		public UIItem GetSelectedGridItem() {
			if (selectedIndex > 0) {
				return gridItems[selectedIndex];
			}
			return null;
		}

		public void SetGridItems(UIItem[] items) {
			if (items == null)
				return;
			gridItems.Clear();
			int counter = 0;
			float xLeft = screenX - ((gridWidth - 1) * (gridSize + gridSpacing)) / 2f;
			float yTop = ((gridDisplayHeight-1) * (gridSize + gridSpacing)) / 2f + screenY;
			foreach (UIItem b in items) {
				b.SetScreenDimensions((counter % gridWidth) * (gridSize + gridSpacing) + xLeft, yTop - (counter / gridWidth) * (gridSize + gridSpacing), gridSize / 2f, gridSize / 2f);
				counter++;
				b.SetLayer(layer);
				gridItems.Add(b);
			}
			SetScissorMask();
			scrollMax = ((int)Math.Ceiling(gridItems.Count / (double)gridWidth) - gridDisplayHeight) * (gridSize + gridSpacing);
		}

		public void SetScissorMask() {
			float wid = (gridWidth * (gridSize + gridSpacing) - gridSpacing) / 2f;
			float hei = (gridDisplayHeight * (gridSize + gridSpacing) - gridSpacing) / 2f;
			scissorMask[0] = (int)(Screen.PixeltoNormalizedWidth(screenX - wid) * Screen.screenWidth) + Screen.screenX;
			scissorMask[1] = (int)(Screen.PixeltoNormalizedHeight(screenY - hei) * Screen.screenHeight) + Screen.screenY;
			scissorMask[2] = (int)(Screen.PixeltoNormalizedWidth(screenX + wid) * Screen.screenWidth) - (int)(Screen.PixeltoNormalizedWidth(screenX - wid) * Screen.screenWidth);
			scissorMask[3] = (int)(Screen.PixeltoNormalizedHeight(screenY + hei) * Screen.screenHeight) - (int)(Screen.PixeltoNormalizedHeight(screenY - hei) * Screen.screenHeight);
			SetScreenDimensions(wid, hei);
		}

		public bool CheckScrollWheel(float x, float y, int dir, object sender) {
			if (base.CheckCoords(x, y)) {
				ScrollWheel(dir, sender);
				return true;
			}
			return false;
		}

		public void ScrollTo(float amount) {
			float desiredScroll = amount * scrollMax;
			desiredScroll = desiredScroll - scrollAmount;
			foreach (UIButton b in gridItems)
				b.ShiftScreenPosition(0, desiredScroll);
			scrollAmount += desiredScroll;
		}
		private int scrollMod = 4;
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
			foreach (UIButton b in gridItems) {
				b.ShiftScreenPosition(0, temp);
			}
			scrollAmount += temp;
		}

		public override void Render() {
			if (!Visible)
				return;
			Gl.PushAttrib(AttribMask.ScissorBit);
			Gl.Scissor(scissorMask[0], scissorMask[1], scissorMask[2], scissorMask[3]);
			foreach (UIBase b in gridItems) {
				b.Render();
			}
			Gl.PopAttrib();
		}

		public override bool CheckClick(float x, float y) {
			foreach (UIButton b in gridItems)
				if (b.CheckClick(x, y))
					return true;
			return false;
		}
		public override bool CheckCoords(float x, float y) {
			foreach (UIButton b in gridItems)
				if (b.CheckCoords(x, y))
					return true;
			return false;
		}

		public override void Setup() {
			foreach (UIBase b in gridItems)
				b.Setup();
			Screen.ResizeEvent += SetScissorMask;
		}

		public override void Cleanup() {
			foreach (UIBase b in gridItems)
				b.Cleanup();
			Screen.ResizeEvent -= SetScissorMask;
		}
	}
}
