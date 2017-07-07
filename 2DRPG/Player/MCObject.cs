using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.Entities;

namespace _2DRPG.Player {
	class MCObject : WorldObjectControllable, IDamagable, IEffectable {

		public MCObject() : base("character") {
			MovementSpeed = 2.5f;
			size = 32f;
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
