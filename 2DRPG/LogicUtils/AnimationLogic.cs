using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace _2DRPG.LogicUtils {
	public static partial class Logic {

		public static void AnimationLogic(object sender, ElapsedEventArgs e) {
			World.Objects.WorldObjectBase[] worldObjects = WorldData.currentObjects.ToArray();
			foreach (object o in worldObjects) {
				if (o is World.Objects.WorldObjectAnimated)
					((World.Objects.WorldObjectAnimated)o).SpriteUpdate();
			}
			GUI.UIBase[] guiObjects = Screen.UIObjects.ToArray();
			foreach (GUI.UIBase u in guiObjects) {
				if (u is GUI.UIAnimated)
					((GUI.UIAnimated)u).SpriteUpdate();
			}

		}

	}
}
