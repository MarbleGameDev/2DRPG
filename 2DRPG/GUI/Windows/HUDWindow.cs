using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace _2DRPG.GUI.Windows {
	class HUDWindow : IWindow {

		static UIDropdownButton butt = new UIDropdownButton(-240, 140f, 60f, 15f, new UIText(-240f, 140f, .5f, 1, "Dropdown"), null);
		HashSet<UIBase> UIObjects = new HashSet<UIBase>() {
			new UIText(100f, 80f, .5f, 2, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ") { textColor = Color.Black },
			butt,
			new UIBase(0f, 0f, .02f, .02f, 0, "default"),
			new UIDraggable(-100, -100, 15f, 15f, 2, "button")

		};

		static HUDWindow() {
			butt.SetDropdowns(new UIButton[]{
				new UIButton(() => { butt.displayLabel.SetText("Button 1"); butt.ToggleDropdowns(); }, "Button 1"),
				new UIButton(() => { butt.displayLabel.SetText("Button 2"); butt.ToggleDropdowns(); }, "Button 2")
			});
		}

		public ref HashSet<UIBase> LoadObjects() {
			//UIObjects.Add(new UITextBox(-180, 80, .5f, 90, "What the fuck did you just fucking say about me, you little bitch? I'll have you know I graduated top of my class in the \nNavy Seals and I've been involved in numerous secret raids on Al-Quaeda, and I have over 300 confirmed kills."));
			//UIObjects.Add(new UITextBox(-80, 80, 1f, 40, "Testing Testing"));
			return ref UIObjects;
		}

		string[] textureNames = new string[] {
			"button"
		};

		public void LoadTextures() {
			TextureManager.RegisterTextures(textureNames);
		}

		public void UnloadTextures() {
			TextureManager.UnRegisterTextures(textureNames);
		}
	}
}
