using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI.Windows {
	class BuilderWindow : IWindow {

		public HashSet<UIBase> screenObjects = new HashSet<UIBase>() {
			new UIBase(220, 0, 100, 180, 3, "button"),
			new UIButton(300, 160, 10, 10, () => { Screen.CloseWindow("worldBuilder"); },1, "button"){ displayLabel = new UIText(310, 170, 1f, 0, "X") }
		};

		public ref HashSet<UIBase> LoadObjects() {
			return ref screenObjects;
		}

		public string[] textures = new string[] {
			"button"
		};

		public void LoadTextures() {
			TextureManager.RegisterTextures(textures);
			Screen.CloseWindow("hud");
		}

		public void UnloadTextures() {
			TextureManager.UnRegisterTextures(textures);
			Screen.AddWindow("hud");
		}
	}
}
