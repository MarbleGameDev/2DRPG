using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI {
	interface IScrollable {
		void ScrollWheel(int dir);
		bool CheckCoords(float x, float y);
		void ScrollTo(float amount);
	}
}
