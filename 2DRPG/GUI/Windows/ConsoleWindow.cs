using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2DRPG.GUI.Windows {
	class ConsoleWindow : IWindow {

		HashSet<UIBase> UIObjects = new HashSet<UIBase>() {
			new UIBase(0, 145, 205, 35, 0, "lightBack"),
			input, output,

		};

		static UITypeBox input = new UITypeBox(0, 88, 200, 20, 0, 1, "") { showBackground = false };
		static UITextBox output = new UITextBox(0, 170, .5f, 400, 0, 5, "");

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
			output.SetText(output.GetText() + "\n" + Console.ExecuteCommand(input.text.GetText().Substring(1)));
			input.text.SetText("`");
			input.StartTyping();
			if (output.rows > 5)
				output.ScrollTo(1f);
				
		}


		string[] textures = new string[] { "lightBack" };

		public void LoadTextures() {
			TextureManager.RegisterTextures(textures);
		}

		public void UnloadTextures() {
			input.DisableTyping();
			TextureManager.UnRegisterTextures(textures);
		}
	}
}
