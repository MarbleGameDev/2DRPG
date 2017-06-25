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
			screenX = x;
			screenY = y;
			arrayPosition[0] = x - width / 2 - .5f;
			arrayPosition[3] = x - width / 2 - .5f;
			arrayPosition[6] = x + width / 2 + .5f;
			arrayPosition[9] = x + width / 2 + .5f;
			arrayPosition[1] = y - height / 2 - .5f;
			arrayPosition[10] = y - height / 2 - .5f;
			arrayPosition[4] = y + height / 2 + .5f;
			arrayPosition[7] = y + height / 2 + .5f;
		}
		public override void SetScreenPosition(float x, float y, int layer) {
			SetScreenPosition(x, y);
			SetLayer(layer);
		}
	}
}
