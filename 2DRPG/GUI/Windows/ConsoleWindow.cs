using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2DRPG.GUI.Windows {
	class ConsoleWindow : IWindow {

		HashSet<UIBase> UIObjects = new HashSet<UIBase>() {
			new UIBase(0, 144, 205, 36, 0, "textBox"),
			input, output,

		};

		static UITypeBox input = new UITypeBox(0, 100, 200, 20, 0, 1, "") { showBackground = false };
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
			if (input.text.GetText().Substring(1).Length == 0)
				return;
			output.SetText(output.GetText() + "\n" + Console.ExecuteCommand(input.text.GetText().Substring(1)));
			input.text.SetText("`");
			input.StartTyping();
			output.ScrollTo(1f);
		}


		string[] textures = new string[] { "textBox" };

		public void LoadTextures() {
			input.Setup();
			output.Setup();
			TextureManager.RegisterTextures(textures);
		}

		public void UnloadTextures() {
			input.Cleanup();
			output.Cleanup();
			TextureManager.UnRegisterTextures(textures);
		}
	}
}
