using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;
using System.Drawing;

namespace _2DRPG.GUI {
	class UIText : UIBase {

		protected string displayText;
		protected List<UIChar> chars = new List<UIChar>();
		public Color textColor = Color.Black;
		public float textSize;

		/// <summary>
		/// UIText with a default width of the length of the text string
		/// </summary>
		/// <param name="x">X position on the screen</param>
		/// <param name="y">Y position on the screen</param>
		/// <param name="textSize">size of the text, multipled by 16 to get pixel counts</param>
		/// <param name="text">text to be displayed</param>
		public UIText(float x, float y, float textSize, int layer, string text) : base(x, y, layer, TextureManager.TextureNames.button, textSize * 16 * text.Length, textSize * 16, textSize * 16 * text.Length, textSize * 16, textSize * 16 * text.Length, textSize * 16, textSize * 16 * text.Length, textSize * 16) {
			SetText(text, textSize);
		}
		/// <summary>
		/// Complete Declaration for UIText
		/// </summary>
		/// <param name="x">X position on the screen</param>
		/// <param name="y">Y position on the screen</param>
		/// <param name="textSize">size of the text, multipled by 16 to get pixel counts</param>
		/// <param name="charWidth">width of the text</param>
		/// <param name="layer">Render layer</param>
		/// <param name="text">text to be displayed</param>
		public UIText(float x, float y, float textSize, int charWidth, int layer, string text) : base(x, y, layer, TextureManager.TextureNames.button, charWidth, textSize * 16, charWidth, textSize * 16, charWidth, textSize * 16, charWidth, textSize * 16) {
			SetText(text, textSize);
		}

		public override void Render() {
			if (!Visible)
				return;
			Gl.Color3(textColor.R, textColor.G, textColor.B);
			lock (chars)
				foreach (UIChar c in chars) {
					if (c != null)
						c.Render();
				}
			Gl.Color3(1f, 1f, 1f);
		}

		public virtual void SetText(string text, float textSize) {
			displayText = text;
			SetTextSize(textSize);
			SetupChars();
		}
		public virtual void SetText(string text) {
			displayText = text;
			SetTextSize(textSize);
			SetupChars();
		}
		public void SetTextSize(float textSize) {
			this.textSize = textSize;
			width1 = textSize * 16 * displayText.Length;
			width2 = width1;
			width3 = width1;
			width4 = width1;
			height1 = textSize * 16;
			height2 = height1;
			height3 = height1;
			height4 = height1;
			SetupChars();
		}
		public string GetText() {
			return displayText;
		}

		protected virtual void SetupChars() {
			if (displayText == null)
				return;
			lock (chars) {
				chars.Clear();
				char[] characters = displayText.ToCharArray();
				float charSize, startX, startY;
				float widthA = 0, widthB = 0;
                int col = -2;
				if (width1 / characters.Length < height1) {
					charSize = width1 / characters.Length;
					startX = screenX - width1 / 2;
					startY = screenY + (height1 - charSize) / 2 - height1 / 4;
				} else {
					int count = 0;
					foreach (char c in characters) {
						count += (int)(UIChar.baseFontWidth[c - 32] * textSize * 2);
					}
					charSize = height1;
					startY = screenY - height1 / 4;
					startX = screenX - count / 2;
				}
				if (characters.Length > 0) {
					widthB = UIChar.baseFontWidth[characters[0] - 32] * textSize * 2;
					col -= ((int)(widthA / 2 + .5) + (int)(widthB / 2)) + (int)(2 * textSize);
				}
				foreach (char c in characters) {
                    widthB = UIChar.baseFontWidth[c - 32] * textSize * 2;
                    col += ((int)(widthA / 2 + .5) + (int)(widthB / 2)) + (int)(2 * textSize);
                    chars.Add(new UIChar(startX + col, startY, charSize, layer, c));
                    widthA = UIChar.baseFontWidth[c - 32] * textSize * 2;
                }
			}
		}

		public override void SetScreenPosition(float x, float y) {
			base.SetScreenPosition(x, y);
			SetupChars();
		}
	}
}
