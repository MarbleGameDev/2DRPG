﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using _2DRPG.World.Objects;

namespace _2DRPG.LogicUtils {
	public static partial class Logic {
		static void PhysicsLogic(object sender, ElapsedEventArgs e) {
			if (GameState.CurrentState == GameState.GameStates.Paused)
				return;
			WorldData.controllableOBJ.MoveRelative();
			lock (WorldData.currentRegions.Values.ToArray().SyncRoot) {     //Render the World Objects
				foreach (HashSet<WorldObjectBase> l in WorldData.currentRegions.Values.ToArray()) {
					foreach (WorldObjectBase o in l) {
						if (o is IMovable) {
							float[] testPoints = o.arrayPosition;
							foreach (WorldObjectBase j in l) {
								if (j != o) {
									if (CheckIntersection(j.arrayPosition, testPoints[0], testPoints[1]))       //1 is bottom left
										FixCollision((IMovable)o, 1);
									else if (CheckIntersection(j.arrayPosition, testPoints[3], testPoints[4]))  //2 is upper left
										FixCollision((IMovable)o, 2);
									else if (CheckIntersection(j.arrayPosition, testPoints[6], testPoints[7]))  //3 is upper right
										FixCollision((IMovable)o, 3);
									else if (CheckIntersection(j.arrayPosition, testPoints[9], testPoints[10])) //4 is bottom right
										FixCollision((IMovable)o, 4);
								}
							}
						}

					}
				}
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

		static bool fixCollisionEnabled = false;
		private static void FixCollision(IMovable o, int direction) {
			if (!fixCollisionEnabled)
				return;
			//System.Diagnostics.Debug.WriteLine("Collision: " + direction);
			switch (direction) {
				default:
				case 1:
					o.MoveRelative(.05f, .05f);
					break;
				case 2:
					o.MoveRelative(.05f, -.05f);
					break;
				case 3:
					o.MoveRelative(-.05f, -.05f);
					break;
				case 4:
					o.MoveRelative(-.05f, .05f);
					break;
			}

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
