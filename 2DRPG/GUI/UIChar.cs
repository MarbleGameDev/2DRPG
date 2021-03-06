﻿using System;
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
        public static int[] baseFontWidth = {6, 1, 3, 8, 5, 8, 5, 1, 3, 3,
                                      3, 5, 2, 4, 1, 4, 5, 3, 4, 4,
                                      5, 4, 5, 4, 5, 5, 1, 2, 3, 5,
                                      3, 4, 7, 6, 5, 5, 6, 5, 5, 5,
                                      6, 3, 4, 5, 4, 7, 5, 5, 5, 5,
                                      5, 5, 5, 6, 7, 7, 5, 5, 4, 3,
                                      4, 3, 5, 6, 2, 5, 4, 4, 4, 4,
                                      6, 4, 4, 1, 4, 4, 1, 7, 5, 4,
                                      4, 4, 3, 4, 3, 5, 5, 7, 4, 4,
                                      4, 3, 1, 3, 6};

		public UIChar(float x, float y, float size, int layer, char cha) : base(x, y, size, size, layer, TextureManager.TextureNames.baseFont) {
			charSize = size;
			displayChar = cha;
			NineSliceRendering = false;

			int ch = displayChar - 32;
			int col = ch % 10;
			int row = 12 - (ch / 10);
			float horizontal = 1 / 10f, vertical = 1 / 12f;

			if (ch > 95 || ch < 0) {
				texturePosition = new float[] {
					4/10f, 2/12f,
					4/10f, 3/12f,
					5/10f, 3/12f,
					5/10f, 2/12f
				};
			} else {
				texturePosition = new float[] {
					col * horizontal, vertical*(row - 1),
					col * horizontal, vertical*(row),
					(col + 1) * horizontal, vertical*(row),
					(col + 1) * horizontal, vertical*(row - 1)
				};
			}
			automaticTextureMapping = false;

		}

	}

}
