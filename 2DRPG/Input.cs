using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _2DRPG.GUI;

namespace _2DRPG {
	public static class Input {
		public static event inputMethod InputCall;
		public delegate void inputMethod(KeyInputs[] k);
		//Dictionary that stores the game inputs and the current mapped Keycode for the input
		private static Dictionary<KeyInputs, Keys> keycodes = new Dictionary<KeyInputs, Keys>() {
			{KeyInputs.left, Keys.A },
			{KeyInputs.right, Keys.D },
			{KeyInputs.up, Keys.W },
			{KeyInputs.down, Keys.S },
			{KeyInputs.escape, Keys.Escape},
			{KeyInputs.interact, Keys.E}
		};
		private static bool anyKeysHeld = false;
		private static Dictionary<KeyInputs, bool> keysHeld = new Dictionary<KeyInputs, bool>() {
			{KeyInputs.none, false },
			{KeyInputs.left, false },
			{KeyInputs.right, false },
			{KeyInputs.up, false },
			{KeyInputs.down, false }
		};
		private static HashSet<KeyInputs> ignoreKeys = new HashSet<KeyInputs>();

		public static void KeyDown(object sender, KeyEventArgs e) {
			//System.Diagnostics.Debug.WriteLine(e.KeyCode);
			KeyInputs k =  keycodes.FirstOrDefault(x => x.Value == e.KeyCode).Key;  //Reverse lookup for the key based on the value given by the keycode event
			if (keysHeld.ContainsKey(k)) {	//If it's a key that should be held with others
				if (!keysHeld[k]) {
					keysHeld[k] = true;
					anyKeysHeld = true;
				}
			} else {	//If it's a one press key
				if (ignoreKeys.Contains(k))
					return;
				InputCall.Invoke(new KeyInputs[] { k });
				ManualKeys(new KeyInputs[] { k });
				ignoreKeys.Add(k);

			}
		}
		public static void KeyUp(object sender, KeyEventArgs e) {
			KeyInputs k = keycodes.FirstOrDefault(x => x.Value == e.KeyCode).Key;  //Reverse lookup for the key based on the value given by the keycode event
			if (keysHeld.ContainsKey(k)) {
				if (keysHeld[k]) {
					keysHeld[k] = false;
					bool newKeys = false;
					foreach (KeyInputs h in keysHeld.Keys) {
						newKeys = newKeys | keysHeld[h];
					}
					anyKeysHeld = newKeys;
				}
			} else {
				if (ignoreKeys.Contains(k))
					ignoreKeys.Remove(k);
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
				ManualKeys(keys.ToArray());
			}
		}

		public static void ClearKeys() {
			foreach (KeyInputs k in keysHeld.Keys.ToList())
				keysHeld[k] = false;
		}

		public static void MouseSent(object sender, MouseEventArgs e) {
			if (e.Button.Equals(MouseButtons.Left)) {
				HashSet<UIBase>[] windows;
				lock (Screen.currentWindows) {
					windows = Screen.currentWindows.Values.ToArray();
				}
				float checkX = ((float)e.X / Screen.WindowWidth - .5f) * Screen.pixelWidth;    //Convert from mouse coordinates to screen coordinates
				float checkY = -((float)e.Y / Screen.WindowHeight - .5f) * Screen.pixelHeight;
				foreach(HashSet<UIBase> h in windows)
					foreach (UIBase u in h) {
						if (u is UIButton)
							if (((UIButton)u).CheckClick(checkX, checkY))
								return;
					}
			}
		}


		private static void ManualKeys(KeyInputs[] keys) {
			if (keys.Contains(KeyInputs.escape)) {
				if (GameState.CurrentState == GameState.GameStates.Game)
					Screen.AddWindow("pause");
				else if (GameState.CurrentState == GameState.GameStates.Paused) {
					Screen.CloseWindow("pause");
					GameState.SetGameState(GameState.GameStates.Game);
				}
			}
		}

		/// <summary>
		/// Enum values for the game inputs
		/// </summary>
		public enum KeyInputs {none, left, right, up, down, escape, interact};

	}
}
