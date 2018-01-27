using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace _2DRPG.GUI.Windows {
	class HUDWindow : IWindow {

		static UIDropdownButton butt = new UIDropdownButton(-240, 140f, 60f, 15f, 4, TextureManager.TextureNames.textBox, new UIText(-240f, 142f, .5f, 3, "Dropdown"), null);
		static UITextBox copypasta = new UITextBox(-180, 80, .5f, 200, 3, 3, "The quick brown cox jumped over the lazy doggo \n\nABCDEFGHIJKLMNOPQRSTUVWXYandZ\n\nabcdefghijklmnopqrstuvwxyandz\n\n!@#$%^&*()_-;'[]{}:<>,./?");
		static UIScrollBar scroll = new UIScrollBar(-100, 0, 5, 20, 2) { scrollTarget = copypasta };
		HashSet<UIBase> UIObjects = new HashSet<UIBase>() {
			butt,
			new UIDraggable(-100, -100, 15f, 15f, 2, TextureManager.TextureNames.textBox),
			copypasta, scroll
			
		};

		static HUDWindow() {
			butt.SetDropdowns(new UIButton[]{
				new UIButton(() => { butt.displayLabel.SetText("Button 1"); butt.ToggleDropdowns(); }, "Emoji crying fa", TextureManager.TextureNames.textBox),
				new UIButton(() => { butt.displayLabel.SetText("Button 2"); butt.ToggleDropdowns(); }, "Click me", TextureManager.TextureNames.textBox)
			});
		}

		public HashSet<UIBase> LoadObjects() {
			scroll.ScrollTo(0);
			copypasta.scrollbar = scroll;
			return UIObjects;
		}
		public HashSet<UIBase> GetScreenObjects() {
			return UIObjects;
		}

		Texture[] textureNames = new Texture[] {
			TextureManager.TextureNames.button, TextureManager.TextureNames.lightBack, TextureManager.TextureNames.darkBack, TextureManager.TextureNames.textBox, TextureManager.TextureNames.baseFont
		};

		public void LoadTextures() {
			TextureManager.RegisterTextures(textureNames);
		}

		public void UnloadTextures() {
			TextureManager.UnRegisterTextures(textureNames);
		}
	}
}
