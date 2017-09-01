using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.Items.ItemClasses {
	class BasicChestplate : ArmorItem {

		public override string Name { get { return "Basic Chestplate"; } }

		public BasicChestplate() : base(1) {

		}

		public BasicChestplate(GameSave.ItemStorage store) : base(store) {

		}

		public override GameSave.ItemStorage SerializeObject() {
			GameSave.ItemStorage s = base.SerializeObject();
			s.type = typeof(BasicChestplate);
			return s;
		}
	}
}
