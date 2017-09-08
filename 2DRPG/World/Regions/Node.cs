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
		public int DistanceFromStart { get; private set; } = 0;    //Length of path from start node to current node
		public int DistanceToEnd { get; private set; }    //straight line distance from current node to end node
		public Node ParentNode { get; set; }

		/// <summary>
		/// Returns the F value for the node based on the parent node and the end node target
		/// Sets DistanceFromStart and DistanceToEnd and sets the parent node
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="end"></param>
		/// <returns></returns>
		public int DistanceEstimate(Node parent, Node end) {
			DistanceFromStart = CalculateDistFromStart(parent);
			DistanceToEnd = 10 * (Math.Abs(Location.X - end.Location.X) + Math.Abs(Location.Y - end.Location.Y));
			return DistanceFromStart + DistanceToEnd;
		}

		public int CalculateDistFromStart(Node parent) {
			return parent.DistanceFromStart + ((Math.Abs(parent.Location.X - Location.X) == 4 && Math.Abs(parent.Location.Y - Location.Y) == 4) ? (14) : (10));
		}


	}
}
