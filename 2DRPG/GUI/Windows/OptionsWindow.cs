using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2DRPG.GUI.Windows {
	class OptionsWindow : IWindow {
		
		static UIDropdownButton windowSize = new UIDropdownButton(60, 50, 40, 10, 3, "button", new UIText(60, 51, .5f, 3, "1920 x 1080") { textColor = System.Drawing.Color.DarkBlue}, null) { displaySize = 6};
		static UIDropdownButton vSync = new UIDropdownButton(60, 30, 40, 10, 3, "button", new UIText(60, 31, .5f, 3, "True") { textColor = System.Drawing.Color.DarkBlue }, null) { displaySize = 2 };

		static UIDropdownButton controls = new UIDropdownButton(40, 30, 40, 10, 3, "button", new UIButton[] { }) { displaySize = 4, hideTop = true, showDrops = true, displayLabel = new UIText(0, 0, .5f, 1, "")};
		static UIDropdownButton controlNames = new UIDropdownButton(-40, 30, 40, 10, 3, "button", new UIButton[] { }) { displaySize = 4, hideTop = true, showDrops = true, displayLabel = new UIText(0, 0, .5f, 1, "")};

		static UITab tabs = new UITab(-30, 110, 30, 10, 3, new List<UIButton> {
			new UIButton("textBox"){ displayLabel = new UIText(-30, 112, .5f, 1, "Graphics") },
			new UIButton("textBox"){ displayLabel = new UIText(30, 112, .5f, 1, "Controls") }
		}, new HashSet<UIBase>[] {
			//Graphics
			new HashSet<UIBase>() {
				new UIText(-40, 50, .5f, 2, "Resolution:"), windowSize,
				new UIText(-40, 30, .5f, 2, "V-Sync:"), vSync
			},
			//Controls
			new HashSet<UIBase>() {
				new UIText(0, 40, .5f, 2, "Key Bindings:"), controls, controlNames
			}
		});


		static HashSet<UIBase> UIObjects = new HashSet<UIBase>() {
			new UIBase(0, 0, 120, 150, 4, "darkBack"),
			new UIButton(110, 140, 8, 8, () => { Screen.CloseWindow("options"); },1, "exit"),
			new UIText(0, 140, 1f, 2, "Options"),
			tabs
		};

		public HashSet<UIBase> GetScreenObjects() {
			return UIObjects;
		}

		public HashSet<UIBase> LoadObjects() {

			//Graphics Options
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

			//Controls Options
			List<UIButton> names = new List<UIButton>();
			List<UIButton> values = new List<UIButton>();
			foreach (KeyValuePair<Input.KeyInputs, Keys> pair in SaveData.GameSettings.keys) {
				if (pair.Key != Input.KeyInputs.console && pair.Key != Input.KeyInputs.escape) {
					names.Add(new UIButton(() => { }, pair.Key.ToString()));
					UIButton ba = new UIButton(() => { }, pair.Value.ToString());
					ba.SetButtonAction(() => {
						Input.RedirectKeys = true;
						ba.displayLabel.SetText("_");
						Input.DirectKeyCode += (Keys k) => {
							Input.ClearDirectKeyCode();
							ba.displayLabel.SetText(k.ToString(), .5f);
							SaveData.GameSettings.keys[pair.Key] = k;
						};
					});
					values.Add(ba);
				}
			}
			controls.SetDropdowns(values.ToArray());
			controlNames.SetDropdowns(names.ToArray());
			controls.scrollbar = controlNames;
			controlNames.scrollbar = controls;
			return UIObjects;
		}

		string[] textures = new string[] { "button", "darkBack", "textBox", "exit" };

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
