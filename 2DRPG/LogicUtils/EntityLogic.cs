﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using _2DRPG.World.Objects;
using System.Diagnostics;

namespace _2DRPG.LogicUtils {
	static partial class Logic {
		//Logic for calculating entities
		public static float interactionDistance = 55f;

		public static WorldObjectBase interactableObject;

		private static WorldObjectBase nulled = new WorldObjectBase(float.MaxValue, float.MaxValue, 10, TextureManager.TextureNames.DEFAULT);

		static void EntityLogic(object sender, ElapsedEventArgs e) {
			if (GameState.CurrentState == GameState.GameStates.Paused || Screen.WindowOpen) {
				WorldData.interChar.Visible = false;
				return;
			}
			if (Save.SaveData.GameSettings.coOp && !Net.SessionManager.isHost)
				return;

			interactableObject = nulled;
			lock (WorldData.currentRegions) {
				foreach (World.Regions.RegionBase l in WorldData.currentRegions.Values) {
					foreach (WorldObjectBase o in l.GetWorldObjects()) {

						if (o is Entities.IEffectable en)
							en.EffectTick();
						if (o is World.Entities.StandardMob sn) {
							sn.AITick();
						}

						float dist = ObjectDistance(o, WorldData.player);
						if (dist <= interactionDistance) {
							if (o is IInteractable) {
								if (interactableObject == null || dist < ObjectDistance(interactableObject, WorldData.player))
									interactableObject = o;
							}
						}
					}
				}
			}
			if (interactableObject == null || interactableObject.Equals(nulled))
				interactableObject = null;
			if (interactableObject != null) {
				WorldData.interChar.SetScreenPosition(interactableObject.worldX, interactableObject.worldY + 15f);
				WorldData.interChar.Visible = true;
			} else {
				WorldData.interChar.Visible = false;
			}
		}
		/// <summary>
		/// Returns the distance from Object o to Object p
		/// </summary>
		/// <param name="o"></param>
		/// <param name="p"></param>
		/// <returns></returns>
		public static float ObjectDistance(WorldObjectBase o, WorldObjectBase p) {
			return (float)Math.Sqrt(Math.Pow(p.worldX - o.worldX, 2f) + Math.Pow(p.worldY - o.worldY, 2f));
		}
	}
}
