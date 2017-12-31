using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2DRPG {
    static class Program {

		public static Form1 mainForm;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

			//Add any pre-game checks here
			if (!Save.SaveData.CheckSaveState()) {
				MessageBox.Show("Could not find game installation files, please check installation directory. If you believe this is incorrect, please file a bug report.", "Game Load Error");
				Application.Exit();
				return;
			}

			mainForm = new Form1();
			Application.Run(mainForm);
		}
    }
}
