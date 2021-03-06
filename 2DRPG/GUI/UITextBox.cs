﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;

namespace _2DRPG.GUI {
	class UITextBox : UIText, IScrollable {

		private int linespacing = 32;
		private int charWidth;


		public float displaySize = 2f;
		private int[] scissorMask = new int[4];
		private float scrollAmount = 0, scrollMax;
		public int rows;

		public IScrollable scrollbar = null;

		/// <summary>
		/// Complete Declaration for UITextBox,
		/// Improves on UIText by allowing new lines, scrolling, and word wrapping
		/// </summary>
		/// <param name="x">X position on the screen</param>
		/// <param name="y">Y position on the screen</param>
		/// <param name="textSize">size of the text, multipled by 16 to get pixel counts</param>
		/// <param name="charWidth">width of the text in pixels</param>
		/// <param name="layer">Render layer</param>
		/// <param name="displaySize">Number of rows displayed at one time</param>
		/// <param name="text">text to be displayed</param>
		public UITextBox(float x, float y, float textSize, int charWidth, int layer, float displaySize, string text) : base(x, y, textSize, charWidth / 2, layer, text) {
			this.charWidth = charWidth;
			this.displaySize = displaySize;
			SetText(text);
		}

		public bool CheckCoords(float x, float y) {
			if (!Visible)
				return false;
			float[] testCoords = new float[12];
			quadPosition.CopyTo(testCoords, 0);
			testCoords[1] = screenY - height3 - displaySize * linespacing * textSize;
			testCoords[10] = testCoords[1];
			return LogicUtils.Logic.CheckIntersection(testCoords, x, y);
		}

		private int scrollMod = 4;
		public void ScrollWheel(int y, object sender) {
			if (scrollbar != null) {
				scrollbar.ScrollWheel(y, this);
				return;
			}

			float temp = 0;
			if (y > 0) {
				temp = (scrollAmount >= scrollMod) ? -scrollMod : -(scrollAmount % scrollMod);
			} else if (y < 0) {
				temp = ((scrollMax - scrollAmount) >= scrollMod) ? scrollMod : ((scrollMax - scrollAmount) % scrollMod);
			}
			foreach (UIChar b in chars) {
				b.ShiftScreenPosition(0, temp);
			}
			scrollAmount += temp;
		}

		public void ScrollTo(float val) {
			if (rows < displaySize)
				return;
			float desiredScroll = val * scrollMax;
			desiredScroll = desiredScroll - scrollAmount;
			foreach (UIChar b in chars)
				b.ShiftScreenPosition(0, desiredScroll);
			scrollAmount += desiredScroll;
		}

		public bool CheckScrollWheel(float x, float y, int dir, object sender) {
			if (CheckCoords(x, y)) {
				ScrollWheel(dir, sender);
				return true;
			}
			return false;
		}

		private void SetScissorMask() {
			float maxHeight = (height1 > height2) ? height1 : height2;
			float minHeight = (height3 > height4) ? height3 : height4;
			float maxWidth = (width1 > width4) ? width1 : width4;
			float minWidth = (width2 > width3) ? width2 : width3;
			scissorMask[0] = (int)(Screen.PixeltoNormalizedWidth(screenX - minWidth) * Screen.screenWidth) + Screen.screenX;
			scissorMask[1] = (int)Math.Round(Screen.PixeltoNormalizedHeight(screenY - minHeight - displaySize * linespacing * textSize) * Screen.screenHeight) + Screen.screenY;
			scissorMask[2] = -(int)(Screen.PixeltoNormalizedWidth(screenX - minWidth) * Screen.screenWidth) + (int)(Screen.PixeltoNormalizedWidth(screenX + maxWidth + 2) * Screen.screenWidth);
			scissorMask[3] = (int)(Screen.PixeltoNormalizedHeight(screenY + minHeight) * Screen.screenHeight) - (int)(Screen.PixeltoNormalizedHeight(screenY - minHeight - displaySize * linespacing * textSize) * Screen.screenHeight);
		}

		/// <summary>
		/// Sets the text to be displayed
		/// </summary>
		/// <param name="text"></param>
		public override void SetText(string text) {
			displayText = text;
			scrollAmount = 0;
			SetupChars();
		}
		public override void SetText(string text, float textSize) {
			this.textSize = textSize;
			SetText(text);
		}

		public override void Render() {
			if (rows <= displaySize) {
				base.Render();
			} else {
				Gl.PushAttrib(AttribMask.ScissorBit);
				Gl.Scissor(scissorMask[0], scissorMask[1], scissorMask[2], scissorMask[3]);
				base.Render();
				Gl.PopAttrib();
			}
		}

		public override void SetScreenPosition(float x, float y) {
			base.SetScreenPosition(x, y);
			SetScissorMask();
		}

		protected override void SetupChars() {
			if (displayText == null)
				return;
			lock (chars) {
				chars.Clear();
				string[] words = displayText.Split(' ');
				
				float startX = screenX - width2;
				float startY = screenY;
                float widthA = 6, widthB = 0;
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
							widthB = UIChar.baseFontWidth[c - 32];
                            col += ((int)(widthA / 2 + .5) + (int)(widthB / 2)) + 1;
                            chars.Add(new UIChar(startX + col, startY - row * textSize * linespacing, textSize * 16, layer, c));
                            widthA = UIChar.baseFontWidth[c - 32];
                        }
					}
				}
				rows = row;
				scrollMax = (rows - displaySize) * linespacing * textSize;
			}
		}

		public override void Cleanup() {
			Screen.ResizeEvent -= SetScissorMask;
		}
		public override void Setup() {
			SetScissorMask();
			Screen.ResizeEvent += SetScissorMask;
		}
	}
}
