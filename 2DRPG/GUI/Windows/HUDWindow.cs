using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace _2DRPG.GUI.Windows {
	class HUDWindow : IWindow {

		static UIDropdownButton butt = new UIDropdownButton(-240, 140f, 60f, 15f, 2, "button", new UIText(-240f, 140f, .5f, 1, "Dropdown"), null);
		static UITextBox copypasta = new UITextBox(-180, 80, .5f, 200, 2, 4, "The quick brown cox jumped over the lazy doggo \n\nABCDEFGHIJKLMNOPQRSTUVWXYandZ\n\nabcdefghijklmnopqrstuvwxyandz\n\n!@#$%^&*()_-;'[]{}:<>,./?");
		static UIScrollBar scroll = new UIScrollBar(-100, 0, 5, 20, 2) { scrollTarget = copypasta };
		HashSet<UIBase> UIObjects = new HashSet<UIBase>() {
			//new UIText(100f, 80f, .5f, 2, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ") { textColor = Color.Black },
			butt,
			new UIBase(0f, 0f, .02f, .02f, 0, "default"),
			new UIDraggable(-100, -100, 15f, 15f, 2, "button"),
			copypasta, scroll
			
		};

		static HUDWindow() {
			butt.SetDropdowns(new UIButton[]{
				new UIButton(() => { butt.displayLabel.SetText("Button 1"); butt.ToggleDropdowns(); }, "Button 1"),
				new UIButton(() => { butt.displayLabel.SetText("Button 2"); butt.ToggleDropdowns(); }, "Button 2")
			});
		}

		public HashSet<UIBase> LoadObjects() {
			scroll.ScrollTo(0);
			//copypasta.scrollbar = scroll;
			return UIObjects;
		}
		public HashSet<UIBase> GetScreenObjects() {
			return UIObjects;
		}

		string[] textureNames = new string[] {
			"button", "lightBack", "darkBack"
		};

		public void LoadTextures() {
			TextureManager.RegisterTextures(textureNames);
		}

		public void UnloadTextures() {
			TextureManager.UnRegisterTextures(textureNames);
		}
	}
}
