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

		static UIDropdownButton objectData = new UIDropdownButton(220, 100, 100, 20, new UIText(220, 100, .5f, 1, "No Object Selected"));

		public HashSet<UIBase> screenObjects = new HashSet<UIBase>() {
			new UIBase(220, 0, 100, 180, 3, "darkBack"),
			new UIButton(310, 170, 10, 10, () => { Screen.CloseWindow("worldBuilder"); },1, "button"){ displayLabel = new UIText(317, 175, 1f, 0, "X") },
			new UIButton(185, 140, 65, 8, () => { Screen.CloseWindow("worldBuilder");  checkWorldObjects = true; },1, "button"){ displayLabel = new UIText(188, 142, .5f, 0, "Select Object") },
			new UIButton(185, -160, 65, 8, () => { SaveData.SaveGame(); }, 1, "button"){ displayLabel = new UIText(188, -160, .5f, 0, "Save Game")},
			objectData,
		};

		public ref HashSet<UIBase> LoadObjects() {
			return ref screenObjects;
		}
		public ref HashSet<UIBase> GetScreenObjects() {
			return ref screenObjects;
		}

		static void UpdateObjectInfo() {
			if (objectData != null) {
				List<UIButton> b = new List<UIButton>();
				FieldInfo[] props = currentObject.GetType().GetFields().Where(prop => Attribute.IsDefined(prop, typeof(Editable))).ToArray();
				foreach (FieldInfo f in props) {
					UITypeBox tb = new UITypeBox(0, 0, objectData.width, objectData.height, 2, "button");
					tb.text.SetText(f.Name + ":" + f.GetValue(currentObject).ToString());
					tb.valueAction = () => {
						string val = tb.text.GetText();
						val = val.Substring(val.IndexOf(':')+1);
						if (f.FieldType.Equals(typeof(String)))
							f.SetValue(currentObject, val);
						else
							f.SetValue(currentObject, int.Parse(val));
						currentObject.ModificationAction().Invoke();
					};
					b.Add(tb);
				}
				objectData.displaySize = (b.Count > 5) ? 5 : b.Count;
				objectData.SetDropdowns(b.ToArray());
				objectData.displayLabel.SetText(currentObject.GetType().Name);
				objectData.showDrops = true;
			} else {
				objectData.displayLabel.SetText("No Object Currently Selected");
			}
		}

		string[] textures = new string[] {
			"button", "lightBack", "darkBack"
		};

		public void LoadTextures() {
			TextureManager.RegisterTextures(textures);
			Screen.CloseWindow("hud");
		}

		public void UnloadTextures() {
			TextureManager.UnRegisterTextures(textures);
			Screen.OpenWindow("hud");
		}
	}
}
