using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI {
	interface IScrollable {
		void ScrollWheel(int dir, object sender);
		bool CheckCoords(float x, float y);
		/// <summary>
		/// Scrolls to a normalized value between 0 and 1
		/// </summary>
		/// <param name="amount"></param>
		void ScrollTo(float amount);
	}
}
