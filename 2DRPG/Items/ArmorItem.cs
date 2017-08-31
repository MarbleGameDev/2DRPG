using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.Items {
	abstract class ArmorItem : Item, IEquippable {


		public ArmorItem(int slot) : base(1, false) {
			ValidSlot = slot;
		}

		public int ValidSlot { get; private set; }
	}
}
