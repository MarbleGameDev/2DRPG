using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using _2DRPG.World.Regions;
using System.Diagnostics;

namespace _2DRPG.LogicUtils {
	static class PathLogic {

		public static void TestPath() {
			PathFind(new Point(-180, 70), new Point(-240, 70));
		}

		static bool showPath = false;

		public static List<Node> PathFind(Point start, Point end) {
			HashSet <Node> path = new HashSet<Node>(); //Open List
			HashSet<Node> closed = new HashSet<Node>(); //Closed List
			Point center = new Point((((start.X + end.X) / 2) / 4) * 4, (((start.Y + end.Y) / 2) / 4 ) * 4);
			NodeGrid grid = WorldData.GetNodeGrid(center);
			Node st = grid.GetNode(start);
			Node ed = grid.GetNode(end);
			Node current = st;
			if (ed == null || st == null)
				return null;
			path.Add(st);
			bool atend = false;

			if (showPath)
				lock (WorldData.currentRegions)
					WorldData.tempRender.Clear();

			do {
				List<Node> surrounding = grid.GetSurroundingNodes(current);
				if (surrounding == null) {
					break;
				}
				//Move from surrounding list to the open path
				for (int i = 0; i < surrounding.Count; i++) {
					if (surrounding[i].IsOpen && !closed.Contains(surrounding[i]))
						if (!path.Contains(surrounding[i])) {
							path.Add(surrounding[i]);
							surrounding[i].ParentNode = current;
						} else {
							int newG = surrounding[i].CalculateDistFromStart(current);
							if (newG < surrounding[i].DistanceFromStart) {
								surrounding[i].DistanceEstimate(current, ed);
								surrounding[i].ParentNode = current;
							}
						}
				}

				//Finds best F value from surrounding nodes
				int bestTotal = int.MaxValue;
				Node bestNode = null;
				foreach(Node nd in path) {
					int dist = nd.DistanceEstimate(current, ed);
					if (dist < bestTotal) {
						bestNode = nd;
						bestTotal = dist;
					}
				}
				if (bestNode == null) {
					break;
				}

				path.Remove(bestNode);
				closed.Add(bestNode);
				current = bestNode;

				//If close enough, end the search for the end point
				if (Math.Abs(ed.Location.X - bestNode.Location.X) < 4 && Math.Abs(ed.Location.Y - bestNode.Location.Y) < 4)
					atend = true;
			} while (!atend);
			List<Node> routedPath = new List<Node>() { current };
			while (true) {
				if (current == st)
					break;
				else {
					current = current.ParentNode;
					routedPath.Add(current);
				}
			}
			routedPath.Reverse();

			if (showPath)
				lock (WorldData.currentRegions) {
					foreach (Node nd in path) {
						WorldData.tempRender.Add(new World.Objects.WorldObjectBase(nd.Location.X, nd.Location.Y, "button", 1, 1));
					}
					foreach (Node nd in routedPath)
						WorldData.tempRender.Add(new World.Objects.WorldObjectBase(nd.Location.X, nd.Location.Y, "textBox", 1, 1));

				}

			return routedPath;
		}
		/// <summary>
		/// Adds the boarder amount to each side of the quad array passed
		/// </summary>
		/// <param name="quadPosition"></param>
		/// <param name="boarder"></param>
		/// <returns></returns>
		public static float[] ExpandPosition(float[] quadPosition, float boarder) {
			float[] tempPos = new float[12];
			quadPosition.CopyTo(tempPos, 0);
			tempPos[0] -= boarder;
			tempPos[3] -= boarder;
			tempPos[6] += boarder;
			tempPos[9] += boarder;
			tempPos[1] -= boarder;
			tempPos[4] += boarder;
			tempPos[7] += boarder;
			tempPos[10] -= boarder;

			return tempPos;
		}

		/// <summary>
		/// Returns the Pythagorean distance between point A and point B
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static float DistanceBetweenPoints(Point a, Point b) {
			return (float)Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
		}
	}
}
