using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI {
	class UIText : UIBase {

		private string displayText;
		private List<UIChar> chars = new List<UIChar>();

		public UIText(float x, float y, float width, float height, string text) : base(x, y, width, height, 1, "Button") {
			displayText = text;
			SetupChars();
		}

		public override void Render() {
			base.Render();
			UIChar[] renderC = chars.ToArray();
			foreach (UIChar c in renderC)
				c.Render();
			
		}

		public void SetText(string text) {
			displayText = text;
			SetupChars();
		}

		private void SetupChars() {
			if (displayText == null)
				return;
			chars.Clear();
			char[] characters = displayText.ToCharArray();
			float charSize, startX, startY;
			if (width / characters.Length < height) {
				charSize = width / characters.Length;
				startX = screenX - width/4;
				startY = screenY + (height - charSize) / 2 - height/3;
			} else {
				charSize = height;
				startY = screenY - height/3;
				startX = screenX + (width - characters.Length * charSize) / 2 - width/2;
			}
			int counter = 0;
			foreach (char c in characters) {
				chars.Add(new UIChar(startX + counter++ * charSize, startY, charSize, c));
			}
		}

		public override void SetScreenPosition(float x, float y) {
			base.SetScreenPosition(x, y);
			SetupChars();
		}

	}
}
