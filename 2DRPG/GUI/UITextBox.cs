using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI {
	class UITextBox : UIText {

		private int linespacing = 20;
		private int charWidth;

		public UITextBox(float x, float y, float textSize, int charWidth, string text) : base(x, y, textSize, charWidth, text) {
			this.charWidth = charWidth;
			SetupChars();
		}

		protected override void SetupChars() {
			if (displayText == null)
				return;
			lock (chars) {
				string[] words = displayText.Split(' ');
				
				float startX = screenX - width / 2;
				float startY = screenY;
				int row = 0, col = 0;
				foreach (string word in words) {
					char[] characters = (word + " ").ToCharArray();
					int summer = 0;
					foreach (char c in characters) {
						summer += UIChar.baseFontWidth[c - 32];
					}
					if (charWidth < col + summer) {
						col = 0;
						row++;
					}
					foreach (char c in characters) {
						int chSpacing = UIChar.baseFontWidth[c - 32] + 2;
						chars.Add(new UIChar(startX + col + chSpacing, startY - row * textSize * linespacing, textSize * 16, c));
						col += chSpacing;
					}
				}
			}
		}
	}
}
