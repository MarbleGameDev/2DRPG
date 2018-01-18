using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2DRPG.GUI.Windows {
	class ConsoleWindow : IWindow {

		HashSet<UIBase> UIObjects = new HashSet<UIBase>() {
			new UIBase(0, 144, 205, 36, 0, TextureManager.TextureNames.textBox),
			input, output,

		};

		List<string> previousInputs = new List<string>();
		int currentInput;

		static UITypeBox input = new UITypeBox(0, 100, 200, 20, 0, 1, TextureManager.TextureNames.DEFAULT) { showBackground = false };
		static UITextBox output = new UITextBox(0, 168, .5f, 400, 0, 3, "");

		public HashSet<UIBase> LoadObjects() {
			input.text.SetText("");
			input.valueAction = SubmitInput;
			input.StartTyping();
			output.SetText("");
			return UIObjects;
		}
		public HashSet<UIBase> GetScreenObjects() {
			return UIObjects;
		}

		private void SubmitInput() {
			string s = input.text.GetText().Replace("`", "");
			if (s.Length == 0)
				return;
			previousInputs.Add(s);
			currentInput = previousInputs.Count;
			input.text.SetText("`");
			input.StartTyping();
			output.SetText(output.GetText() + "\n" + Console.ExecuteCommand(s));
			output.ScrollTo(1f);
		}

		public void KeyCode(Keys e) {
			switch (e) {
				case Keys.Up:
					if (currentInput - 1 >= 0) {
						currentInput--;
						input.text.SetText("`" + previousInputs[currentInput] + "|");
					}
					break;
				case Keys.Down:
					if (++currentInput < previousInputs.Count) {
						input.text.SetText("`" + previousInputs[currentInput] + "|");
					} else {
						input.text.SetText("`|");
					}
					break;
			}
		}


		Texture[] textures = new Texture[] { TextureManager.TextureNames.textBox };

		public void LoadTextures() {
			Input.DirectKeyCode += KeyCode;
			TextureManager.RegisterTextures(textures);
		}

		public void UnloadTextures() {
			Input.DirectKeyCode -= KeyCode;
			TextureManager.UnRegisterTextures(textures);
		}
	}
}
