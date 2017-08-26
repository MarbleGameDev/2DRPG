using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace _2DRPG.GUI {
	class UITab : UIButton, IScrollable {

		private List<UIButton> tabButtons = new List<UIButton>();
		private HashSet<UIBase>[] tabItems;
		private int renderedIndex = 0;
		public Color selectedColor = Color.BlueViolet;
		public Color deselectedColor = Color.Black;

		/// <summary>
		/// Sets up a tab view that toggles between hashsets passed via buttons of the same index also passed
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="layer"></param>
		/// <param name="tabs"></param>
		/// <param name="tabItems"></param>
		public UITab(float x, float y, float width, float height, int layer, List<UIButton> tabs, HashSet<UIBase>[] tabItems) : base("button") {
			int counter = 0;
			foreach(UIButton b in tabs) {
				b.SetScreenDimensions(x + counter * (width * 2 + 2), y, width, height);
				int index = counter++;
				b.SetButtonAction(() => {
					SetTab(index);
				});
				tabButtons.Add(b);
			}
			this.tabItems = tabItems;
			SetTab(0);
		}

		public void SetTab(int index) {
			if (index < tabItems.Length) {
				tabButtons[renderedIndex].displayLabel.textColor = deselectedColor;
				renderedIndex = index;
				tabButtons[renderedIndex].displayLabel.textColor = selectedColor;
			}
		}

		public override void Render() {
			if (!Visible)
				return;
			foreach (UIButton b in tabButtons)
				b.Render();
			foreach (UIBase b in tabItems[renderedIndex])
				b.Render();
		}

		public override bool CheckClick(float x, float y) {
			if (!Visible)
				return false;
			foreach (UIButton b in tabButtons)
				if (b.CheckClick(x, y))
					return true;
			foreach (UIBase b in tabItems[renderedIndex])
				if (b is UIButton but)
					if (but.CheckClick(x, y))
						return true;
			return false;
		}

		public override bool CheckCoords(float x, float y) {
			bool result = false;
			foreach (UIBase b in tabItems[renderedIndex]) {
				if (b is IScrollable sc)
					if (sc.CheckCoords(x, y))
						result = true;
			}
			return result;
		}

		public override void Cleanup() {
			foreach (HashSet<UIBase> set in tabItems)
				foreach (UIBase b in set)
					b.Cleanup();
		}
		public override void Setup() {
			foreach (HashSet<UIBase> set in tabItems)
				foreach (UIBase b in set)
					b.Setup();
		}

		public void ScrollWheel(int dir, object sender) {
			return;
		}
		public bool CheckScrollWheel(float x, float y, int dir, object sender) {
			foreach (UIBase b in tabItems[renderedIndex])
				if (b is IScrollable sc)
					if (sc.CheckScrollWheel(x, y, dir, sender))
						return true;
			return false;
		}

		public void ScrollTo(float amount) {
			throw new NotImplementedException();
		}
	}
}
