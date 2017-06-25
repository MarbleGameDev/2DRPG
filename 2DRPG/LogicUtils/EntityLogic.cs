using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using _2DRPG.World.Objects;

namespace _2DRPG.LogicUtils {
	public static partial class Logic {
		//Logic for calculating entities
		static void EntityLogic(object sender, ElapsedEventArgs e) {
			List<WorldObjectBase>[] tobjects = WorldData.currentRegions.Values.ToArray();     //Render the World Objects
			foreach (List<WorldObjectBase> l in tobjects)
				foreach (WorldObjectBase o in l) {
				if (o is Entities.IEffectable) {
					((Entities.IEffectable)o).EffectTick();
				}
			}
		}
	}
}
