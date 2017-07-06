using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2DRPG.GUI.Windows {
	class ConsoleWindow : IWindow {

		HashSet<UIBase> UIObjects = new HashSet<UIBase>();

		UITextBox input = new UITextBox(0, 100, .5f, 400, "");
		UITextBox output = new UITextBox(0, 150, .5f, 400, "");

		public HashSet<UIBase> LoadObjects() {
			UIObjects.Clear();
			UIObjects.Add(new UIBase(0, 150, 225, 20, 5, "button"));
			UIObjects.Add(input);
			input.SetText("");
			UIObjects.Add(output);
			return UIObjects;
		}

		public void getKey(char c) {
			if (c.Equals((char)Keys.Enter)) {
				submitInput();
			} else if (c.Equals((char)Keys.Back)) {
				input.SetText(input.GetText().Remove(input.GetText().Length - 1, 1));
			} else {
				input.SetText(input.GetText() + c);
			}
		}

		private void submitInput() {
			output.SetText(Console.ExecuteCommand(input.GetText().Substring(1)));
			input.SetText("`");
		}


		private string[] textures = new string[] { "button" };

		public void LoadTextures() {
			TextureManager.RegisterTextures(textures);
			Input.DirectCall += getKey;
			Input.RedirectKeys = true;
		}

		public void UnloadTextures() {
			TextureManager.UnRegisterTextures(textures);
			Input.DirectCall -= getKey;
			Input.RedirectKeys = false;
		}
	}
}
