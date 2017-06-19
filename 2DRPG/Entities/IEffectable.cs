using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.Entities {
	interface IEffectable {

		void AddEffect(EntityEffect e);
		void EffectTick();	//Called every 10th of a second
	}
}
