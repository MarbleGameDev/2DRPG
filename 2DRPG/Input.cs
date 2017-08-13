using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _2DRPG.GUI;

namespace _2DRPG {
	public static class Input {
		/// <summary>
		/// Event call for KeyCode enums
		/// </summary>
		public static event inputMethod InputCall;
		/// <summary>
		/// Event call for direct char input
		/// </summary>
		public static event keyMethod DirectCall;
		public static event KeysMethod DirectKeyCode;
		public delegate void inputMethod(KeyInputs[] k);
		public delegate void keyMethod(char k);
		public delegate void KeysMethod(Keys k);
		//Dictionary that stores the game inputs and the current mapped Keycode for the input
		public static Dictionary<KeyInputs, Keys> keycodes = new Dictionary<KeyInputs, Keys>() {
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

		public static void KeyDown(KeyEventArgs e) {
			if (RedirectKeys && !e.KeyCode.Equals(keycodes[KeyInputs.escape]) && !e.KeyCode.Equals(keycodes[KeyInputs.console])) {
				if (DirectKeyCode != null) {
					DirectKeyCode.Invoke(e.KeyCode);
				}
				return;
			}
			KeyInputs k =  keycodes.FirstOrDefault(x => x.Value == e.KeyCode).Key;  //Reverse lookup for the key based on the value given by the keycode event
			if (keysHeld.ContainsKey(k)) {	//If it's a key that should be held with others
				if (!keysHeld[k]) {
					keysHeld[k] = true;
					anyKeysHeld = true;
				}
			} else {	//If it's a one press key
				if (ignoreKeys.Contains(k))
					return;
				ManualKeys(new KeyInputs[] { k });
				InputCall.Invoke(new KeyInputs[] { k });
				ignoreKeys.Add(k);

			}
		}

		public static void KeyPress(KeyPressEventArgs e) {
			if (!RedirectKeys || e.KeyChar.Equals((char)keycodes[KeyInputs.escape]) || e.KeyChar.Equals((char)keycodes[KeyInputs.console]))
				return;
			if (DirectCall != null)
				DirectCall.Invoke(e.KeyChar);
		}

		public static void KeyUp(KeyEventArgs e) {
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

		public static void MouseSent(MouseEventArgs e) {
			if (e.Button.Equals(MouseButtons.Left)) {
				if (GUI.Windows.BuilderWindow.checkWorldObjects) {
					lock (WorldData.currentRegions) {
						foreach (HashSet<World.Objects.WorldObjectBase> l in WorldData.currentRegions.Values) {
							foreach (World.Objects.WorldObjectBase o in l)
								if (o.CheckCoords(MouseX + WorldData.CurrentX, MouseY + WorldData.CurrentY))
									return;
						}
					}
				}
				HashSet<UIBase>[] windows;
				lock (Screen.currentWindows) {
					windows = Screen.currentWindows.Values.ToArray();
				}
				foreach(HashSet<UIBase> h in windows)
					foreach (UIBase u in h) {
						if (u is UIButton bu)
							if (bu.CheckClick(MouseX, MouseY))
								return;
					}
			}
		}

		public static void MouseScroll(MouseEventArgs e) {
			int d = e.Delta / 120;
			HashSet<UIBase>[] windows;
			lock (Screen.currentWindows) {
				windows = Screen.currentWindows.Values.ToArray();
			}

			foreach (HashSet<UIBase> h in windows)
				foreach (UIBase u in h) {
					if (u is IScrollable sc)
						if (sc.CheckCoords(MouseX, MouseY))
							sc.ScrollWheel(d, null);
				}
		}

		public static void MouseMove(MouseEventArgs e) {
			MouseX = (e.X  - Screen.WindowWidth / 2f) / Screen.screenWidth * Screen.pixelWidth;    //Convert from mouse coordinates to screen coordinates
			MouseY = -(e.Y - Screen.WindowHeight / 2f) / Screen.screenHeight * Screen.pixelHeight;
		}

		/// <summary>
		/// Manual key testing for key testing or global calls
		/// </summary>
		/// <param name="keys"></param>
		private static void ManualKeys(KeyInputs[] keys) {
			if (keys.Contains(KeyInputs.escape)) {
				if (GameState.CurrentState == GameState.GameStates.Game) {
					List<string> windows = Screen.currentWindows.Keys.ToList();
					if (windows.Contains("hud"))
						windows.Remove("hud");
					if (windows.Contains("notification"))
						windows.Remove("notification");
					foreach (string s in windows) {
						Screen.CloseWindow(s);
					}
					if (windows.Count == 0)
						Screen.OpenWindow("pause");
				} else if (GameState.CurrentState == GameState.GameStates.Paused) {
					Screen.CloseWindow("pause");
				}
			}
			if (keys.Contains(KeyInputs.console)) {
				if (Screen.currentWindows.ContainsKey("console"))
					Screen.CloseWindow("console");
				else {
					Screen.OpenWindow("console");
					ClearKeys();
				}
			}
			if (keys.Contains(KeyInputs.interact)) {
				if (LogicUtils.Logic.interactableObject != null && !Screen.WindowOpen) {
					GUI.Windows.InteractionWindow.holdInput = true;
					LogicUtils.Logic.interactableObject.Interact();
				}
			}
		}

		public static void ClearDirectKeyCode() {
			DirectKeyCode = null;
		}

		/// <summary>
		/// Enum values for the game inputs
		/// </summary>
		public enum KeyInputs {none, left, right, up, down, escape, interact, console};

	}
}
