using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.World.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace _2DRPG.Entities {
	class AIBase {

		public AIType type;

		private StandardMob attachedObject;

		public AIBase(StandardMob obj) {
			attachedObject = obj;
		}

		public void AITick() {
			switch (type) {
				case AIType.Aggressive:
					AggressiveTick();
					break;
				case AIType.Passive:
					PassiveTick();
					break;
				case AIType.Neutral:
					NeutralTick();
					break;
			}
		}



		protected virtual void AggressiveTick() {
			float dist = LogicUtils.Logic.ObjectDistance(attachedObject, WorldData.controllableOBJ);
			if (dist < 150) {
				attachedObject.SetPath(LogicUtils.PathLogic.PathFind(attachedObject.GetPointLocation(), WorldData.controllableOBJ.GetPointLocation()));
			} else {
				attachedObject.SetPath(new List<World.Regions.Node>());
			}
		}
		protected virtual void PassiveTick() {

		}
		protected virtual void NeutralTick() {

		}

		public enum AIType { Passive, Aggressive, Neutral};
	}
}
