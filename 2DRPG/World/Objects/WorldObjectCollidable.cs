using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.Save;

namespace _2DRPG.World.Objects {
	[Serializable]
	class WorldObjectCollidable : WorldObjectBase, ICollidable{

		/// <summary>
		/// Complete Declaration for WorldObjectCollidable
		/// </summary>
		/// <param name="x">X position in the world</param>
		/// <param name="y">Y position in the world</param>
		/// <param name="layer">Render Layer</param>
		/// <param name="textureName">Name of the texture</param>
		public WorldObjectCollidable(float x, float y, int layer, Texture textureName, float width = 16, float height = 16) : base(x, y, textureName, width, height) { }
	}
}
