using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.World.Objects;
using _2DRPG.Player;

namespace _2DRPG.World.Regions {
	/// <summary>
	/// Region for 0x0 to 99x99
	/// </summary>
	class Region0x0 : RegionBase {
		public override int RegionX { get { return 0; } }
		public override int RegionY { get { return 0; } }
		public override string RegionTag { get { return "0x0"; } }
		public override string[] TextureNames { get { return new string[] {
			"flower", "default", "heart", "josh", "character"
		};} }

		static WorldObjectInteractable inventory = new WorldObjectInteractable(180f, 58f);
		static WorldObjectAnimated flower = new WorldObjectAnimated(70f, 0f, 5, 4, 16, 16, 10, "flower") {
			size = 16f
		};

		new HashSet<WorldObjectBase> regionObjects = new HashSet<WorldObjectBase>() {
			inventory,
			new WorldObjectCollidable(-200f, -10f),
			flower
		};

	}
}
