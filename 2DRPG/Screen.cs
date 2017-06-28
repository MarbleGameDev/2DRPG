using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using _2DRPG.GUI;
using _2DRPG.GUI.Windows;

namespace _2DRPG {
	static class Screen {
		public static int WindowHeight;
		public static int WindowWidth;
		public static float windowRatio;

		public static int screenHeight;
		public static int screenWidth;

		public static int pixelWidth = 496;
		public static int pixelHeight = 279;

		private static Dictionary<string, IWindow> windowFiles = new Dictionary<string, IWindow>();

		public static Dictionary<string, HashSet<UIBase>> currentWindows = new Dictionary<string, HashSet<UIBase>>();

		static Screen() {
			windowFiles.Add("pause", new PauseWindow());
			windowFiles.Add("hud", new HUDWindow());
		}

		public static void AddWindow(string windowName) {
			if (windowFiles.ContainsKey(windowName)) {
				windowFiles[windowName].LoadTextures();
				currentWindows.Add(windowName, windowFiles[windowName].LoadObjects());
			}
		}

		public static void CloseWindow(string windowName) {
			lock (currentWindows) {
				if (windowFiles.ContainsKey(windowName)) {
					windowFiles[windowName].UnloadTextures();
					currentWindows.Remove(windowName);
				}
			}
		}

		public static void ScreenStartup() {
			LoadGUITextures();
			AddWindow("hud");
		}

		public static void SetWindowDimensions(int width, int height) {
			WindowHeight = height;
			WindowWidth = width;
			windowRatio = (float)width / height;
		}
		public static void SetScreenDimensions(int width, int height) {
			screenHeight = height;
			screenWidth = width;
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

		private static string[] textureNames = new string[] {
			"button", "baseFont", "default"
		};

		public static void LoadGUITextures() {
			TextureManager.RegisterTextures(textureNames);
		}
		/// <summary>
		/// Takes a value either [0, 16/9] for width or [0, 1] for height and returns the pixel number
		/// </summary>
		/// <returns></returns>
		public static int NormalizedToScreen(float normalizedCoord) {
			return (int)(normalizedCoord * 279);
		}
	}
}
