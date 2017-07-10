using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.GUI.Interaction;

namespace _2DRPG.GUI.Windows {
	class InteractionWindow : IWindow {

		static UIButton advance = new UIButton(0, -80, 20, 10, 2, "button") { displayLabel = new UIText(2, -80, .5f, 1, "Next")};
		static UIButton regress = new UIButton(-50, -80, 20, 10, 2, "button") { displayLabel = new UIText(-48, -80, .5f, 1, "Back") };
		static UIBlob blob = new UIBlob(() => { }) {
			RenderAction = () => {
				RenderCurrent();
			}
		};

		HashSet<UIBase> UIObjects = new HashSet<UIBase>() {
			new UIBase(0, 0, 200, 100, 3, "darkBack"),
			advance, regress, blob
		};

		static List<InteractionBase> interactionElements = new List<InteractionBase>();

		InteractionBase currentInteraction;

		static int elementNum = 0;

		public void NextNode() {
			elementNum++;
			if (elementNum < interactionElements.Count) {
				currentInteraction = interactionElements[elementNum];
				currentInteraction.nextNode = NextNode;
				currentInteraction.Setup();
			} else {
				Screen.CloseWindow("interaction");
			}
		}
		public void PreviousNode() {
			elementNum--;
			if (elementNum >= 0) {
				currentInteraction = interactionElements[elementNum];
				currentInteraction.nextNode = NextNode;
				currentInteraction.Setup();
			} else {
				Screen.CloseWindow("interaction");
				elementNum = 0;
			}
		}
		public static void InsertNodes(List<InteractionBase> items) {
			interactionElements.InsertRange(elementNum+1, items);
		}

		static void RenderCurrent() {
			if (interactionElements.Count > 0)
			if (interactionElements[elementNum] != null)
				interactionElements[elementNum].Render();
		}

		public static void SetInteractionElements(List<InteractionBase> elements) {
			interactionElements.Clear();
			elementNum = 0;
			interactionElements.AddRange(elements);
		}

		public HashSet<UIBase> LoadObjects() {
			blob.ClickAction = (float x, float y) => {
				if (currentInteraction is InteractionChoice c)
					return c.CheckClick(x, y);
				else return false;
			};
			elementNum = 0;
			foreach (InteractionBase b in interactionElements)
				b.nextNode = NextNode;
			advance.SetButtonAction(NextNode);
			regress.SetButtonAction(PreviousNode);
			return UIObjects;
		}

		public HashSet<UIBase> GetScreenObjects() {
			return UIObjects;
		}

		private string[] textures = new string[] {
			"darkBack", "button"
		};

		public void LoadTextures() {
			Screen.CloseWindow("hud");
			Screen.WindowOpen = true;
			TextureManager.RegisterTextures(textures);
		}

		public void UnloadTextures() {
			Screen.OpenWindow("hud");
			Screen.WindowOpen = false;
			TextureManager.UnRegisterTextures(textures);
		}
	}
}
