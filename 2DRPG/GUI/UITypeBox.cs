using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2DRPG.GUI {
	class UITypeBox : UIButton {

		/// <summary>
		/// value of the type box
		/// </summary>
		public UITextBox text;

		private bool typingEnabled = false;
		private float maxRows;
		public bool showBackground = true;
		/// <summary>
		/// Action to be performed when the value is set
		/// </summary>
		public Action valueAction = null;

		/// <summary>
		/// Complete Declaration for UITextBox,
		/// Allows user input and typing
		/// </summary>
		/// <param name="x">X position on the screen</param>
		/// <param name="y">Y position on the screen</param>
		/// <param name="width">Distance to the left and right</param>
		/// <param name="height">Distance to the top and bottom</param>
		/// <param name="layer">Render layer</param>
		/// <param name="maxRows">Maximum number of rows enterable</param>
		/// <param name="textureName">Name of the texture</param>
		public UITypeBox(float x, float y, float width, float height, int layer, float maxRows, string textureName) : base(x, y, width, height, layer, textureName) {
			text = new UITextBox(x, y, .5f, (int)width * 2, layer - 1, maxRows, "");
			this.maxRows = maxRows;
			buttonAction = new Action(StartTyping);
		}


		public void StartTyping() {
			typingEnabled = !typingEnabled;
			if (typingEnabled) {
				lock (Screen.currentWindows) {
					//TODO: clean up this garbage
					foreach (HashSet<UIBase> b in Screen.currentWindows.Values)
						foreach (UIBase u in b) {
							if (u is UITypeBox box) {
								if (box != this)
									box.DisableTyping();
							}
							if (u is UIDropdownButton bex)
								foreach (UIButton but in bex.drops) {
									if (but is UITypeBox bax)
										if (bax != this)
											bax.DisableTyping();
								}
						}
				}
				Input.RedirectKeys = true;
				Input.DirectCall += GetKey;
				text.SetText(text.GetText() + "|");
			} else {
				Input.RedirectKeys = false;
				Input.DirectCall -= GetKey;
				text.SetText(text.GetText().Remove(text.GetText().Length - 1, 1));
				UpdatePublicVar();
			}
		}

		public void DisableTyping() {
			if (typingEnabled) {
				typingEnabled = false;
				Input.RedirectKeys = false;
				Input.DirectCall -= GetKey;
				text.SetText(text.GetText().Remove(text.GetText().Length - 1, 1));
			}
		}

		public override void Render() {
			if (!Visible)
				return;
			if (showBackground)
				base.Render();
			text.Render();

		}
		public override void SetScreenPosition(float x, float y) {
			base.SetScreenPosition(x, y);
			if (text != null)
				text.SetScreenPosition(x, y);
		}

		void UpdatePublicVar() {
			if (valueAction != null)
				valueAction.Invoke();
		}

		void GetKey(char c) {
			if (c.Equals((char)Keys.Enter)) {
				StartTyping();
			} else if (c.Equals((char)Keys.Back)) {
				if (text.GetText().Length > 1) {
					if (!text.GetText().EndsWith(":|") && !text.GetText().EndsWith("`|")) {
						text.SetText(text.GetText().Remove(text.GetText().Length - 2, 2) + "|");
					}
				}
			} else {
				text.SetText(text.GetText().Remove(text.GetText().Length - 1, 1) + c + "|");
				if (text.rows + 1 > maxRows) {
					GetKey((char)Keys.Back);
				}
			}
		}

		public override void Cleanup() {
			if (typingEnabled) {
				Input.RedirectKeys = false;
				Input.DirectCall -= GetKey;
			}
			text.Cleanup();
		}

	}
}
