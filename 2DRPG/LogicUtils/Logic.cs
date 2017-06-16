using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace _2DRPG.LogicUtils {
	public static partial class Logic {
		static double logicInterval = 1000d / 10;	//Logic is run 10 times per second
		static double animationInterval = 1000d / 30;	//Animation calls are run 30 times per second

		private static bool LogicEnabled {
			get { return movementTimer.Enabled && collisionTimer.Enabled && animationTimer.Enabled; }
			set { movementTimer.Enabled = value; collisionTimer.Enabled = value; animationTimer.Enabled = value; }
		}

		public static bool MovementEnabled { get { return movementTimer.Enabled; } set { movementTimer.Enabled = value; } }
		public static bool CollisionEnabled { get { return collisionTimer.Enabled; } set { collisionTimer.Enabled = value; } }
		public static bool AnimationEnabled { get { return animationTimer.Enabled; } set { animationTimer.Enabled = value; } }

		private static Timer movementTimer;
		private static Timer collisionTimer;
		private static Timer animationTimer;

		public static void LogicStart() {
			movementTimer = new Timer() { Interval = logicInterval };
			movementTimer.Elapsed += new ElapsedEventHandler(MovementLogic);
			collisionTimer = new Timer() { Interval = logicInterval };
			collisionTimer.Elapsed += new ElapsedEventHandler(CollisionLogic);
			animationTimer = new Timer() { Interval = animationInterval };
			animationTimer.Elapsed += new ElapsedEventHandler(AnimationLogic);
			movementTimer.Start();
			collisionTimer.Start();
			animationTimer.Start();
		}
		

	}
}
