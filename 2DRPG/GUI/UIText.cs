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
		public Color textColor = Color.White;
		public float textSize;

		/// <summary>
		/// UIText with a default width of the length of the text string
		/// </summary>
		/// <param name="x">X position on the screen</param>
		/// <param name="y">Y position on the screen</param>
		/// <param name="textSize">size of the text, multipled by 16 to get pixel counts</param>
		/// <param name="text">text to be displayed</param>
		public UIText(float x, float y, float textSize, int layer, string text) : base(x, y, textSize * 16 * text.Length, textSize * 16, layer, "button") {
			displayText = text;
			this.textSize = textSize;
			SetupChars();
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
		public UIText(float x, float y, float textSize, int charWidth, int layer, string text) : base(x, y, charWidth, textSize * 16, layer, "button") {
			displayText = text;
			this.textSize = textSize;
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

		public void SetText(string text, float textSize) {
			displayText = text;
			SetTextSize(textSize);
			SetupChars();
		}
		public void SetText(string text) {
			displayText = text;
			SetupChars();
		}
		public void SetTextSize(float textSize) {
			this.textSize = textSize;
			width = textSize * 16 * displayText.Length;
			height = textSize * 16;
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
				if (width / characters.Length < height) {
					charSize = width / characters.Length;
					startX = screenX - width / 2;
					startY = screenY + (height - charSize) / 2 - height / 3;
				} else {
					charSize = height;
					startY = screenY - height / 3;
					startX = screenX + (width - characters.Length * charSize) / 2 - width / 2;
				}
				int counter = 0;
				foreach (char c in characters) {
					chars.Add(new UIChar(startX + counter++ * charSize, startY, charSize, layer, c));
				}
			}
		}

		public override void SetScreenPosition(float x, float y) {
			base.SetScreenPosition(x, y);
			SetupChars();
		}

	}
}
