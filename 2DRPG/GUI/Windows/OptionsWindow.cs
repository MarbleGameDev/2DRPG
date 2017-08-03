using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI.Windows {
	class OptionsWindow : IWindow {
		
		static UIDropdownButton windowSize = new UIDropdownButton(60, 50, 40, 10, 3, "button", new UIText(60, 50, .5f, 3, "1920 x 1080") { textColor = System.Drawing.Color.DarkBlue}, null) { displaySize = 6};
		static UIDropdownButton vSync = new UIDropdownButton(60, 30, 40, 10, 3, "button", new UIText(60, 30, .5f, 3, "True") { textColor = System.Drawing.Color.DarkBlue }, null) { displaySize = 2 };

		HashSet<UIBase> UIObjects = new HashSet<UIBase>() {
			new UIBase(0, 0, 120, 150, 4, "darkBack"),
			new UIButton(110, 140, 10, 10, () => { Screen.CloseWindow("options"); },1, "button"){ displayLabel = new UIText(117, 143, 1f, 0, "X") },
			new UIText(0, 140, 1f, 2, "Options"),
			new UIText(-40, 50, .5f, 2, "Resolution:"), windowSize,
			new UIText(-40, 30, .5f, 2, "V-Sync:"), vSync
		};

		public HashSet<UIBase> GetScreenObjects() {
			return UIObjects;
		}

		public HashSet<UIBase> LoadObjects() {
			windowSize.SetDropdowns(new UIButton[] {
				new UIButton(() => {
					windowSize.displayLabel.SetText("640 x 360");
					windowSize.ToggleDropdowns();
					Form1.ResizeWindow(640, 360);
				}) { displayLabel = new UIText(0, 0, .5f, 1, "640 x 360")},
				new UIButton(() => {
					windowSize.displayLabel.SetText("1280 x 720");
					windowSize.ToggleDropdowns();
					Form1.ResizeWindow(1280, 720);
				}) { displayLabel = new UIText(0, 0, .5f, 1, "1280 x 720")},
				new UIButton(() => {
					windowSize.displayLabel.SetText("Fullscreen");
					windowSize.ToggleDropdowns();
					Form1.SetFullscreen();
				}) { displayLabel = new UIText(0, 0, .5f, 1, "Fullscreen")}
			});
			windowSize.displayLabel.SetText(SaveData.GameSettings.windowx + " x " + SaveData.GameSettings.windowy);
			vSync.SetDropdowns(new UIButton[] {
				new UIButton(() => {
					vSync.displayLabel.SetText("True");
					vSync.ToggleDropdowns();
					OpenGL.Wgl.SwapIntervalEXT(-1);
					SaveData.GameSettings.VSync = true;
				}){ displayLabel = new UIText(0, 0, .5f, 1, "True")},
				new UIButton(() => {
					vSync.displayLabel.SetText("False");
					vSync.ToggleDropdowns();
					OpenGL.Wgl.SwapIntervalEXT(0);
					SaveData.GameSettings.VSync = false;
				}){ displayLabel = new UIText(0, 0, .5f, 1, "False")}
			});
			vSync.displayLabel.SetText(SaveData.GameSettings.VSync ? ("True") : ("False"));
			return UIObjects;
		}

		string[] textures = new string[] { "button", "darkBack" };

		public void LoadTextures() {
			TextureManager.RegisterTextures(textures);
			Screen.CloseWindow("hud");
			Screen.WindowOpen = true;
		}

		public void UnloadTextures() {
			TextureManager.UnRegisterTextures(textures);
			Screen.OpenWindow("hud");
			Screen.WindowOpen = false;
			SaveData.SerializeObject(SaveData.GameSettings, "Settings");
		}
	}
}
