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
			UIObjects.Add(new UIText(1f, 0f, 1f, .2f, "0123456789") { textColor = Color.Black });
			UIObjects.Add(new UIDropdownButton(-1.4f, .8f, .3f, .1f, "Dropdown", new UIButton[]{
				new UIButton(() => { System.Diagnostics.Debug.WriteLine("1 Pressed"); }, "Button 1"),
				new UIButton(() => { System.Diagnostics.Debug.WriteLine("2 Pressed"); }, "Button 2")
			}));
			UIObjects.Add(new UIBase(0f, 0f, .02f, .02f, 0, "default"));
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
