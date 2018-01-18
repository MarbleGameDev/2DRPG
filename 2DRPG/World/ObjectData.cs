using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.World {
	class ObjectData {
		public enum WorldObjects{
			Animated, Base, Collidable, Controllable,
			Dialogue, Inventory, Movable, MovableAnimated, SimpleItem
		};
	}
}
