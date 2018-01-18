using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG {
	[Serializable]
	public class Texture {
		public string path;
		public uint glID;
		public ushort uses;

		public Texture(string path) {
			this.path = path;
			glID = 0;
			uses = 0;
		}
	}
}
