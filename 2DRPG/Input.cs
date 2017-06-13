using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2DRPG {
	static class Input {

		public static void KeySent(object sender, KeyEventArgs e) {
			System.Diagnostics.Debug.WriteLine(e.KeyCode);
		}

		public static void MouseSent(object sender, MouseEventArgs e) {
			System.Diagnostics.Debug.WriteLine(e.Button);
		}

	}
}
