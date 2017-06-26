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

		public override void Render() {
			arrayPosition[0] = worldX - size / 2 - .5f;
			arrayPosition[3] = worldX - size / 2 - .5f;
			arrayPosition[6] = worldX + size / 2 + .5f;
			arrayPosition[9] = worldX + size / 2 + .5f;
			arrayPosition[1] = worldY - size / 2 - .5f;
			arrayPosition[10] = worldY - size / 2 - .5f;
			arrayPosition[4] = worldY + size / 2 + .5f;
			arrayPosition[7] = worldY + size / 2 + .5f;
			base.Render();
		}
	}
}
