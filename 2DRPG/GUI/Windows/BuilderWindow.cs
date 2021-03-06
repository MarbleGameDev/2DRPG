﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.World.Objects;
using _2DRPG.World;
using System.Reflection;
using _2DRPG.World.Entities;
using _2DRPG.Entities;
using System.Windows.Forms;

namespace _2DRPG.GUI.Windows {
	class BuilderWindow : IWindow {

		private static WorldObjectBase currentObject;
		private static StandardMob currentMob;
		public static bool checkWorldObjects = false;

		private static bool moving = false;

		public static void SetCurrentObject(WorldObjectBase obj) {
			currentObject = obj;
			checkWorldObjects = false;
			Screen.OpenWindow("worldBuilder");
			tabs.SetTab(0);
			UpdateObjectInfo();
		}

		static UIDropdownButton objectData = new UIDropdownButton(220, 100, 100, 20, 3, TextureManager.TextureNames.button, null, null) { hideTop = true };
		static UIDropdownButton newObjects = new UIDropdownButton(0, 100, 100, 20, 2, TextureManager.TextureNames.button, new UIText(0, 100, .5f, 1, "Select Object Type")) { Visible = false, hideTop = true, showDrops = true, displaySize = 4 };
		static UIButton applyBut = new UIButton(185, -140, 65, 8, 1, TextureManager.TextureNames.button) {
			displayLabel = new UIText(188, -140, .5f, 0, "Apply"),
			buttonAction = () => { NewObject(); }, Visible = false };
		static UIButton moveBut = new UIButton(145, 120, 25, 8, 1, TextureManager.TextureNames.button) {
			displayLabel = new UIText(145, 122, .5f, 0, "Move"),
			buttonAction = () => { StartMovement(); }, Visible = false };
		static UIText objectName = new UIText(220, 125, .5f, 1, "No Object Selected") { textColor = System.Drawing.Color.Aqua};
		static UIText entityName = new UIText(220, 125, .5f, 1, "Selected Object is not Entity") { textColor = System.Drawing.Color.Aqua };

		static UIDropdownButton AIType = new UIDropdownButton(240, 100, 40, 10, 3, TextureManager.TextureNames.button, new UIText(240, 100, .5f, 2, ""), null) { Visible = false };
		static UIDropdownButton AIActive = new UIDropdownButton(240, 70, 40, 10, 3, TextureManager.TextureNames.button, new UIText(240, 70, .5f, 2, ""), new UIButton[] {
			new UIButton(() => {
				currentMob.mobAI.active = true;
				AIActive.displayLabel.SetText("True");
				AIActive.ToggleDropdowns();
			}){ displayLabel = new UIText(0, 0, .5f, 1, "True")}, 
			new UIButton(() => {
				currentMob.mobAI.active = false;
				AIActive.displayLabel.SetText("False");
				AIActive.ToggleDropdowns();
			}){ displayLabel = new UIText(0, 0, .5f, 1, "False")}
		}) { Visible = false };

		static UITab tabs = new UITab(190, 170, 35, 10, 3, new UIButton[] {
			new UIButton(TextureManager.TextureNames.textBox) { displayLabel = new UIText(0, 0, .5f, 1, "Objects")},
			new UIButton(TextureManager.TextureNames.textBox) { displayLabel = new UIText(0, 0, .5f, 1, "Entities")}
		}, new HashSet<UIBase>[] {
			new HashSet<UIBase> {
			new UIButton(155, 140, 35, 8,1, TextureManager.TextureNames.button){
				displayLabel = new UIText(155, 142, .5f, 0, "Select Object"),
				buttonAction = () => { Screen.CloseWindow("worldBuilder"); checkWorldObjects = true; } },
			new UIButton(300, 140, 20, 8, 1, TextureManager.TextureNames.button){
				displayLabel = new UIText(300, 142, .5f, 0, "Delete"),
				buttonAction = () => { WorldData.RemoveObject(currentObject); } },
			new UIButton(235, 140, 35, 8, 1, TextureManager.TextureNames.button){
				displayLabel = new UIText(235, 142, .5f, 0, "New Object"),
				buttonAction = () => { newObjects.Visible = !newObjects.Visible; } },
			objectData, newObjects, applyBut, objectName, moveBut
			},
			new HashSet<UIBase> {
				new UIText(160, 100, .5f, 3, "AI Type: "), AIType,
				new UIText(160, 70, .5f, 3, "AI Active: "), AIActive, 
				entityName
			}
		});

		HashSet<UIBase> screenObjects = new HashSet<UIBase>() {
			new UIBase(220, 0, 100, 180, 4, TextureManager.TextureNames.darkBack),
			new UIButton(312, 172, 8, 8,1, TextureManager.TextureNames.exit){
			buttonAction = () => { Screen.CloseWindow("worldBuilder"); Screen.InvokeSelection(); } },
			new UIButton(288, -170, 30, 8, 1, TextureManager.TextureNames.button){
				displayLabel = new UIText(288, -168, .5f, 0, "Save Game"),
				buttonAction = () => { Save.SaveData.SaveGameData(); }},
			tabs
		};

		public HashSet<UIBase> LoadObjects() {
			List<UIButton> tempbuts = new List<UIButton>();
			foreach(AIBase.AIType s in Enum.GetValues(typeof(AIBase.AIType))) {
				tempbuts.Add(new UIButton(TextureManager.TextureNames.button) { displayLabel = new UIText(0, 0, .5f, 1, s.ToString()), buttonAction = () => {
					AIType.displayLabel.SetText(s.ToString());
					currentMob.mobAI.type = s;
					AIType.ToggleDropdowns();
				} });
			}
			AIType.SetDropdowns(tempbuts.ToArray());
			AIType.displaySize = (tempbuts.Count > 3) ? 3.5f : tempbuts.Count;
			if (currentObject != null)
				UpdateObjectInfo();
			return screenObjects;
		}
		public HashSet<UIBase> GetScreenObjects() {
			return screenObjects;
		}

		static void UpdateObjectInfo() {

			if (currentObject == null)
				return;

			if (currentObject is StandardMob s) {
				//isEntity = true;
				AIType.Visible = true;
				AIActive.Visible = true;
				currentMob = s;
				entityName.SetText(currentObject.GetType().Name);
				AIType.displayLabel.SetText(currentMob.mobAI.type.ToString());
				AIActive.displayLabel.SetText(currentMob.mobAI.active.ToString());
			} else {
				AIType.Visible = false;
				AIActive.Visible = false;
				entityName.SetText("Selected Object is not Entity");
			}

			if (objectData != null) {
				List<UIButton> b = new List<UIButton>();
				FieldInfo[] props = currentObject.GetType().GetFields().Where(prop => Attribute.IsDefined(prop, typeof(Editable))).ToArray();
				foreach (FieldInfo f in props) {
					UITypeBox tb = new UITypeBox(0, 0, objectData.width1, objectData.height1, 2, 1, TextureManager.TextureNames.button);
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
				objectData.displaySize = (b.Count > 5) ? 5.5f : b.Count;
				objectData.SetDropdowns(b.ToArray());
				objectName.SetText(currentObject.GetType().Name);
				objectData.showDrops = true;
				moveBut.Visible = true;
			} else {
				objectData.displayLabel.SetText("No Object Selected");

			}
		}
		static Type nt;
		static object[] ntParams;
		static void SetupNewObject(ObjectData.WorldObjects newType) {
			switch (newType) {
				case ObjectData.WorldObjects.Animated:
					nt = typeof(WorldObjectAnimated);
					break;
				case ObjectData.WorldObjects.Base:
					nt = typeof(WorldObjectBase);
					break;
				case ObjectData.WorldObjects.Collidable:
					nt = typeof(WorldObjectCollidable);
					break;
				case ObjectData.WorldObjects.Controllable:
					nt = typeof(WorldObjectControllable);
					break;
				case ObjectData.WorldObjects.Dialogue:
					nt = typeof(WorldObjectDialogue);
					break;
				case ObjectData.WorldObjects.Inventory:
					nt = typeof(WorldObjectInventory);
					break;
				case ObjectData.WorldObjects.SimpleItem:
					nt = typeof(WorldObjectSimpleItem);
					break;
				case ObjectData.WorldObjects.Movable:
					nt = typeof(WorldObjectMovable);
					break;
				case ObjectData.WorldObjects.MovableAnimated:
					nt = typeof(WorldObjectMovableAnimated);
					break;
				default:
					System.Diagnostics.Debug.WriteLine("No code path found for this new object!");
					return;
			}
			List<UIButton> b = new List<UIButton>();
			ParameterInfo[] parms = nt.GetConstructors()[0].GetParameters();
			ntParams = new object[parms.Length];
			int counter = 0;
			foreach (ParameterInfo p in parms) {
				UITypeBox tb = new UITypeBox(0, 0, objectData.width1, objectData.height1, 2, 1, TextureManager.TextureNames.button);
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

		static void StartMovement() {
			Screen.InvokeSelection();
			if (moving) {
				StopMovement();
				return;
			} else {
				moving = true;
			}
			Input.RedirectKeys = true;
			Input.DirectKeyCode += CheckKey;
			moveBut.displayLabel.textColor = System.Drawing.Color.Aqua;
		}
		static void StopMovement() {
			moving = false;
			Input.DirectKeyCode -= CheckKey;
			Input.RedirectKeys = false;
			moveBut.displayLabel.textColor = System.Drawing.Color.Black;
			UpdateObjectInfo();
		}

		static void CheckKey(Keys k) {
			switch (k) {
				case Keys.Enter:
					StopMovement();
					UpdateObjectInfo();
					break;
				case Keys.Left:
					currentObject.SetWorldPosition(currentObject.worldX - 1, currentObject.worldY);
					break;
				case Keys.Right:
					currentObject.SetWorldPosition(currentObject.worldX + 1, currentObject.worldY);
					break;
				case Keys.Up:
					currentObject.SetWorldPosition(currentObject.worldX, currentObject.worldY + 1);
					break;
				case Keys.Down:
					currentObject.SetWorldPosition(currentObject.worldX, currentObject.worldY - 1);
					break;
			}
		}

		Texture[] textures = new Texture[] {
			TextureManager.TextureNames.button, TextureManager.TextureNames.lightBack, TextureManager.TextureNames.darkBack, TextureManager.TextureNames.textBox, TextureManager.TextureNames.exit
		};

		public void LoadTextures() {
			List<UIButton> buts = new List<UIButton>();
			foreach (ObjectData.WorldObjects t in Enum.GetValues(typeof(ObjectData.WorldObjects))) {
				buts.Add(new UIButton(0, 0, newObjects.width1, newObjects.height1, 1, TextureManager.TextureNames.button) {
					displayLabel = new UIText(0, 0, .5f, 0, t.ToString()),
					buttonAction = () => { SetupNewObject(t); newObjects.Visible = false; }
				});
			}
			newObjects.SetDropdowns(buts.ToArray());
			TextureManager.RegisterTextures(textures);
			Screen.WindowOpen = true;
			Screen.CloseWindow("hud");
		}

		public void UnloadTextures() {
			StopMovement();
			TextureManager.UnRegisterTextures(textures);
			Screen.WindowOpen = false;
			Screen.OpenWindow("hud");
		}
	}
}
