﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _2DRPG.GUI;

namespace _2DRPG {
	public static class Input {
		public static event inputMethod InputCall;
		public static event keyMethod DirectCall;
		public delegate void inputMethod(KeyInputs[] k);
		public delegate void keyMethod(char k);
		//Dictionary that stores the game inputs and the current mapped Keycode for the input
		private static Dictionary<KeyInputs, Keys> keycodes = new Dictionary<KeyInputs, Keys>() {
			{KeyInputs.left, Keys.A },
			{KeyInputs.right, Keys.D },
			{KeyInputs.up, Keys.W },
			{KeyInputs.down, Keys.S },
			{KeyInputs.escape, Keys.Escape},
			{KeyInputs.interact, Keys.E},
			{KeyInputs.console, Keys.Oemtilde }
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

		public static float MouseX, MouseY;
		public static bool MouseHeld;
		public static bool RedirectKeys;

		public static void KeyDown(object sender, KeyEventArgs e) {
			if (RedirectKeys && !e.KeyCode.Equals(Keys.Escape) && !e.KeyCode.Equals(Keys.Oemtilde))
				return;
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

		public static void KeyPress(object sender, KeyPressEventArgs e) {
			if (!RedirectKeys || e.KeyChar.Equals((char)Keys.Escape) || e.KeyChar.Equals((char)Keys.Oemtilde))
				return;
			if (DirectCall != null)
				DirectCall.Invoke(e.KeyChar);
		}

		public static void KeyUp(object sender, KeyEventArgs e) {
			if (RedirectKeys && !e.KeyCode.Equals(Keys.Escape) && !e.KeyCode.Equals(Keys.Oemtilde))
				return;
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
				
				foreach(HashSet<UIBase> h in windows)
					foreach (UIBase u in h) {
						if (u is UIButton)
							if (((UIButton)u).CheckClick(MouseX, MouseY))
								return;
					}
			}
		}

		public static void MouseMove(object sender, MouseEventArgs e) {
			MouseX = (e.X  - Screen.WindowWidth / 2f) / Screen.screenWidth * Screen.pixelWidth;    //Convert from mouse coordinates to screen coordinates
			MouseY = -(e.Y - Screen.WindowHeight / 2f) / Screen.screenHeight * Screen.pixelHeight;
		}


		private static void ManualKeys(KeyInputs[] keys) {
			if (keys.Contains(KeyInputs.escape)) {
				if (GameState.CurrentState == GameState.GameStates.Game)
					Screen.AddWindow("pause");
				else if (GameState.CurrentState == GameState.GameStates.Paused) {
					Screen.CloseWindow("pause");
				}
			}
			if (keys.Contains(KeyInputs.console)) {
				if (Screen.currentWindows.ContainsKey("console"))
					Screen.CloseWindow("console");
				else {
					Screen.AddWindow("console");
					ClearKeys();
				}
			}
			if (keys.Contains(KeyInputs.interact)) {
				if (LogicUtils.Logic.interactableObject != null)
					LogicUtils.Logic.interactableObject.Interact();
			}
		}

		/// <summary>
		/// Enum values for the game inputs
		/// </summary>
		public enum KeyInputs {none, left, right, up, down, escape, interact, console};

	}
}
