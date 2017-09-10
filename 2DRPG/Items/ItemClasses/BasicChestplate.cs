using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.Items.ItemClasses {
	class BasicChestplate : ArmorItem {

		public override string Name { get; set; } = "Basic Chestplate";

		public BasicChestplate() : base(1) {

		}

		public BasicChestplate(GameSave.ItemStorage store) : base(store) {

		}

		public override GameSave.ItemStorage StoreObject() {
			GameSave.ItemStorage s = base.StoreObject();
			s.type = typeof(BasicChestplate);
			return s;
		}
	}
}
