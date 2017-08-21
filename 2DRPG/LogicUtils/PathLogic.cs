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
			List<Node> path = new List<Node>();
			Point start = new Point(170, 90);
			Point end = new Point((int)WorldData.CurrentX, (int)WorldData.CurrentY);
			Point center = new Point((start.X + end.X) / 2, (start.Y + end.Y) / 2);
			NodeGrid grid = WorldData.GetNodeGrid(center);
			Node st = grid.GetNode(start);
			Node ed = grid.GetNode(end);
			if (ed == null || st == null)
				return;
			path.Add(st);
			bool atend = false;

			if (showPath)
				lock (WorldData.currentRegions)
					WorldData.tempRender.Clear();

			do {
				Stopwatch s = new Stopwatch();
				s.Start();
				List<Node> surrounding = grid.GetSurroundingNodes(path[path.Count - 1], ed);
				if (surrounding == null)
					break;
				float bestTotal = float.MaxValue;
				Node bestNode = null;
				for (int i = 0; i < surrounding.Count; i++) {
					if (surrounding[i].IsOpen && surrounding[i].TotalDistanceEstimate < bestTotal) {
						bestNode = surrounding[i];
						bestTotal = bestNode.TotalDistanceEstimate;
					}
				}
				if (bestNode == null)
					break;
				if (path.Count > 2)
					if (path[path.Count - 1].Equals(bestNode) || path[path.Count - 2].Equals(bestNode))
						break;
				path.Add(bestNode);
				if (Math.Abs(ed.Location.X - bestNode.Location.X) < 4 && Math.Abs(ed.Location.Y - bestNode.Location.Y) < 4)
					atend = true;
				if (s.Elapsed > TimeSpan.FromMilliseconds(100)) {
					System.Diagnostics.Debug.WriteLine("Uh Oh: " + s.Elapsed);
					break;
				}
			} while (!atend);
			if (showPath)
				lock (WorldData.currentRegions) {
					foreach (Node nd in path) {
						WorldData.tempRender.Add(new World.Objects.WorldObjectBase(nd.Location.X, nd.Location.Y, "button", 1, 1));
					}
				}
		}

		static bool showPath = true;

		public static List<Node> PathFind(Point start, Point end) {
			List<Node> path = new List<Node>();
			Point center = new Point((start.X + end.X) / 2, (start.Y + end.Y) / 2);
			NodeGrid grid = WorldData.GetNodeGrid(center);
			Node st = grid.GetNode(start);
			Node ed = grid.GetNode(end);
			if (ed == null || st == null)
				return null;
			path.Add(st);
			bool atend = false;

			if (showPath)
				lock (WorldData.currentRegions)
					WorldData.tempRender.Clear();

			do {
				List<Node> surrounding = grid.GetSurroundingNodes(path[path.Count - 1], ed);
				if (surrounding == null)
					break;
				float bestTotal = float.MaxValue;
				Node bestNode = null;
				for (int i = 0; i < surrounding.Count; i++) {
					if (surrounding[i].IsOpen && surrounding[i].TotalDistanceEstimate < bestTotal) {
						bestNode = surrounding[i];
						bestTotal = bestNode.TotalDistanceEstimate;
					}
				}
				if (bestNode == null)
					break;
				if (path.Count > 2)
					if (path[path.Count - 1].Equals(bestNode) || path[path.Count - 2].Equals(bestNode))
						break;
				path.Add(bestNode);
				if (Math.Abs(ed.Location.X - bestNode.Location.X) < 4 && Math.Abs(ed.Location.Y - bestNode.Location.Y) < 4)
					atend = true;
			} while (!atend);
			if (showPath)
				lock (WorldData.currentRegions) {
					foreach (Node nd in path) {
						WorldData.tempRender.Add(new World.Objects.WorldObjectBase(nd.Location.X, nd.Location.Y, "button", 1, 1));
					}
				}
			return path;
		}

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
	}
}
