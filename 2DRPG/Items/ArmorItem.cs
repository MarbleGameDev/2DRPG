using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.Items {
	abstract class ArmorItem : Item, IEquippable {

		public int ValidSlot { get; private set; }

		public ArmorItem(int slot) : base(1, false) {
			ValidSlot = slot;
		}

		public ArmorItem(GameSave.ItemStorage store) : base(store){
			ValidSlot = Convert.ToInt32(store.extraData[0]);
		}

		public override GameSave.ItemStorage SerializeObject() {
			GameSave.ItemStorage s = base.SerializeObject();
			s.type = typeof(ArmorItem);
			s.extraData = new object[] { ValidSlot };
			return s;
		}

	}
}
