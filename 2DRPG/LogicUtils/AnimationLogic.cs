using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace _2DRPG.LogicUtils {
	public static partial class Logic {

		public static void AnimationLogic(object sender, ElapsedEventArgs e) {
			foreach (object o in WorldData.currentObjects) {
				if (o is IAnimated)
					((IAnimated)o).SpriteUpdate();
			}
		}

	}
}
