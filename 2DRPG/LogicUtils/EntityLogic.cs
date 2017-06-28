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
			if (GameState.CurrentState == GameState.GameStates.Paused)
				return;
			lock (WorldData.currentRegions.Values.ToArray().SyncRoot) {     //Render the World Objects
				foreach (HashSet<WorldObjectBase> l in WorldData.currentRegions.Values.ToArray())
					foreach (WorldObjectBase o in l) {
						if (o is Entities.IEffectable) {
							((Entities.IEffectable)o).EffectTick();
						}
					}
			}
		}
	}
}
