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
			UIObjects.Add(new UIButton(0f, .6f, .8f, .1f, () => {
				GameState.SetGameState(GameState.GameStates.Game);
				Screen.CloseWindow("pause");
			}, 1, "button") {
				displayLabel = new UIText(0f, .6f, .8f, .1f, "Continue")
			});
			UIObjects.Add(new UIButton(0f, .4f, .8f, .1f, () => {
				Application.Exit();
			}, 1, "button") {
				displayLabel = new UIText(0f, .4f, .8f, .1f, "Quit")
			});

			return UIObjects;
		}

		string[] textureNames = new string[] {
			"button", "baseFont"
		};

		public void LoadTextures() {
			TextureManager.RegisterTextures(textureNames);
		}

		public void UnloadTextures() {
			TextureManager.UnRegisterTextures(textureNames);
		}
	}
}
