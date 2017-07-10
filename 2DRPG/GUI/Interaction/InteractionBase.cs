using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI.Interaction {
	abstract class InteractionBase {

		public Action nextNode;

		public abstract void Render();

		public abstract void Setup();
	}
}
