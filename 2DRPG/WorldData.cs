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
			TexturedObject t = new TexturedObject();
			TexturedObject j = new TexturedObject();
			t.MoveRelative(0.2f, 0, -.5f);
			j.MoveRelative(0, 0.1f, -.6f);	//Testing two objects of differing depths
			currentObjects.Add(t);
			currentObjects.Add(j);
		}

	}
}
