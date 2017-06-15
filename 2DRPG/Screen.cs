using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG {
	public static class Screen {
		public static int WindowHeight;
		public static int WindowWidth;
		public static float windowRatio;
		public static void SetWindowDimensions(int width, int height) {
			WindowHeight = height;
			WindowWidth = width;
			windowRatio = (float)width / height;
		}
	}
}
