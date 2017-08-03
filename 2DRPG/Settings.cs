using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG {
	[Serializable()]
	public class Settings {
		public bool debugEnabled = true;
		public bool fullScreen = false;
		public bool VSync = true;
		public bool interactionEditor = false;
		public bool worldBuilder = true;
		public int windowx = 640, windowy = 360;
	}
}
