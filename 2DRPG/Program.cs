using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
			//WorldData.currentObjects.Add(new TexturedObject());
			WorldData.LoadCurrentObjects();
			logic = new Thread(() => Logic.LogicStart());
			Application.Run(new Form1());
		}

		public static Thread logic;
    }
}
