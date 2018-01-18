using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.World.Objects {
	public interface IMovable {
		float MovementSpeed { get; set; }
		void MoveRelative(float x = 0, float y = 0);
	}
}
