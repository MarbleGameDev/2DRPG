using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.Save;

namespace _2DRPG.Items {
	[Serializable]
	abstract class ArmorItem : Item, IEquippable {

		public int ValidSlot { get; private set; }

		public ArmorItem(int slot) : base(1, false) {
			ValidSlot = slot;
		}

	}
}
