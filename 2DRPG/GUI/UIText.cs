﻿using System;
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

		public UIText(float x, float y, float width, float height, string text) : base(x, y, width, height, 1, "button") {
			displayText = text;
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

		public void SetText(string text) {
			displayText = text;
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
