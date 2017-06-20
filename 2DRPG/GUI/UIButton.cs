using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI {
	class UIButton : UIBase {

		Action buttonAction;

		public UIButton(Action click) : base("Button") {
			buttonAction = click;
		}
		public UIButton(Action click, string textureName) : base(textureName) {
			buttonAction = click;
		}
		public UIButton(float x, float y, float width, float height, Action click, int layer, string textureName) : base(x, y, width, height, layer, textureName) {
			buttonAction = click;
		}
		public UIButton(float x, float y, float width, float height, Action click) : base(x, y, width, height, 1, "Button") {
			SetLayer(defaultLayer);
			buttonAction = click;
			texturePosition = new float[] {
			1.0f, 0.0f,
			1.0f, 1.0f,
			2.0f, 1.0f,
			2.0f, 0.0f
		};
		}

		public void CheckClick(float x, float y) {
			if (LogicUtils.Logic.CheckIntersection(arrayPosition, x, y)) {
				if (buttonAction != null)
					buttonAction.Invoke();
			}
		}

	}
}
