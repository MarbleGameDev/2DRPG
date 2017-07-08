using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI {
	class UIButton : UIBase {

		protected Action buttonAction;
		/// <summary>
		/// Text displayed on the button
		/// </summary>
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
		/// <summary>
		/// Complete Declaration for UIButton
		/// </summary>
		/// <param name="x">X position on the screen</param>
		/// <param name="y">Y position on the screen</param>
		/// <param name="width">Distance to the left and right</param>
		/// <param name="height">Distance to the top and bottom</param>
		/// <param name="click">Action to be executed when clicked</param>
		/// <param name="layer">Render layer</param>
		/// <param name="textureName">Name of the texture</param>
		public UIButton(float x, float y, float width, float height, Action click, int layer, string textureName) : base(x, y, width, height, layer, textureName) {
			buttonAction = click;
		}
		public UIButton(float x, float y, float width, float height, int layer, string textureName) : base(x, y, width, height, layer, textureName) { }
		public UIButton(float x, float y, float width, float height, Action click) : base(x, y, width, height, 2, "button") {
			SetLayer(defaultLayer);
			buttonAction = click;
		}
		/// <summary>
		/// Checks if the x and y coordinates are in the button and clicks if so
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public virtual bool CheckClick(float x, float y) {
			if (!Visible)
				return false;
			bool check = CheckCoords(x, y);
			if (check) {
				if (buttonAction != null)
					buttonAction.Invoke();
			}
			return check;
		}
		/// <summary>
		/// Returns if the x and y coordinates are in the button
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public virtual bool CheckCoords(float x, float y) {
			if (!Visible)
				return false;
			return LogicUtils.Logic.CheckIntersection(arrayPosition, x, y);
		}

		public void SetButtonAction(Action btn) {
			buttonAction = btn;
		}

		public override void Render() {
			if (!Visible)
				return;
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
