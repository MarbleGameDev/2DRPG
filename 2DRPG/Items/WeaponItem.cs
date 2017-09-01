using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.Items {
	abstract class WeaponItem : Item, IEquippable {
		public int ValidSlot { get; private set; }

		public WeaponItem() : base(1, false) {
			ValidSlot = 0;
		}
	}
}
