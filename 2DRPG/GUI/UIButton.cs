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

		public UIButton(Action click) : base("button") {
			buttonAction = click;
		}
		public UIButton(Action click, string labelText) : base("button") {
			buttonAction = click;
			displayLabel = new UIText(0, 0, 1f, 1, labelText);
		}
		public UIButton(Action click, string labelText, string textureName) : base(textureName) {
			buttonAction = click;
			displayLabel = new UIText(0, 0, 1f, 1, labelText);
		}
		public UIButton(float x, float y, float width, float height, Action click, int layer, string textureName) : base(x, y, width, height, layer, textureName) {
			buttonAction = click;
		}
		public UIButton(float x, float y, float width, float height, int layer, string textureName) : base(x, y, width, height, layer, textureName) { }
		public UIButton(float x, float y, float width, float height, Action click) : base(x, y, width, height, 2, "button") {
			SetLayer(defaultLayer);
			buttonAction = click;
		}

		public virtual bool CheckClick(float x, float y) {
			bool check = CheckCoords(x, y);
			if (check) {
				if (buttonAction != null)
					buttonAction.Invoke();
			}
			return check;
		}
		public virtual bool CheckCoords(float x, float y) {
			return LogicUtils.Logic.CheckIntersection(arrayPosition, x, y);
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
				displayLabel.SetScreenPosition(x, y);
			}
		}

	}
}
