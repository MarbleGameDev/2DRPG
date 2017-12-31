﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.Entities;
using _2DRPG.Save;

namespace _2DRPG.Items.ItemClasses {
	class FoodBase : Item, IConsumable {

		public override string Name { get; set; } = "Generic Foodstuff";

		public FoodBase(int quantity, string name) : base(quantity, true) {
			Name = name;
		}
		public FoodBase(int quantity) : base(quantity, true) { }

		public FoodBase() : base(1, true) { }

		public FoodBase(RegionSave.ItemStorage store) : base(store){

		}

		public EntityEffect GetEffect() {
			throw new NotImplementedException();
		}

		public int GetHealth() {
			throw new NotImplementedException();
		}

		public override RegionSave.ItemStorage StoreObject() {
			RegionSave.ItemStorage s = base.StoreObject();
			s.type = typeof(FoodBase);
			return s;
		}
	}
}
