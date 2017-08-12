using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace _2DRPG.GUI {
	class UIDraggable : UIButton {

		public float mouseX, mouseY;

		public Action positionUpdate;

		/// <summary>
		/// Complete Declaration for UIDraggable
		/// </summary>
		/// <param name="x">X position on the screen</param>
		/// <param name="y">Y position on the screen</param>
		/// <param name="width">Distance to the left and right</param>
		/// <param name="height">Distance to the top and bottom</param>
		/// <param name="layer">Render layer</param>
		/// <param name="textureName">Name of the texture</param>
		public UIDraggable(float x, float y, float width, float height, int layer = 0, string textureName = "button") : base(x, y, width, height, layer, textureName) {
			buttonAction = () => { Form1.dragged = this; };
			positionUpdate = () => { SetScreenPosition(Input.MouseX + mouseX, Input.MouseY + mouseY); };
		}

		public override bool CheckClick(float x, float y) {
			mouseX = screenX - x;
			mouseY = screenY - y;
			return base.CheckClick(x, y);
		}
	}
}
