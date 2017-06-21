using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI {
	class UIButton : UIBase {

		protected Action buttonAction;
		public UIText displayLabel = null;

		public UIButton(string textureName) : base(textureName) { }

		public UIButton(Action click) : base("Button") {
			buttonAction = click;
		}
		public UIButton(Action click, string labelText) : base("Button") {
			buttonAction = click;
			displayLabel = new UIText(0, 0, .1f, .1f, labelText);
		}
		public UIButton(Action click, string labelText, string textureName) : base(textureName) {
			buttonAction = click;
			displayLabel = new UIText(0, 0, .1f, .1f, labelText);
		}
		public UIButton(float x, float y, float width, float height, Action click, int layer, string textureName) : base(x, y, width, height, layer, textureName) {
			buttonAction = click;
		}
		public UIButton(float x, float y, float width, float height, int layer, string textureName) : base(x, y, width, height, layer, textureName) { }
		public UIButton(float x, float y, float width, float height, Action click) : base(x, y, width, height, 1, "Button") {
			SetLayer(defaultLayer);
			buttonAction = click;
		}

		public virtual void CheckClick(float x, float y) {
			if (LogicUtils.Logic.CheckIntersection(arrayPosition, x, y)) {
				if (buttonAction != null)
					buttonAction.Invoke();
			}
		}

		public void SetButtonAction(Action btn) {
			buttonAction = btn;
		}

		public override void Render() {
			base.Render();
			if (displayLabel != null)
				displayLabel.Render();
		}
		public override void SetScreenPosition(float x, float y) {
			base.SetScreenPosition(x, y);
			if (displayLabel != null) {
				displayLabel.width = width;
				displayLabel.height = height;
				displayLabel.SetScreenPosition(x, y);
			}
		}

	}
}
