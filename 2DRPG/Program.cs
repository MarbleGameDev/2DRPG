using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2DRPG {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
			objects.Add(new RotatingTriangle() { angle = 10f, });
			objects.Add(new RotatingTriangle());
            Application.Run(new Form1());
        }
		public static List<IRenderObject> objects = new List<IRenderObject>();
    }
}
