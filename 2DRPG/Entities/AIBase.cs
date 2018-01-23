using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.World.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using _2DRPG.World.Entities;

namespace _2DRPG.Entities {
	class AIBase {

		public AIType type;

		public bool active = true;

		private StandardMob attachedObject;

		public AIBase(StandardMob obj) {
			attachedObject = obj;
		}

		public void AITick() {
			if (!active)
				return;
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
			float dist = LogicUtils.Logic.ObjectDistance(attachedObject, WorldData.player);
			if (dist < 200) {
				attachedObject.SetPath(LogicUtils.PathLogic.PathFind(attachedObject.GetPointLocation(), WorldData.player.GetPointLocation()));
			} else {
				attachedObject.SetPath(new List<World.Regions.Node>());
			}
		}
		protected virtual void PassiveTick() {

		}
		protected virtual void NeutralTick() {
			float dist = LogicUtils.Logic.ObjectDistance(attachedObject, WorldData.player);
			if (dist > 450)
				return;
			Random rnd = new Random();
			double d = rnd.NextDouble();
			if (d < .05f) {
				int x = (int)((rnd.NextDouble()- .5d) * 48);
				int y = (int)((rnd.NextDouble() - .5d) * 48);
				attachedObject.SetPath(LogicUtils.PathLogic.PathFind(attachedObject.GetPointLocation(), new System.Drawing.Point(x + (int)attachedObject.worldX, y + (int)attachedObject.worldY)));
			}
		}

		public enum AIType { Passive, Aggressive, Neutral};
	}
}
