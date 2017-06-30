using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2DRPG.GUI.Windows {
	class PauseWindow : IWindow {

		private HashSet<UIBase> UIObjects = new HashSet<UIBase>();

		public HashSet<UIBase> LoadObjects() {
			GameState.SetGameState(GameState.GameStates.Paused);
			Input.ClearKeys();
			UIObjects.Clear();
			UIObjects.Add(new UIButton(0f, 80f, 80f, 10f, () => {
				GameState.SetGameState(GameState.GameStates.Game);
				Screen.CloseWindow("pause");
			}, 1, "button") {
				displayLabel = new UIText(0f, 80f, .5f, "Continue")
			});
			UIObjects.Add(new UIButton(0f, 50f, 80f, 10f, () => {
				Application.Exit();
			}, 1, "button") {
				displayLabel = new UIText(0f, 50f, .5f, "Quit")
			});

			return UIObjects;
		}

		string[] textureNames = new string[] {
			"button"
		};

		public void LoadTextures() {
			TextureManager.RegisterTextures(textureNames);
		}

		public void UnloadTextures() {
			TextureManager.UnRegisterTextures(textureNames);
		}
	}
}
