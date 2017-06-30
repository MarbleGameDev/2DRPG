using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;
using System.Drawing;

namespace _2DRPG.GUI {
	class UIText : UIBase {

		private string displayText;
		private List<UIChar> chars = new List<UIChar>();
		public Color textColor = Color.White;
		public float textSize;

		/// <summary>
		/// TextSize is multiplied by 16 to get the number of screen pixels each char is sized
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="textSize"></param>
		/// <param name="text"></param>
		public UIText(float x, float y, float textSize, string text) : base(x, y, textSize * 16 * text.Length, textSize * 16, 1, "button") {
			displayText = text;
			this.textSize = textSize;
			SetupChars();
		}

		public override void Render() {
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
		public void SetTextSize(float textSize) {
			this.textSize = textSize;
			width = textSize * 16 * displayText.Length;
			height = textSize * 16;
			SetupChars();
		}

		private void SetupChars() {
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
					chars.Add(new UIChar(startX + counter++ * charSize, startY, charSize, c));
				}
			}
		}

		public override void SetScreenPosition(float x, float y) {
			base.SetScreenPosition(x, y);
			SetupChars();
		}

	}
}
