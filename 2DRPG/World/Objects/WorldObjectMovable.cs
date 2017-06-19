﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.World.Objects {
	class WorldObjectMovable : WorldObjectBase, IMovable {

		public float MovementSpeed { get; set; }
		public float VelocityX { get; set; }
		public float VelocityY { get; set; }

		public WorldObjectMovable() : base() { }
		public WorldObjectMovable(string textureName) : base(textureName) { }
		public WorldObjectMovable(float x, float y, string textureName) : base(textureName) {
			SetWorldPosition(x, y);
		}

		public void MoveRelative(float x = 0, float y = 0) {
			worldX += x;
			worldY += y;
			ShiftScreenPosition(x, y);
		}
	}
}