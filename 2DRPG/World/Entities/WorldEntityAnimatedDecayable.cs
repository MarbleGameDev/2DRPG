using _2DRPG.World.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.World.Entities {
	class WorldEntityAnimatedDecayable : WorldEntityAnimated, IDelayable {


		
		public WorldEntityAnimatedDecayable(float x, float y, int layer, int spritesAmt, int spriteWth, int spriteHt, int frameInt, Texture textureName, int tickNum, float width = 16, float height = 16) : base(x, y, layer, spritesAmt, spriteWth, spriteHt, frameInt, textureName, width, height) {
			TickNum = tickNum;
			DelayAction = DeleteObject;
		}

		public int TickNum { get; set; }
		public Action DelayAction { get; set; }

		private int tickCounter = 0;

		public override void AnimationTick() {
			base.AnimationTick();
			if (++tickCounter == TickNum) {
				tickCounter = 0;
				DelayAction.Invoke();
			}
		}

		private void DeleteObject() {
			WorldData.RemoveObject(this);
		}
	}
}
