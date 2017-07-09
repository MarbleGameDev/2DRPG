using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using _2DRPG.GUI;
using _2DRPG.GUI.Windows;
/// <summary>
/// Standards for Layers:
/// 5-10 world space
/// 2 - Default GUI
/// 1 - Default Text
/// 0 - Reserved for crucial overlays (pause menu)
/// </summary>
namespace _2DRPG {
	static class Screen {
		public static int WindowHeight;
		public static int WindowWidth;
		public static float windowRatio;

		public static int screenHeight;
		public static int screenWidth;
		public static int screenX;
		public static int screenY;

		public static int pixelWidth = 640;
		public static int pixelHeight = 360;

		public static event Action ResizeEvent;

		private static Dictionary<string, IWindow> windowFiles = new Dictionary<string, IWindow>();

		public static Dictionary<string, HashSet<UIBase>> currentWindows = new Dictionary<string, HashSet<UIBase>>();

		static Screen() {
			windowFiles.Add("pause", new PauseWindow());
			windowFiles.Add("hud", new HUDWindow());
			windowFiles.Add("console", new ConsoleWindow());
			windowFiles.Add("worldBuilder", new BuilderWindow());
		}

		public static void OpenWindow(string windowName) {
			if (windowFiles.ContainsKey(windowName) && !currentWindows.ContainsKey(windowName)) {
				windowFiles[windowName].LoadTextures();
				currentWindows.Add(windowName, windowFiles[windowName].LoadObjects());
			}
		}

		public static void CloseWindow(string windowName) {
			lock (currentWindows) {
				if (windowFiles.ContainsKey(windowName)) {
					windowFiles[windowName].UnloadTextures();
					foreach (UIBase b in windowFiles[windowName].GetScreenObjects())
						b.Cleanup();
					currentWindows.Remove(windowName);
				}
			}
		}

		public static void ScreenStartup() {
			OpenWindow("hud");
		}

		public static void SetWindowDimensions(int width, int height) {
			WindowHeight = height;
			WindowWidth = width;
			windowRatio = (float)width / height;

			if (ResizeEvent != null)
				ResizeEvent.Invoke();
		}
		public static void SetScreenDimensions(int x, int y, int width, int height) {
			screenHeight = height;
			screenWidth = width;
			screenX = x;
			screenY = y;
		}
		/// <summary>
		/// Returns a value from 0 to 1 based on the width of the screen
		/// </summary>
		/// <param name="xCoord"></param>
		/// <returns></returns>
		public static float PixeltoNormalizedWidth(float xCoord) {
			return (float)(xCoord + pixelWidth / 2) / pixelWidth;
		}
		/// <summary>
		/// Returns a value from 0 to 1 based on the height of the screen
		/// </summary>
		/// <param name="yCoord"></param>
		/// <returns></returns>
		public static float PixeltoNormalizedHeight(float yCoord) {
			return (float)(yCoord + pixelHeight / 2) / pixelHeight;
		}
	}
}
