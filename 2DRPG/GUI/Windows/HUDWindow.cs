using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace _2DRPG.GUI.Windows {
	class HUDWindow : IWindow {

		private HashSet<UIBase> UIObjects = new HashSet<UIBase>();

		public HashSet<UIBase> LoadObjects() {
			UIObjects.Clear();
			UIObjects.Add(new UIText(100f, 80f, .5f, "0123456789") { textColor = Color.Black });
			UIObjects.Add(new UIDropdownButton(-240, 140f, 60f, 15f, new UIText(-240f, 140f, .5f, "Dropdown"), new UIButton[]{
				new UIButton(() => { System.Diagnostics.Debug.WriteLine("1 Pressed"); }, "Button 1"),
				new UIButton(() => { System.Diagnostics.Debug.WriteLine("2 Pressed"); }, "Button 2")
			}));
			UIObjects.Add(new UIBase(0f, 0f, .02f, .02f, 0, "default"));
			UIObjects.Add(new UIDraggable(-100, -100, 15f, 15f, 0, "button"));
			//UIObjects.Add(new UITextBox(-180, 80, .5f, 90, "What the fuck did you just fucking say about me, you little bitch? I'll have you know I graduated top of my class in the \nNavy Seals and I've been involved in numerous secret raids on Al-Quaeda, and I have over 300 confirmed kills."));
			//UIObjects.Add(new UITextBox(-80, 80, 1f, 40, "Testing Testing"));
			return UIObjects;
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
