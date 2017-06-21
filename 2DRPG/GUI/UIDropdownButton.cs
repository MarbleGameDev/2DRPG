using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI {
	class UIDropdownButton : UIButton {

		private List<UIButton> drops = new List<UIButton>();
		private bool showDrops = false;
		private float spacing = .05f; //gap between buttons displayed in the dropdown

		public UIDropdownButton(float x, float y, float width, float height, UIButton[] dropdowns) : this(x, y, width, height, null, dropdowns) { }
		public UIDropdownButton(float x, float y, float width, float height, string labelText, UIButton[] dropdowns) : base(x, y, width, height, 1, "Button") {
			if (labelText != null)
				displayLabel = new UIText(x, y, width, height, labelText);
			int counter = 1;
			foreach (UIButton b in dropdowns) {
				b.width = width;
				b.height = height;
				b.SetScreenPosition(x, y - counter++ * height * (2 + spacing));
				drops.Add(b);
			}
			buttonAction = new Action(ToggleDropdowns);
		}

		public override void CheckClick(float x, float y) {
			base.CheckClick(x, y);
			if (showDrops)
				foreach (UIButton b in drops)
					b.CheckClick(x, y);
		}

		public override void Render() {
			base.Render();
			if (displayLabel != null)
				displayLabel.Render();
			if (showDrops)
				foreach (UIButton b in drops)
					b.Render();
		}


		private void ToggleDropdowns() {
			showDrops = !showDrops;
		}
	}
}
