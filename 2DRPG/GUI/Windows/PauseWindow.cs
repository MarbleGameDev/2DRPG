using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2DRPG.GUI.Windows {
	class PauseWindow : IWindow {

		HashSet<UIBase> UIObjects = new HashSet<UIBase>() {
			new UIButton(0f, 80f, 80f, 10f, 1, TextureManager.TextureNames.button) {
				displayLabel = new UIText(0f, 80f, .5f, 0, "Continue"),
				buttonAction = () => {
					Screen.CloseWindow("pause");
				}
			},
			new UIButton(0f, 50f, 80f, 10f, 1, TextureManager.TextureNames.button) {
				displayLabel = new UIText(0f, 50f, .5f, 0, "Options"),
				buttonAction = () => {
					Screen.CloseWindow("pause");
					Screen.OpenWindow("options");
				}
			},
			new UIButton(0f, 20f, 80f, 10f, 1, TextureManager.TextureNames.button) {
				displayLabel = new UIText(0f, 20f, .5f, 0, "Quit"),
				buttonAction = () => {
					Application.Exit();
				}
			}
		};

		public HashSet<UIBase> LoadObjects() {
			Input.ClearKeys();

			return UIObjects;
		}
		public HashSet<UIBase> GetScreenObjects() {
			return UIObjects;
		}

		Texture[] textureNames = new Texture[] {
			TextureManager.TextureNames.button
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
