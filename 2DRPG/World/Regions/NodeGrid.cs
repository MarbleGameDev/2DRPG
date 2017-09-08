using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace _2DRPG.World.Regions {
	class NodeGrid {
		//[500, 500] is the center
		public Node[,] nodes = new Node[640, 320];
		public int centerX, centerY;

		public bool CheckNodeOpen(int x, int y) {
			return nodes[x, y].IsOpen;
		}

		public void SetNodes(Node[,] nods) {
			nodes = nods;
		}
		public void SetCenter(int x, int y) {
			centerX = x;
			centerY = y;
		}
		/// <summary>
		/// Assumes that the location is within the 600x600 grid
		/// </summary>
		/// <param name="location"></param>
		/// <returns></returns>
		public Node GetNode(Point location) {
			int dx = (location.X - centerX) / 4;
			int dy = (location.Y - centerY) / 4;
			if (Math.Abs(dx) >= 74 || Math.Abs(dy) >= 74)
				return null;
			return nodes[dx + 75, dy + 75];
		}

		public List<Node> GetSurroundingNodes(Node nd) {
			int dx = (nd.Location.X - centerX) / 4;
			int dy = (nd.Location.Y - centerY) / 4;
			if (Math.Abs(dx) >= 74 || Math.Abs(dy) >= 74)
				return null;

			List<Node> nods = new List<Node>(8) {
				nodes[dx + 75, dy + 76],  //Iterating clockwise from Noon
				nodes[dx + 76, dy + 76],
				nodes[dx + 76, dy + 75],
				nodes[dx + 76, dy + 74],
				nodes[dx + 75, dy + 74],
				nodes[dx + 74, dy + 74],
				nodes[dx + 74, dy + 75],
				nodes[dx + 74, dy + 76]
			};
			return nods;
		}
	}
}
