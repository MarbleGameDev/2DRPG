using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace _2DRPG {
	public static partial class Logic {

		//Logic for calculating movement
		static void MovementLogic(object sender, ElapsedEventArgs e) {
			System.Diagnostics.Debug.WriteLine("Tick");
			WorldData.currentObjects.Add(new RotatingTriangle());
		}
	}
}
