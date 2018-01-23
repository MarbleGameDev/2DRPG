using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.World.Objects {
	interface IDelayable {
		int TickNum { get; set; }
		Action DelayAction { get; set; }
		void AnimationTick();
	}
}
