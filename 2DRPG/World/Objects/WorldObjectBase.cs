using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.World.Objects {
	class WorldObjectBase : TexturedObject {
		public float worldX;
		public float worldY;

		public WorldObjectBase() : base() { }
		public WorldObjectBase(string textureName) : base(textureName) { }
		public WorldObjectBase(float x, float y, string textureName) : base(textureName) {
			SetWorldPosition(x, y);
		}
	
		/// <summary>
		/// Sets the position of the object in the world
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void SetWorldPosition(float x, float y) {
			worldX = x;
			worldY = y;
		}
	}
}
