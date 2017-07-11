using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI.Interaction {
	public abstract class InteractionBase {

		public Action nextNode;

		public abstract void Render();

		public abstract void Setup();

		public abstract GameSave.InteractionObjectStorage StoreObject();

		public abstract override string ToString();

		public virtual void ModificationAction() { }
	}
}
