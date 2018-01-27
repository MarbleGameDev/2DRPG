using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI {
	class UIButton : UIBase {

		public Action buttonAction;
		/// <summary>
		/// Text displayed on the button
		/// </summary>
		public UIText displayLabel = null;

		public UIButton(Texture textureName) : base(textureName) { }

		public UIButton(Action click) : base(TextureManager.TextureNames.button) {
			buttonAction = click;
		}
		public UIButton(Action click, string labelText) : base(TextureManager.TextureNames.button) {
			buttonAction = click;
			displayLabel = new UIText(0, 0, 1f, 1, labelText);
		}
		public UIButton(Action click, string labelText, Texture textureName) : base(textureName) {
			buttonAction = click;
			displayLabel = new UIText(0, 0, .5f, 1, labelText);
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
		public UIButton(float x, float y, float width1, float height1, float width2, float height2, float width3, float height3, float width4, float height4, Action click, int layer, Texture textureName) : base(x, y, layer, textureName, width1, height1, width2, height2, width3, height3, width4, height4) {
			buttonAction = click;
		}
		public UIButton(float x, float y, float width, float height, int layer, Texture textureName) : base(x, y, layer, textureName, width, height, width, height, width, height, width, height) { }
		public UIButton(float x, float y, float width, float height, Action click) : base(x, y, 2, TextureManager.TextureNames.button, width, height, width, height, width, height, width, height) {
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
			return LogicUtils.Logic.CheckIntersection(quadPosition, x, y);
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
			if (displayLabel != null)
				displayLabel.SetScreenPosition(x, y + (int)(height1 / 8));
		}
		public override void SetScreenDimensions(float x, float y, float[] dim1, float[] dim2, float[] dim3, float[] dim4) {
			base.SetScreenDimensions(x, y, dim1, dim2, dim3, dim4);
			if (displayLabel != null)
				displayLabel.SetScreenPosition(x, y + (int)(height1 / 8));
		}

	}
}
