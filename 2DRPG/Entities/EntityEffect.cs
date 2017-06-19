using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.Entities {
	class EntityEffect {
		public float effectDuration;	//Duration in number of seconds
		private Action effectFunction;

		public EntityEffect(Action effect, int duration = 0) {
			this.effectFunction = effect;
			effectDuration = duration;
		}

		//Called Every 10th of a second
		public void EffectUpdate() {
			if (effectFunction != null)
				effectFunction.Invoke();

			effectDuration -= .1f;
		}

	}
}
