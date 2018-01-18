using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.Save;

namespace _2DRPG.GUI.Interaction {
	[Serializable]
	public abstract class InteractionBase {

		public Action nextNode;

		public abstract void Render();

		public abstract void Setup();
		public abstract void Takedown();

		public abstract override string ToString();

		public virtual void ModificationAction() { }
	}
}
