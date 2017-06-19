using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG {
	public interface IMovable {
		float MovementSpeed { get; set; }
		float VelocityX { get; set; }
		float VelocityY { get; set; }
		void MoveRelative(float x = 0, float y = 0);
	}
}
