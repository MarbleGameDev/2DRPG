﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace _2DRPG.LogicUtils {
	public static partial class Logic {

		public static void AnimationLogic(object sender, ElapsedEventArgs e) {
			object[] worldObjects = WorldData.currentObjects.ToArray();
			foreach (object o in worldObjects) {
				if (o is IAnimated)
					((IAnimated)o).SpriteUpdate();
			}
		}

	}
}
