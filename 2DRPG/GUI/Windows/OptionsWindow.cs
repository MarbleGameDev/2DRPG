using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI.Windows {
	class OptionsWindow : IWindow {

		static UIDropdownButton fullscreen = new UIDropdownButton(60, 80, 40, 10, 2, "button", new UIText(60, 80, .5f, 1, "false"), null);

		HashSet<UIBase> UIObjects = new HashSet<UIBase>() {
			new UIBase(0, 0, 120, 150, 4, "darkBack"),
			new UIText(0, 140, 1f, 2, "Options"),
			new UIText(-40, 80, .5f, 2, "FullScreen:"), fullscreen
		};

		public HashSet<UIBase> GetScreenObjects() {
			return UIObjects;
		}

		public HashSet<UIBase> LoadObjects() {
			fullscreen.SetDropdowns(new UIButton[] {
				new UIButton(() => {
					fullscreen.displayLabel.SetText("True");
					fullscreen.ToggleDropdowns();
					SaveData.GameSettings.fullScreen = true;
				}){ displayLabel = new UIText(0, 0, .5f, 1, "True")},
				new UIButton(() => {
					fullscreen.displayLabel.SetText("False");
					fullscreen.ToggleDropdowns();
					SaveData.GameSettings.fullScreen = false;
				}){ displayLabel = new UIText(0, 0, .5f, 1, "False")}
			});
			fullscreen.displayLabel.SetText(SaveData.GameSettings.fullScreen.ToString());
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
