using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI {
	class UIBase : TexturedObject {
		public float width = .1f;
		public float height = .1f;
		protected int defaultLayer = 1;
		public UIBase() : base() {
			SetScreenPosition(screenX, screenY, defaultLayer);
		}
		public UIBase(string textureName) : base(textureName) {
			SetScreenPosition(screenX, screenY, defaultLayer);
		}
		public UIBase(float x, float y, float width, float height, int layer, string textureName) : base(x, y, layer, textureName) {
			this.width = width;
			this.height = height;
			SetScreenPosition(x, y, defaultLayer);
		}

		public override void SetScreenPosition(float x, float y) {
			arrayPosition[0] = x - width;
			arrayPosition[3] = x - width;
			arrayPosition[6] = x + width;
			arrayPosition[9] = x + width;
			arrayPosition[1] = y - height;
			arrayPosition[10] = y - height;
			arrayPosition[4] = y + height;
			arrayPosition[7] = y + height;
		}
		public override void SetScreenPosition(float x, float y, int layer) {
			SetScreenPosition(x, y);
			SetLayer(layer);
		}
	}
}
