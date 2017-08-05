using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.World.Objects;
using _2DRPG.World;
using System.Reflection;

namespace _2DRPG.GUI.Windows {
	class BuilderWindow : IWindow {

		private static WorldObjectBase currentObject;
		public static bool checkWorldObjects = false;

		public static void SetCurrentObject(WorldObjectBase obj) {
			currentObject = obj;
			checkWorldObjects = false;
			Screen.OpenWindow("worldBuilder");
			UpdateObjectInfo();
		}

		static UIDropdownButton objectData = new UIDropdownButton(220, 100, 100, 20, 3, "button", null, null) { hideTop = true };
		static UIDropdownButton newObjects = new UIDropdownButton(0, 100, 100, 20, 2, "button", new UIText(0, 100, .5f, 1, "Select Object Type")) { Visible = false, hideTop = true, showDrops = true, displaySize = 4 };
		static UIButton applyBut = new UIButton(185, -140, 65, 8, () => { NewObject(); }, 1, "button") { displayLabel = new UIText(188, -140, .5f, 0, "Apply"), Visible = false };
		static UIText objectName = new UIText(220, 100, .5f, 1, "No Object Selected") { textColor = System.Drawing.Color.Aqua};

		public HashSet<UIBase> screenObjects = new HashSet<UIBase>() {
			new UIBase(220, 0, 100, 180, 4, "darkBack"),
			new UIButton(310, 170, 10, 10, () => { Screen.CloseWindow("worldBuilder"); },1, "button"){ displayLabel = new UIText(317, 175, 1f, 0, "X") },
			new UIButton(185, 140, 65, 8, () => { Screen.CloseWindow("worldBuilder");  checkWorldObjects = true; },1, "button"){ displayLabel = new UIText(188, 142, .5f, 0, "Select Object") },
			new UIButton(165, -160, 45, 8, () => { SaveData.SaveGame(); }, 1, "button"){ displayLabel = new UIText(168, -160, .5f, 0, "Save Game")},
			new UIButton(265, -160, 35, 8, () => { WorldData.RemoveObject(currentObject); }, 1, "button"){ displayLabel = new UIText(268, -160, .5f, 0, "Delete")},
			new UIButton(185, 160, 65, 8, () => { newObjects.Visible = !newObjects.Visible; }, 1, "button"){ displayLabel = new UIText(188, 160, .5f, 0, "New Object")},
			objectData, newObjects, applyBut, objectName
		};

		public HashSet<UIBase> LoadObjects() {
			return screenObjects;
		}
		public HashSet<UIBase> GetScreenObjects() {
			return screenObjects;
		}

		static void UpdateObjectInfo() {
			if (objectData != null) {
				List<UIButton> b = new List<UIButton>();
				FieldInfo[] props = currentObject.GetType().GetFields().Where(prop => Attribute.IsDefined(prop, typeof(Editable))).ToArray();
				foreach (FieldInfo f in props) {
					UITypeBox tb = new UITypeBox(0, 0, objectData.width, objectData.height, 2, 1, "button");
					tb.text.SetText(f.Name + ":" + f.GetValue(currentObject).ToString());
					tb.text.SetLayer(1);
					tb.valueAction = () => {
						string val = tb.text.GetText();
						val = val.Substring(val.IndexOf(':')+1);
						try {
							if (val.Length > 0) {
								if (f.FieldType.Equals(typeof(String)))
									f.SetValue(currentObject, val);
								else
									f.SetValue(currentObject, int.Parse(val));
								currentObject.ModificationAction();
							}
						} catch (FormatException) {
							System.Diagnostics.Debug.WriteLine("Invalid Format, could not update Value");
						}
					};
					b.Add(tb);
				}
				objectData.displaySize = (b.Count > 5) ? 5 : b.Count;
				objectData.SetDropdowns(b.ToArray());
				objectName.SetText(currentObject.GetType().Name);
				objectData.showDrops = true;
			} else {
				objectData.displayLabel.SetText("No Object Currently Selected");
			}
		}
		static Type nt;
		static object[] ntParams;
		static void SetupNewObject(GameSave.WorldObjectType newType) {
			switch (newType) {
				case GameSave.WorldObjectType.Animated:
					nt = typeof(WorldObjectAnimated);
					break;
				case GameSave.WorldObjectType.Base:
					nt = typeof(WorldObjectBase);
					break;
				case GameSave.WorldObjectType.Collidable:
					nt = typeof(WorldObjectCollidable);
					break;
				case GameSave.WorldObjectType.Controllable:
					nt = typeof(WorldObjectControllable);
					break;
				case GameSave.WorldObjectType.Interactable:
					nt = typeof(WorldObjectInteractable);
					break;
				case GameSave.WorldObjectType.Movable:
					nt = typeof(WorldObjectMovable);
					break;
				case GameSave.WorldObjectType.MovableAnimated:
					nt = typeof(WorldObjectMovableAnimated);
					break;
				default:
					return;
			}
			List<UIButton> b = new List<UIButton>();
			ParameterInfo[] parms = nt.GetConstructors()[0].GetParameters();
			ntParams = new object[parms.Length];
			int counter = 0;
			foreach (ParameterInfo p in parms) {
				UITypeBox tb = new UITypeBox(0, 0, objectData.width, objectData.height, 2, 1, "button");
				tb.text.SetText(p.Name + ":");
				int i = counter++;
				tb.valueAction = () => {
					string val = tb.text.GetText();
					val = val.Substring(val.IndexOf(':') + 1);
					try {
						if (val.Length > 0) {
							if (p.ParameterType.Equals(typeof(String)))
								ntParams[i] = val;
							else
								ntParams[i] = int.Parse(val);
						}
					} catch (FormatException) {
						System.Diagnostics.Debug.WriteLine("Invalid Format");
						ntParams[i] = null;
					}
				};
				b.Add(tb);
			}
			objectData.displaySize = (b.Count > 5) ? 5 : b.Count;
			objectData.SetDropdowns(b.ToArray());
			objectName.SetText(nt.Name);
			objectData.showDrops = true;
			applyBut.Visible = true;
		}

		static void NewObject() {
			foreach (UIButton b in objectData.drops) {
				if (b is UITypeBox tb) {
					tb.DisableTyping();
					tb.UpdatePublicVar();
				}
			}
			foreach (object o in ntParams) {
				if (o == null)
					return;
			}
			try {
				WorldObjectBase obj = (WorldObjectBase)Activator.CreateInstance(nt, ntParams);
				WorldData.AddToRegion(WorldData.WorldToRegion(obj.worldX), WorldData.WorldToRegion(obj.worldY), obj);
				applyBut.Visible = false;
				SetCurrentObject(obj);
			} catch (Exception e) {
				System.Diagnostics.Debug.WriteLine(e.Message);
			}
		}

		string[] textures = new string[] {
			"button", "lightBack", "darkBack"
		};

		public void LoadTextures() {
			List<UIButton> buts = new List<UIButton>();
			foreach (GameSave.WorldObjectType t in Enum.GetValues(typeof(GameSave.WorldObjectType))) {
				buts.Add(new UIButton(0, 0, newObjects.width, newObjects.height, () => { SetupNewObject(t); newObjects.Visible = false; }, 1, "button") {
					displayLabel = new UIText(0, 0, .5f, 0, t.ToString())
				});
			}
			newObjects.SetDropdowns(buts.ToArray());
			TextureManager.RegisterTextures(textures);
			Screen.WindowOpen = true;
			Screen.CloseWindow("hud");
		}

		public void UnloadTextures() {
			TextureManager.UnRegisterTextures(textures);
			Screen.WindowOpen = false;
			Screen.OpenWindow("hud");
		}
	}
}
