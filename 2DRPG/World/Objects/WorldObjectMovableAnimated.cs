using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.World.Objects {
	class WorldObjectMovableAnimated : WorldObjectAnimated, IMovable {

		public float MovementSpeed { get; set; }
		public float VelocityX { get; set; }
		public float VelocityY { get; set; }
		
		public WorldObjectMovableAnimated(string textureName) : base(textureName) { }
		public WorldObjectMovableAnimated(float x, float y, string textureName) : base(textureName) {
			SetWorldPosition(x, y);
		}

		public virtual void MoveRelative(float x = 0, float y = 0) {
			worldX += x;
			worldY += y;
			ShiftScreenPosition(x, y);
		}
	}
}
