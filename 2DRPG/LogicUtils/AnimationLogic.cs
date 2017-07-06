﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using _2DRPG.World.Objects;

namespace _2DRPG.LogicUtils {
	public static partial class Logic {
		public static void AnimationLogic(object sender, ElapsedEventArgs e) {
			if (GameState.CurrentState == GameState.GameStates.Paused)
				return;
			lock (WorldData.currentRegions) {
				foreach (HashSet<WorldObjectBase> l in WorldData.currentRegions.Values.ToArray()) {
					foreach (WorldObjectBase o in l) {
						WorldObjectAnimated an = o as WorldObjectAnimated;
						if (an != null)
							an.SpriteUpdate();
					}
				}
			}
			lock (Screen.currentWindows) {
				foreach (HashSet<GUI.UIBase> b in Screen.currentWindows.Values)   //Render the GUI Objects
					foreach (GUI.UIBase u in b) {
						if (u is GUI.UIAnimated)
							((GUI.UIAnimated)u).SpriteUpdate();
				}
			}

		}

	}
}
