using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using _2DRPG.World.Objects;

namespace _2DRPG.LogicUtils {
	public static partial class Logic {
		static void PhysicsLogic(object sender, ElapsedEventArgs e) {
			if (GameState.CurrentState == GameState.GameStates.Paused || Screen.WindowOpen)
				return;
			bool playerstuck = false;
			float[] contPos = new float[12];
			WorldData.player.quadPosition.CopyTo(contPos, 0);
			WorldObjectBase.ShiftPosition(contPos, WorldData.player.movementQueueX, WorldData.player.movementQueueY);
			lock (WorldData.currentRegions) {
				foreach (World.Regions.RegionBase l in WorldData.currentRegions.Values) {
					foreach (WorldObjectBase o in l.GetWorldObjects()) {
						if (o is ICollidable)
							if (CheckIntersection(o.quadPosition, contPos[0], contPos[1]) ||
								CheckIntersection(o.quadPosition, contPos[3], contPos[4]) ||
								CheckIntersection(o.quadPosition, contPos[6], contPos[7]) ||
								CheckIntersection(o.quadPosition, contPos[9], contPos[10]) ||
								CheckIntersection(o.quadPosition, (contPos[0] + contPos[9]) / 2, contPos[1]) ||
								CheckIntersection(o.quadPosition, contPos[0], (contPos[1] + contPos[4]) / 2) ||
								CheckIntersection(o.quadPosition, (contPos[3] + contPos[6]) / 2, contPos[4]) ||
								CheckIntersection(o.quadPosition, contPos[6], (contPos[7] + contPos[10]) / 2) ||
								CheckIntersection(o.quadPosition, (contPos[0] + contPos[9]) / 2, (contPos[1] + contPos[10]) / 2)) {
								playerstuck = true;
							}
					}
				}
			}
			if (!playerstuck) {
				Form1.AddOrthoUpdate(WorldData.player.UpdatePosition);
			} else {
				WorldData.player.movementQueueX = 0;
				WorldData.player.movementQueueY = 0;
			}
		}

		/// <summary>
		/// Returns true if the textX and testY are inside the coords passed
		/// </summary>
		/// <param name="coords"></param>
		/// <param name="testX"></param>
		/// <param name="testY"></param>
		/// <returns></returns>
		public static bool CheckIntersection(float[] coords, float testX, float testY) {
			int sideCount = coords.Length / 3;
			float[] xCoords = new float[sideCount];
			float[] yCoords = new float[sideCount];

			for (int i = 0; i < sideCount; i++) {
				xCoords[i] = coords[i * 3];
			}

			for (int i = 0; i < sideCount; i++) {
				yCoords[i] = coords[i * 3 + 1];
			}

			return PnPoly(xCoords, yCoords, testX, testY);
		}

		/**
		 * Shamelessly stolen code from Randolph Franklin
		 * https://www.ecse.rpi.edu/Homepages/wrf/Research/Short_Notes/pnpoly.html
		 * 		Converted to C# by Jeremy
		 * @param nvert
		 * @return
		 */
		private static bool PnPoly(float[] vertX, float[] vertY, float testX, float testY) {
			int nVert = vertX.Length, i, j;
			bool c = false;

			for (i = 0, j = nVert - 1; i < nVert; j = i++) {
				if (((vertY[i] > testY) != (vertY[j] > testY)) &&
						(testX < (vertX[j] - vertX[i]) * (testY - vertY[i]) / (vertY[j] - vertY[i]) + vertX[i]))
					c = !c;
			}

			return c;
		}
	}
}
