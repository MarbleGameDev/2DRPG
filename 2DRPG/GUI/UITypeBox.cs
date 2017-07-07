using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2DRPG.GUI {
	class UITypeBox : UIButton {

		public UITextBox text;

		private bool typingEnabled = false;

		public Action valueAction = null;

		public UITypeBox(float x, float y, float width, float height, int layer, string textureName) : base(x, y, width, height, layer, textureName) {
			text = new UITextBox(x, y, .5f, (int)width * 2, layer - 1, "");
			buttonAction = new Action(StartTyping);
		}


		void StartTyping() {
			typingEnabled = !typingEnabled;
			if (typingEnabled) {
				lock (Screen.currentWindows) {
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
				Input.RedirectKeys = false;
				Input.DirectCall -= GetKey;
				text.SetText(text.GetText().Remove(text.GetText().Length - 1, 1));
				UpdatePublicVar();
				typingEnabled = false;
			}
		}

		public override void Render() {
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
					if (!text.GetText().EndsWith(":|")) {
						text.SetText(text.GetText().Remove(text.GetText().Length - 2, 2) + "|");
					}
				}
			} else {
				text.SetText(text.GetText().Remove(text.GetText().Length - 1, 1) + c + "|");
			}
		}

		public override void Cleanup() {
			if (typingEnabled) {
				Input.RedirectKeys = false;
				Input.DirectCall -= GetKey;
			}
		}

	}
}
