using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG {
	class EffectAttack : Attack {
		Entities.EntityEffect effects;

		public EffectAttack(Action attack, float damage = 0) : base(attack, damage) { }
		public EffectAttack(Action attack, Entities.EntityEffect effect, float damage = 0) : base(attack, damage) {
			effects = effect;
		}
		public EffectAttack(Entities.EntityEffect effect, float damage = 0) : base(damage) {
			effects = effect;
		}

		public override Action GetAttackAction(object cl) {
			return new Action(() => {
				if (cl is Entities.IEffectable) {	//If the object being attacked is Effectable, add an effect to the object
					Debug.WriteLine("Effecto");
					((Entities.IEffectable)cl).AddEffect(effects);
				}
			});
		}
	}
}
