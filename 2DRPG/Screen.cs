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
		/// <summary>
		/// Converts the conventional -1 to 1 width coordinates for screen space into the correct values for the orthographic projection
		/// Height is the same
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static float ConvertWidth(float value) {
			return value *= Screen.windowRatio;
		}
	}
}
