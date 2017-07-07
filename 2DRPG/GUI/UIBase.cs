using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI {
	class UIBase : TexturedObject {
		public float width = .1f;
		public float height = .1f;
		protected int defaultLayer = 2;
		public bool Visible = true;

		public UIBase() : base() {
			SetScreenPosition(screenX, screenY, defaultLayer);
		}
		public UIBase(string textureName) : base(textureName) {
			SetScreenPosition(screenX, screenY, defaultLayer);
		}
		public UIBase(float x, float y, float width, float height, int layer, string textureName) : base(x, y, layer, textureName) {
			this.width = width;
			this.height = height;
			SetScreenPosition(x, y, layer);
		}

		public override void SetScreenPosition(float x, float y) {
			screenX = x;
			screenY = y;
			arrayPosition[0] = (screenX - width);
			arrayPosition[3] = (screenX - width);
			arrayPosition[6] = (screenX + width);
			arrayPosition[9] = (screenX + width);
			arrayPosition[1] = (screenY - height);
			arrayPosition[10] = (screenY - height);
			arrayPosition[4] = (screenY + height);
			arrayPosition[7] = (screenY + height);
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
