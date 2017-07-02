using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.World.Objects {
	public class WorldObjectInteractable : WorldObjectBase, IInteractable {

		public Action interAction;

		public WorldObjectInteractable(float x, float y, float size) : base(x, y, "default") {

		}

		public void Interact() {
			if (interAction != null)
				interAction.Invoke();
		}
	}
}
