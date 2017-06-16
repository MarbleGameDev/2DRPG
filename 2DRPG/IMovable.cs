using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG {
	public interface IMovable {
		void MoveRelative(float x = 0, float y = 0);
	}
}
