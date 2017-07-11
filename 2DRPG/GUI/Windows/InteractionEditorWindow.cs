using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.GUI.Interaction;
using System.Reflection;
using _2DRPG.World;

namespace _2DRPG.GUI.Windows {
	class InteractionEditorWindow : IWindow {

		public static World.Objects.WorldObjectInteractable interactable;

		static UIDropdownButton objectData = new UIDropdownButton(200, 171, 100, 20, 2, "button", null, null) { hideTop = true };
		static UITypeBox typeInput = new UITypeBox(200, 110, 100, 40, 3, 5, "lightBack") { LimitTyping = false, Visible = false };

		static HashSet<UIBase> UIObjects = new HashSet<UIBase>() {
			new UIBase(0, 0, 300, 150, 4, "darkBack"),
			new UIBlob(() => { }){ RenderAction = RenderButtons, ClickAction = ClickButtons },
			new UIButton(245, -160, 45, 8, () => { SaveData.SaveGame(); }, 2, "button"){ displayLabel = new UIText(248, -159, .5f, 1, "Save Game")},
			objectData, typeInput
		};

		private static HashSet<UIButton> currentlySelected = new HashSet<UIButton>();

		private static int layer = 0;
		private static int indent = 0;
		private static int maxIndent = 0;

		static Dictionary<int, List<InteractionBase>> boxes = new Dictionary<int, List<InteractionBase>>();
		static Dictionary<int, List<UIButton>> buttons = new Dictionary<int, List<UIButton>>();

		public HashSet<UIBase> GetScreenObjects() {
			return UIObjects;
		}

		public HashSet<UIBase> LoadObjects() {
			layer = 0;
			indent = 0;
			boxes.Clear();
			buttons.Clear();
			objectData.drops.Clear();
			SetupObjects(interactable.InterItems);
			return UIObjects;
		}

		public static void RenderButtons() {
			foreach (List<UIButton> b in buttons.Values) {
				foreach (UIButton bu in b)
					bu.Render();
			}
		}

		public static bool ClickButtons(float x, float y) {
			foreach (List<UIButton> b in buttons.Values) {
				foreach (UIButton bu in b)
					if (bu.CheckClick(x, y))
						return true;
			}
			return false;
		}

		public static void SetupObjects(List<InteractionBase> items) {
			List<InteractionBase> its = new List<InteractionBase>();
			int count = 0;
			List<UIButton> h = new List<UIButton>();
			foreach (InteractionBase b in items) {
				UIButton but = new UIButton(-250 + count * 82 + indent * 20, 140 - layer * 23, 40, 10, null) {
					displayLabel = new UIText(-250 + count * 82 + indent * 20, 140 - layer * 23, .5f, 1, b.ToString())
				};
				int k = layer;
				int i = count;
				int j = indent;
				InteractionBase basse = b;
				but.SetButtonAction(() => {
					ObjectSelected(k, j, i, but, basse);
				});
				h.Add(but);
				its.Add(b);
				count++;
			}
			if (maxIndent < indent)
				maxIndent = indent;
			if (boxes.ContainsKey(indent)) {
				boxes[indent].AddRange(its);
			} else {
				boxes.Add(indent, its);
			}
			if (buttons.ContainsKey(indent)) {
				buttons[indent].AddRange(h);
			} else {
				buttons.Add(indent, h);
			}
		}

		public static void SetupData(InteractionBase bass) {
			bool dialogue = false;
			if (bass is InteractionDialogue)
				dialogue = true;

			List<UIButton> b = new List<UIButton>();
			FieldInfo[] props = bass.GetType().GetFields().Where(prop => Attribute.IsDefined(prop, typeof(Editable))).ToArray();
			foreach (FieldInfo f in props) {
				UITypeBox tb;
				if (dialogue) {
					tb = typeInput;
					tb.DisableTyping();
				} else {
					tb = new UITypeBox(0, 0, objectData.width, objectData.height, 2, 2, "button") { LimitTyping = false };
				}
				tb.text.SetText(f.Name + ":" + f.GetValue(bass).ToString());
				tb.valueAction = () => {
					string val = tb.text.GetText();
					val = val.Substring(val.IndexOf(':') + 1);
					try {
						if (val.Length > 0) {
							if (f.FieldType.Equals(typeof(String)))
								f.SetValue(bass, val);
							else
								f.SetValue(bass, int.Parse(val));
							bass.ModificationAction();
						}
					} catch (FormatException) {
						System.Diagnostics.Debug.WriteLine("Invalid Format, could not update Value");
					}
				};
				if (!dialogue)
					b.Add(tb);
			}
			if (dialogue) {
				typeInput.Visible = true;
			} else {
				objectData.displaySize = (b.Count > 5) ? 5 : b.Count;
				objectData.SetDropdowns(b.ToArray());
				objectData.showDrops = true;
			}
		}

		public static void RemoveObjects(int ind) {
			if (maxIndent > ind) {
				for (int i = ind + 1; i <= maxIndent; i++) {
					boxes.Remove(i);
					buttons.Remove(i);
				}
				maxIndent = ind;
				indent = ind;
			}
		}

		public static void ObjectSelected(int lay, int ind, int box, UIButton button, InteractionBase basse) {
			RemoveObjects(ind);
			layer = lay;
			objectData.showDrops = false;
			typeInput.Visible = false;
			objectData.drops.Clear();
			if (currentlySelected.Contains(button)) {
				button.displayLabel.textColor = System.Drawing.Color.Black;
				currentlySelected.Remove(button);
				return;
			}
			foreach (UIButton b in buttons[ind])
				if (currentlySelected.Contains(b)) {
					currentlySelected.Remove(b);
					b.displayLabel.textColor = System.Drawing.Color.Black;
				}
			
			currentlySelected.Add(button);
			button.displayLabel.textColor = System.Drawing.Color.Cyan;
			if (basse is InteractionChoice c) {
				indent++;
				foreach (List<InteractionBase> j in c.choices.Values) {
					layer++;
					SetupObjects(j);
				}
			}
			if (basse is InteractionDialogue d) {
				SetupData(basse);
			}
		}

		private static void AddItems(int ind, int box) {

		}

		private static void RemoveItems() {

		}

		string[] textures = new string[] { "darkBack", "button", "lightBack" };

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
