using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.World.Regions {
	class Node {
		public Point Location { get; set; }
		public bool IsOpen { get; set; }
		public float DistanceFromStart { get; private set; }    //Length of path from start node to current node
		public float DistanceToEnd { get; private set; }    //straight line distance from current node to end node
		public float TotalDistanceEstimate { get { return DistanceFromStart + DistanceToEnd; } }
		public Node ParentNode { get; set; }

		public void CalculateDistances(Node previous, Node end) {
			DistanceFromStart = previous.DistanceFromStart + (float)Math.Ceiling(Math.Sqrt(Math.Pow(Location.X - previous.Location.X, 2) + Math.Pow(Location.Y - previous.Location.Y, 2)));
			DistanceToEnd = (float)Math.Sqrt(Math.Pow(end.Location.X - Location.X, 2) + Math.Pow(end.Location.Y - Location.Y, 2));
			ParentNode = previous;
		}
	}
}
