using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI {
	class UIDropdownButton : UIButton {

		private List<UIButton> drops = new List<UIButton>();
		private bool showDrops = false;
		private float spacing = .0625f; //gap between buttons displayed in the dropdown

		public UIDropdownButton(float x, float y, float width, float height, UIButton[] dropdowns) : this(x, y, width, height, null, dropdowns) { }
		public UIDropdownButton(float x, float y, float width, float height, UIText label, UIButton[] dropdowns) : base(x, y, width, height, 1, "button") {
			displayLabel = label;
			int counter = 1;
			foreach (UIButton b in dropdowns) {
				b.width = width;
				b.height = height;
				if (displayLabel != null && b.displayLabel != null) {
					b.displayLabel.SetTextSize(displayLabel.textSize);
					System.Diagnostics.Debug.WriteLine("g");
				}
				b.SetScreenPosition(x, y - counter++ * height * (2 + spacing));
				
				drops.Add(b);
			}
			buttonAction = new Action(ToggleDropdowns);
		}

		public override bool CheckClick(float x, float y) {
			if (base.CheckClick(x, y))
				return true;
			if (showDrops)
				foreach (UIButton b in drops)
					if (b.CheckClick(x, y))
						return true;
			return false;
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
