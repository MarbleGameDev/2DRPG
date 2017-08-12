﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.GUI.Interaction;

namespace _2DRPG.GUI.Windows {
	class InteractionWindow : IWindow {

		public static bool holdInput = false;

		static UIBlob blob = new UIBlob(() => { }) {
			RenderAction = () => {
				RenderCurrent();
			}
		};

		HashSet<UIBase> UIObjects = new HashSet<UIBase>() {
			new UIBase(0, -120, 150, 40, 3, "textBox"),
			//new UIButton(188, 88, 12, 12, () => { Screen.CloseWindow("interaction"); },1, "textBox"){ displayLabel = new UIText(195, 92, 1f, 0, "X") },
			blob	
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
			blob.ClickAction = (x, y) => {
				if (currentInteraction is InteractionChoice c)
					return c.CheckClick(x, y);
				else {
					NextNode();
					return true;
				}
			};
			elementNum = 0;
			foreach (InteractionBase b in interactionElements)
				b.nextNode = NextNode;
			currentInteraction = interactionElements[elementNum];
			currentInteraction.Setup();
			return UIObjects;
		}

		public HashSet<UIBase> GetScreenObjects() {
			return UIObjects;
		}

		private string[] textures = new string[] {
			"darkBack", "button", "textBox"
		};

		public void LoadTextures() {
			Screen.CloseWindow("hud");
			Screen.WindowOpen = true;
			TextureManager.RegisterTextures(textures);
			Input.InputCall += InputCall;
		}

		private void InputCall(Input.KeyInputs[] k) {
			if (k.Contains(Input.KeyInputs.interact)) {
				if (holdInput) {
					holdInput = false;
					return;
				}
				if (!(currentInteraction is InteractionChoice)) {
					NextNode();
				}
			}
		}

		public void UnloadTextures() {
			Screen.OpenWindow("hud");
			Screen.WindowOpen = false;
			TextureManager.UnRegisterTextures(textures);
			Input.InputCall -= InputCall;
		}

	}
}
