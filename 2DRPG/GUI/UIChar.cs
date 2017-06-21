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

		public UIChar(float x, float y, float size, char ch) : base(x, y, size, size, 1, "CourierFont") {
			charSize = size;
			displayChar = ch;
		}

		public override void Render() {
			int ch = displayChar;
			int col = displayChar % 16;
			int row = displayChar / 16;
			texturePosition = new float[] {
				col / 16f, (15 - row) / 16f,
				col / 16f, (16 - row) / 16f,
				(col + 1) / 16f, (16 - row) / 16f,
				(col + 1) / 16f, (15 - row) / 16f
			};
			base.Render();
		}

	}
}
