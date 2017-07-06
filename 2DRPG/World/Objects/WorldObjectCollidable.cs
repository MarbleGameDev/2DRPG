﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.World.Objects {
	class WorldObjectCollidable : WorldObjectBase, ICollidable{
		public WorldObjectCollidable(float x, float y) : base(x, y, "default") {

		}
	}
}
