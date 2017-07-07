using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2DRPG.GUI.Windows {
	class ConsoleWindow : IWindow {

		HashSet<UIBase> UIObjects = new HashSet<UIBase>() {
			new UIBase(0, 150, 225, 20, 0, "button"),
			input, output,

		};

		static UITextBox input = new UITextBox(0, 100, .5f, 400, 0, "");
		static UITextBox output = new UITextBox(0, 150, .5f, 400, 0, "");

		public ref HashSet<UIBase> LoadObjects() {
			input.SetText("");
			return ref UIObjects;
		}

		public void GetKey(char c) {
			if (c.Equals((char)Keys.Enter)) {
				SubmitInput();
			} else if (c.Equals((char)Keys.Back)) {
				input.SetText(input.GetText().Remove(input.GetText().Length - 1, 1));
			} else {
				input.SetText(input.GetText() + c);
			}
		}

		private void SubmitInput() {
			output.SetText(Console.ExecuteCommand(input.GetText().Substring(1)));
			input.SetText("`");
		}


		string[] textures = new string[] { "button" };

		public void LoadTextures() {
			TextureManager.RegisterTextures(textures);
			Input.DirectCall += GetKey;
			Input.RedirectKeys = true;
		}

		public void UnloadTextures() {
			TextureManager.UnRegisterTextures(textures);
			Input.DirectCall -= GetKey;
			Input.RedirectKeys = false;
		}
	}
}
