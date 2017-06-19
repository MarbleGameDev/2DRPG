using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI {
	class UIBase : TexturedObject {
		public UIBase() : base() { }
		public UIBase(string textureName) : base(textureName) { }
		public UIBase(float x, float y, int layerName, string textureName) : base(x, y, layerName, textureName) { }
	}
}
