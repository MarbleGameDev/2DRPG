using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.Entities;

namespace _2DRPG.World.Entities {
	class WorldEntity : Objects.WorldObjectMovable {

		/// <summary>
		/// Complete Declaration for WorldEntity
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="layer"></param>
		/// <param name="texture"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public WorldEntity(float x, float y, int layer, Texture texture, float width = 16, float height = 16) : base(x, y, layer, texture, width, height) {

		}

		public WorldEntity(float x, float y, Texture texture) : base(x, y, 10, texture) {

		}
	}
}
