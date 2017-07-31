using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2DRPG.GUI {
	class UITypeBox : UIButton, IScrollable {

		/// <summary>
		/// value of the type box
		/// </summary>
		public UITextBox text;

		private bool typingEnabled = false;
		private float maxRows;
		public bool showBackground = true;
		public bool LimitTyping = true;
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
		/// <param name="maxRows">Maximum number of rows enterable, or scrollable if LimitTyping is set to false</param>
		/// <param name="textureName">Name of the texture</param>
		public UITypeBox(float x, float y, float width, float height, int layer, float maxRows, string textureName) : base(x, y, width, height, layer, textureName) {
			text = new UITextBox(x, y + height / 1.5f, .5f, (int)width * 2, layer - 1, maxRows, "");
			this.maxRows = maxRows;
			buttonAction = new Action(StartTyping);
			Screen.SelectionEvent += DisableTyping;
		}


		public void StartTyping() {
			typingEnabled = !typingEnabled;
			if (typingEnabled) {
				Screen.SelectionEvent -= DisableTyping;
				Screen.InvokeSelection();
				Screen.SelectionEvent += DisableTyping;
				Input.RedirectKeys = true;
				Input.DirectCall += GetKey;
				text.SetText(text.GetText() + "|");
				text.ScrollTo(1f);
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
				text.SetScreenPosition(x, y + height / 1.5f);
		}

		public void UpdatePublicVar() {
			if (valueAction != null)
				valueAction.Invoke();
		}

		void GetKey(char c) {
			if (c.Equals(':'))
				return;
			if (c.Equals((char)Keys.Enter)) {
				StartTyping();
			} else if (c.Equals((char)Keys.Back)) {
				if (text.GetText().Length > 1) {
					if (!text.GetText().EndsWith(":|") && !text.GetText().EndsWith("`|")) {
						text.SetText(text.GetText().Remove(text.GetText().Length - 2, 2) + "|");
						text.ScrollTo(1f);
					}
				}
			} else {
				text.SetText(text.GetText().Remove(text.GetText().Length - 1, 1) + c + "|");
				if (LimitTyping && text.rows + 1 > maxRows) {
					GetKey((char)Keys.Back);
				}
				text.ScrollTo(1f);
			}
		}

		public override void Cleanup() {
			if (typingEnabled) {
				Input.RedirectKeys = false;
				Input.DirectCall -= GetKey;
				typingEnabled = false;
			}
			text.Cleanup();
			Screen.SelectionEvent -= DisableTyping;
		}

		public void ScrollWheel(int dir) {
			text.ScrollWheel(dir);
		}

		public void ScrollTo(float amount) {
			text.ScrollTo(amount);
		}
	}
}
