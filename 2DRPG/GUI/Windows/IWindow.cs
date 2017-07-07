using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI.Windows {
	interface IWindow {
		void LoadTextures();
		void UnloadTextures();
		ref HashSet<UIBase> LoadObjects();
		ref HashSet<UIBase> GetScreenObjects();
	}
}
