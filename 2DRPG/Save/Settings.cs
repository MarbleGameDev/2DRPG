using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2DRPG.Save {
	[Serializable]
	public class Settings {
		public bool debugEnabled = true;
		public bool fullScreen = false;
		public bool VSync = true;
		public bool devWindow = false;
		public bool worldBuilder = true;
		public int windowx = 640, windowy = 360;
		public bool coOp = false;
		public Guid coOpUID = Guid.NewGuid();
		[JsonConverter(typeof(DictionaryConverter))]
		public Dictionary<Input.KeyInputs, Keys> keys = Input.keycodes;
	}
}
