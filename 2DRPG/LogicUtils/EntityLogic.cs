using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace _2DRPG.LogicUtils {
	public static partial class Logic {
		//Logic for calculating entities
		static void EntityLogic(object sender, ElapsedEventArgs e) {
			World.Objects.WorldObjectBase[] worldObjects = WorldData.currentObjects.ToArray();
			foreach (World.Objects.WorldObjectBase o in worldObjects) {
				if (o is Entities.IEffectable) {
					((Entities.IEffectable)o).EffectTick();
				}
			}
		}
	}
}
