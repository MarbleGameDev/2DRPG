using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2DRPG {
	static class Input {
		public static event inputMethod InputCall;
		public delegate void inputMethod(KeyInputs[] k);
		//Dictionary that stores the game inputs and the current mapped Keycode for the input
		private static Dictionary<KeyInputs, Keys> keycodes = new Dictionary<KeyInputs, Keys>() {
			{KeyInputs.left, Keys.A },
			{KeyInputs.right, Keys.D },
			{KeyInputs.up, Keys.W },
			{KeyInputs.down, Keys.S }
		};
		private static bool anyKeysHeld = false;
		private static Dictionary<KeyInputs, bool> keysHeld = new Dictionary<KeyInputs, bool>() {
			{KeyInputs.left, false },
			{KeyInputs.right, false },
			{KeyInputs.up, false },
			{KeyInputs.down, false }
		};

		public static void KeyDown(object sender, KeyEventArgs e) {
			//System.Diagnostics.Debug.WriteLine(e.KeyCode);
			KeyInputs k =  keycodes.FirstOrDefault(x => x.Value == e.KeyCode).Key;  //Reverse lookup for the key based on the value given by the keycode event
			if (!keysHeld[k]) {
				keysHeld[k] = true;
				anyKeysHeld = true;
			}
		}
		public static void KeyUp(object sender, KeyEventArgs e) {
			KeyInputs k = keycodes.FirstOrDefault(x => x.Value == e.KeyCode).Key;  //Reverse lookup for the key based on the value given by the keycode event
			if (keysHeld[k]) {
				keysHeld[k] = false;
				bool newKeys = false;
				foreach (KeyInputs h in keysHeld.Keys) {
					newKeys = newKeys | keysHeld[h];
				}
				anyKeysHeld = newKeys;
			}
		}

		public static void UpdateKeys() {
			if (anyKeysHeld) {
				List<KeyInputs> keys = new List<KeyInputs>();
				foreach (KeyInputs k in keysHeld.Keys) {
					if (keysHeld[k])
						keys.Add(k);
				}
				InputCall.Invoke(keys.ToArray());
			}
		}

		public static void MouseSent(object sender, MouseEventArgs e) {
			if (e.Button.Equals(MouseButtons.Left)) {
				GUI.UIBase[] guiObjects = Screen.UIObjects.ToArray();   
				float checkX = Screen.windowRatio * 2 * ((float)e.X / Screen.WindowWidth - .5f);	//Convert from mouse coordinates to screen coordinates
				float checkY = 2 * (.5f - (float)e.Y / Screen.WindowHeight);
				foreach (GUI.UIBase u in guiObjects) {
					if (u is GUI.UIButton)
						((GUI.UIButton)u).CheckClick( checkX, checkY);
				}
			}
		}

		/// <summary>
		/// Enum values for the game inputs
		/// </summary>
		public enum KeyInputs { left, right, up, down};

	}
}
