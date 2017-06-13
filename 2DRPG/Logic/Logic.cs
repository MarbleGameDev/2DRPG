using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace _2DRPG {
	public static partial class Logic {
		static double logicInterval = 1000d / 10;

		private static bool logicEnabled;

		public static void SetLogic(bool logic) {
			logicEnabled = logic;
			movementTimer.Enabled = logic;
			interactionTimer.Enabled = logic;
		}
		public static bool GetLogic() {
			return logicEnabled;
		}

		private static Timer movementTimer;
		private static Timer interactionTimer;

		public static void LogicStart() {
			System.Diagnostics.Debug.WriteLine(logicInterval);
			movementTimer = new Timer() { Interval = logicInterval };
			movementTimer.Elapsed += new ElapsedEventHandler(MovementLogic);
			interactionTimer = new Timer() { Interval = logicInterval };
			interactionTimer.Elapsed += new ElapsedEventHandler(InteractionLogic);
			movementTimer.Start();
			interactionTimer.Start();
		}
		

	}
}
