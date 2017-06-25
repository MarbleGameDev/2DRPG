using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using _2DRPG.World.Objects;

namespace _2DRPG.LogicUtils {
	public static partial class Logic {

		public static void AnimationLogic(object sender, ElapsedEventArgs e) {
			List<WorldObjectBase>[] tobjects = WorldData.currentRegions.Values.ToArray();     //Render the World Objects
			foreach (List<WorldObjectBase> l in tobjects)
				foreach (WorldObjectBase o in l) {
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
