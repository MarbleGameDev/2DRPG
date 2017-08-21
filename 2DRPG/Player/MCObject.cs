using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.Entities;

namespace _2DRPG.Player {
	class MCObject : World.Objects.WorldObjectControllable, IDamagable, IEffectable {

		public MCObject() : base("character") {
			MovementSpeed = 2.5f;
			width = 15;
			height = 33;
		}
		public void AddEffect(EntityEffect e) {
			throw new NotImplementedException();
		}

		public void EffectTick() {

		}

		public void ReceiveAttack(Attack a) {

		}
	}
}
