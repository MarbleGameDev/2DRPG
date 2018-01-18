using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.Save;

namespace _2DRPG.Items.ItemClasses {
	[Serializable]
	class BasicChestplate : ArmorItem {

		public override string Name { get; set; } = "Basic Chestplate";

		public BasicChestplate() : base(1) {

		}
	}
}
