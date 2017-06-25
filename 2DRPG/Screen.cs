﻿using System;
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

		public static int screenHeight;
		public static int screenWidth;

		public static int pixelWidth = 496;
		public static int pixelHeight = 279;

		public static List<UIBase> UIObjects = new List<UIBase>();

		public static UIText worldText = new UIText(400f, 200f, 100f, 30f, "Coords: ");
		public static void ScreenStartup() {
			LoadGUITextures();
			//UIButton b = new UIButton(-1.7f, .8f, .1f, .1f, () => { System.Diagnostics.Debug.WriteLine("ayo"); });
			//UIObjects.Add(b);
			UIObjects.Add(new UIText(1f, 0f, .7f, .2f, "Test text"));
			UIObjects.Add(new UIDropdownButton(45f, 250f, 90f, 15f, "Dropdown", new UIButton[]{
				new UIButton(() => { System.Diagnostics.Debug.WriteLine("1 Pressed"); }, "Button 1"),
				new UIButton(() => { System.Diagnostics.Debug.WriteLine("2 Pressed"); }, "Button 2")
			}));
			UIObjects.Add(worldText);

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

		public static void LoadGUITextures() {
			TextureManager.LoadTexture("Sprites/Button.png", "Button");
			TextureManager.LoadTexture("Sprites/Heart.png", "Heart");
			TextureManager.LoadTexture("Sprites/CourierFont.png", "CourierFont");
			TextureManager.LoadTexture("Sprites/Default.png", "Default");
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
