using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG {
	class Attack {
		public float attackDamage;
		private Action attackAction;

		public Attack(Action attack, float damage = 0) {
			attackAction = attack;
			attackDamage = damage;
		}
		public Attack(float damage = 0) { }

		public virtual Action GetAttackAction(object callingClass) {
			if (attackAction == null)
				return new Action(() => { });
			return attackAction;
		}


	}
}
