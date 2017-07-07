using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using _2DRPG.World.Objects;

namespace _2DRPG.LogicUtils {
	static partial class Logic {
		//Logic for calculating entities
		public static float interactionDistance = 55f;

		public static WorldObjectInteractable interactableObject;
		
		static void EntityLogic(object sender, ElapsedEventArgs e) {
			if (GameState.CurrentState == GameState.GameStates.Paused)
				return;

			interactableObject = new WorldObjectInteractable(float.MaxValue, float.MaxValue);
			lock (WorldData.currentRegions.Values.ToArray().SyncRoot) {     //Render the World Objects
				foreach (HashSet<WorldObjectBase> l in WorldData.currentRegions.Values.ToArray())
					foreach (WorldObjectBase o in l) {
						if (o is Entities.IEffectable en)
							en.EffectTick();
						float dist = ObjectDistance(o, WorldData.controllableOBJ);
						if (dist <= interactionDistance) {
							if (o is WorldObjectInteractable io) {
								if (ObjectDistance(io, WorldData.controllableOBJ) < ObjectDistance(interactableObject, WorldData.controllableOBJ))
									interactableObject = io;
							}
						}
					}
			}
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
		private static float ObjectDistance(WorldObjectBase o, WorldObjectBase p) {
			return (float)Math.Sqrt(Math.Pow(p.worldX - o.worldX, 2f) + Math.Pow(p.worldY - o.worldY, 2f));
		}
	}
}
