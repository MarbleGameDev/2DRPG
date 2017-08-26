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
		/// Checks whether it's possible to scroll the component and scrolls if so, returning success bool
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="dir"></param>
		/// <param name="sender"></param>
		/// <returns></returns>
		bool CheckScrollWheel(float x, float y, int dir, object sender);
		/// <summary>
		/// Scrolls to a normalized value between 0 and 1
		/// </summary>
		/// <param name="amount"></param>
		void ScrollTo(float amount);
	}
}
