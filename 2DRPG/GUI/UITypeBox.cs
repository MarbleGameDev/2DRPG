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
			text = new UITextBox(x, y, .5f, (int)width * 2, layer - 1, maxRows, "");
			this.maxRows = maxRows;
			buttonAction = new Action(StartTyping);
		}


		public void StartTyping() {
			typingEnabled = !typingEnabled;
			if (typingEnabled) {
				Screen.SelectionEvent -= DisableTyping;
				Screen.InvokeSelection();
				Screen.SelectionEvent += DisableTyping;
				Input.RedirectKeys = true;
				Input.DirectCall += GetKey;
				Input.DirectKeyCode += GetKeyCode;
				text.SetText(text.GetText() + "|");
				text.ScrollTo(1f);
			} else {
				Input.RedirectKeys = false;
				Input.DirectCall -= GetKey;
				Input.DirectKeyCode -= GetKeyCode;
				text.SetText(text.GetText().Replace("|", ""));
				Screen.SelectionEvent -= DisableTyping;
				UpdatePublicVar();
			}
		}

		public void DisableTyping() {
			if (typingEnabled) {
				typingEnabled = false;
				Input.RedirectKeys = false;
				Input.DirectCall -= GetKey;
				Input.DirectKeyCode -= GetKeyCode;
				text.SetText(text.GetText().Replace("|", ""));
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

		public void UpdatePublicVar() {
			if (valueAction != null)
				valueAction.Invoke();
		}

		void GetKey(char c) {
			if (c.Equals(':') || c.Equals('|'))
				return;
			if (c.Equals((char)Keys.Enter)) {
				StartTyping();
			} else if (c.Equals((char)Keys.Back)) {
				int index = text.GetText().IndexOf('|');
				if (index > 0 && !text.GetText().ElementAt(index - 1).Equals(':') && !text.GetText().ElementAt(index - 1).Equals('`')) {
					text.SetText(text.GetText().Remove(index - 1, 1));
					text.ScrollTo(1f);
				}
			} else {
				int index = text.GetText().IndexOf("|");
				text.SetText(text.GetText().Insert(index, c.ToString()));
				if (LimitTyping && text.rows + 1 > maxRows) {
					GetKey((char)Keys.Back);
				}
				text.ScrollTo(1f);
			}
		}

		void GetKeyCode(Keys k) {
			int index;
			switch (k) {
				case Keys.Left:
					index = text.GetText().IndexOf('|');
					if (index == 0 || text.GetText().ElementAt(index - 1).Equals('`') || text.GetText().ElementAt(index - 1).Equals(':'))
						break;
					text.SetText(text.GetText().Remove(index, 1).Insert(index - 1, "|"));
					break;
				case Keys.Right:
					index = text.GetText().IndexOf('|');
					if (index == text.GetText().Length - 1)
						break;
					text.SetText(text.GetText().Remove(index, 1).Insert(index + 1, "|"));
					break;
			}

		}

		public override void Cleanup() {
			if (typingEnabled) {
				Input.RedirectKeys = false;
				Input.DirectCall -= GetKey;
				Input.DirectKeyCode -= GetKeyCode;
				typingEnabled = false;
			}
			text.Cleanup();
			Screen.SelectionEvent -= DisableTyping;
		}
		public override void Setup() {
			text.Setup();
			Screen.SelectionEvent += DisableTyping;
		}

		public void ScrollWheel(int dir, object sender) {
			text.ScrollWheel(dir, this);
		}

		public void ScrollTo(float amount) {
			text.ScrollTo(amount);
		}

		public bool CheckScrollWheel(float x, float y, int dir, object sender) {
			if (CheckCoords(x, y)) {
				ScrollWheel(dir, sender);
				return true;
			}
			return false;
		}
	}
}
