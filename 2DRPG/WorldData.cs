using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;
using System.Drawing.Imaging;

namespace _2DRPG {
	public static class WorldData {
		public static List<object> currentObjects = new List<object>(); //TODO: replace with a more accurate object type


		public static void LoadCurrentObjects() {
			currentObjects.Add(new TexturedObject());
		}

	}
}
