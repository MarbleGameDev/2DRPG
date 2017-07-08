using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI {
	class UITextBox : UIText {

		private int linespacing = 20;
		private int charWidth;
		/// <summary>
		/// Complete Declaration for UITextBox,
		/// Improves on UIText by allowing new lines and word wrapping
		/// </summary>
		/// <param name="x">X position on the screen</param>
		/// <param name="y">Y position on the screen</param>
		/// <param name="textSize">size of the text, multipled by 16 to get pixel counts</param>
		/// <param name="charWidth">width of the text</param>
		/// <param name="layer">Render layer</param>
		/// <param name="text">text to be displayed</param>
		public UITextBox(float x, float y, float textSize, int charWidth, int layer, string text) : base(x, y, textSize, charWidth, layer, text) {
			this.charWidth = charWidth;
			SetupChars();
			SetLayer(1);
		}
		/// <summary>
		/// Sets the text to be displayed
		/// </summary>
		/// <param name="text"></param>
		public new void SetText(string text) {
			displayText = text;
			SetupChars();
		}

		protected override void SetupChars() {
			if (displayText == null)
				return;
			lock (chars) {
				chars.Clear();
				string[] words = displayText.Split(' ');
				
				float startX = screenX - width / 2;
				float startY = screenY;
				int row = 0, col = 0;
				foreach (string word in words) {
					char[] characters = (word + " ").ToCharArray();
					int summer = 0;
					foreach (char c in characters) {
						if (c != '\n')
							summer += UIChar.baseFontWidth[c - 32];
					}
					if (charWidth < col + summer) {
						col = 0;
						row++;
					}
					foreach (char c in characters) {
						if (c == '\n') {
							col = 0;
							row++;
						} else {
							int chSpacing = UIChar.baseFontWidth[c - 32] + 2;
							chars.Add(new UIChar(startX + col + chSpacing, startY - row * textSize * linespacing, textSize * 16, layer, c));
							col += chSpacing;
						}
					}
				}
			}
		}
	}
}
