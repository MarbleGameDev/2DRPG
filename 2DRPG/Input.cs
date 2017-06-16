using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2DRPG {
	static class Input {
		public static event inputMethod InputCall;
		public delegate void inputMethod(KeyInputs k);
		//Dictionary that stores the game inputs and the current mapped Keycode for the input
		private static Dictionary<KeyInputs, Keys> keycodes = new Dictionary<KeyInputs, Keys>() {
			{KeyInputs.left, Keys.A },
			{KeyInputs.right, Keys.D },
			{KeyInputs.up, Keys.W },
			{KeyInputs.down, Keys.S }
		};

		public static void KeySent(object sender, KeyEventArgs e) {
			//System.Diagnostics.Debug.WriteLine(e.KeyCode);
			KeyInputs k =  keycodes.FirstOrDefault(x => x.Value == e.KeyCode).Key;	//Reverse lookup for the key based on the value given by the keycode event
			InputCall.Invoke(k);
		}

		public static void MouseSent(object sender, MouseEventArgs e) {
			System.Diagnostics.Debug.WriteLine(e.Button);
		}

		/// <summary>
		/// Enum values for the game inputs
		/// </summary>
		public enum KeyInputs { left, right, up, down};

	}
}
