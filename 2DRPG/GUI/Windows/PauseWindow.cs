using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2DRPG.GUI.Windows {
	class PauseWindow : IWindow {

		HashSet<UIBase> UIObjects = new HashSet<UIBase>() {
			new UIButton(0f, 80f, 80f, 10f, () => {
				Screen.CloseWindow("pause");
			}, 1, "button") {
				displayLabel = new UIText(0f, 80f, .5f, 0, "Continue")
			},
			new UIButton(0f, 50f, 80f, 10f, () => {
				Application.Exit();
			}, 1, "button") {
				displayLabel = new UIText(0f, 50f, .5f, 0, "Quit")
			}
		};

		public ref HashSet<UIBase> LoadObjects() {
			Input.ClearKeys();

			return ref UIObjects;
		}

		public string[] textureNames = new string[] {
			"button"
		};

		public void LoadTextures() {
			GameState.SetGameState(GameState.GameStates.Paused);
			TextureManager.RegisterTextures(textureNames);
		}

		public void UnloadTextures() {
			GameState.SetGameState(GameState.GameStates.Game);
			TextureManager.UnRegisterTextures(textureNames);
		}
	}
}
