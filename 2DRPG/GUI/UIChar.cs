using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;
using System.Drawing;

namespace _2DRPG.GUI {
	class UIChar : UIBase {

		char displayChar;
		float charSize;

		public UIChar(float x, float y, float size, char ch) : base(x, y, size, size, 1, "BaseFont") {
			charSize = size;
			displayChar = ch;
		}

		public override void Render() {
			int ch = displayChar - 32;
			int col = ch % 10;
			int row = 12 - (ch / 10);
            float horizontal = 1/10f, vertical = 1/12f;
			texturePosition = new float[] {
				col * horizontal, vertical*(row - 1),
                col * horizontal, vertical*(row),
				(col + 1) * horizontal, vertical*(row),
                (col + 1) * horizontal, vertical*(row - 1)
            };
			base.Render();
		}

	}
}
