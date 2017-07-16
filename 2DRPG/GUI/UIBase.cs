using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI {
	class UIBase : TexturedObject {
		protected int defaultLayer = 2;
		public bool Visible = true;

		public UIBase() : base() {
			SetScreenPosition(screenX, screenY, defaultLayer);
		}
		public UIBase(string textureName) : base(textureName) {
			SetScreenPosition(screenX, screenY, defaultLayer);
		}
		/// <summary>
		/// Complete Declaration for UIBase
		/// </summary>
		/// <param name="x">X position on the screen</param>
		/// <param name="y">Y position on the screen</param>
		/// <param name="width">Distance to the left and right</param>
		/// <param name="height">Distance to the top and bottom</param>
		/// <param name="layer">Render layer</param>
		/// <param name="textureName">Name of the texture</param>
		public UIBase(float x, float y, float width, float height, int layer, string textureName) : base(x, y, layer, textureName) {
			this.width = width;
			this.height = height;
			SetScreenPosition(x, y, layer);
		}
		/// <summary>
		/// Sets the position of the object on the screen
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public override void SetScreenPosition(float x, float y) {
			screenX = x;
			screenY = y;
			quadPosition[0] = (screenX - width);
			quadPosition[3] = (screenX - width);
			quadPosition[6] = (screenX + width);
			quadPosition[9] = (screenX + width);
			quadPosition[1] = (screenY - height);
			quadPosition[10] = (screenY - height);
			quadPosition[4] = (screenY + height);
			quadPosition[7] = (screenY + height);
		}
		public override void SetScreenPosition(float x, float y, int layer) {
			SetScreenPosition(x, y);
			SetLayer(layer);
		}

		public override void Render() {
			if (Visible)
				base.Render();
		}

		public virtual void Cleanup() {

		}
	}
}
