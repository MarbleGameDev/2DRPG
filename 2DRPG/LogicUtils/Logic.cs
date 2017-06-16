using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace _2DRPG.LogicUtils {
	public static partial class Logic {
		static double logicInterval = 1000d / 10;

		private static bool logicEnabled;

		public static void SetLogic(bool logic) {
			logicEnabled = logic;
			movementTimer.Enabled = logic;
			CollisionTimer.Enabled = logic;
		}
		public static bool GetLogic() {
			return logicEnabled;
		}

		private static Timer movementTimer;
		private static Timer CollisionTimer;

		public static void LogicStart() {
			movementTimer = new Timer() { Interval = logicInterval };
			movementTimer.Elapsed += new ElapsedEventHandler(MovementLogic);
			CollisionTimer = new Timer() { Interval = logicInterval };
			CollisionTimer.Elapsed += new ElapsedEventHandler(CollisionLogic);
			movementTimer.Start();
			CollisionTimer.Start();
		}
		

	}
}
