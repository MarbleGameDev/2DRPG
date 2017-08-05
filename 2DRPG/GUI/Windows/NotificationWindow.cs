using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI.Windows {
	class NotificationWindow : IWindow {

		public static int frameNum = 30;

		public static void NewNotification(string displayText, int frames = 30) {
			display.SetText(displayText, .5f);
			frameNum = frames;
			Screen.OpenWindow("notification");
		}

		private static UIBlob bl = new UIBlob(() => { });
		private static UIText display = new UIText(272, 166, .5f, 0, "");

		HashSet<UIBase> UIObjects = new HashSet<UIBase>() {
			new UIBase(270, 165, 50, 15, 0, "lightBack"),
			bl, display
		};

		public HashSet<UIBase> GetScreenObjects() {
			return UIObjects;
		}

		public HashSet<UIBase> LoadObjects() {
			counter = 0;
			bl.RenderAction = RenderCall;
			return UIObjects;
		}

		string[] textures = new string[] { "lightBack" };

		public void LoadTextures() {
			TextureManager.RegisterTextures(textures);
		}

		public void UnloadTextures() {
			TextureManager.UnRegisterTextures(textures);
		}
		private bool toggle = false;
		private int counter;
		public void RenderCall() {
			if (++counter >= frameNum && !toggle) {
				Screen.QueueCloseWindow("notification");
				toggle = true;
			}
		}
	}
}
