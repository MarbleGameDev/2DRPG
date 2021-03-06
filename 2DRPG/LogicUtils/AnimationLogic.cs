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
			bool pos = !(Save.SaveData.GameSettings.coOp && !Net.SessionManager.isHost);
			lock (WorldData.currentRegions) {
				foreach (World.Regions.RegionBase l in WorldData.currentRegions.Values) {
					foreach (WorldObjectBase o in l.GetWorldObjects()) {
						if (pos && o is World.Entities.StandardMob sn)
							sn.UpdatePosition();
						if (o is WorldObjectAnimated an)
							an.AnimationTick();
						else if (o is IDelayable de)
							de.AnimationTick();
					}
				}
			}
			lock (Screen.currentWindows) {
				foreach (HashSet<GUI.UIBase> b in Screen.currentWindows.Values)   //Render the GUI Objects
					foreach (GUI.UIBase u in b) {
						if (u is GUI.UIAnimated am)
							am.SpriteUpdate();
				}
			}
		}

	}
}
