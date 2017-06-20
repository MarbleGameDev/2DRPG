using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.GUI;

namespace _2DRPG {
	static class Screen {
		public static int WindowHeight;
		public static int WindowWidth;
		public static float windowRatio;

		public static List<UIBase> UIObjects = new List<UIBase>();

		public static void ScreenStartup() {
			LoadGUITextures();
			UIButton b = new UIButton(-1.7f, .8f, .1f, .1f, () => { System.Diagnostics.Debug.WriteLine("ayo"); });
			UIObjects.Add(b);

		}

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
			return value * windowRatio;
		}

		public static void LoadGUITextures() {
			TextureManager.LoadTexture("Sprites/Button.png", "Button");
			TextureManager.LoadTexture("Sprites/Heart.png", "Heart");
		}
	}
}
