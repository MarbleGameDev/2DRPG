using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.World.Objects;

namespace _2DRPG.World.Regions {
	class Region_1x0 : RegionBase {
		public override int RegionX { get { return -1; } }
		public override int RegionY { get { return 0; } }
		public override string RegionTag { get { return "-1x0"; } }
		public override string[] TextureNames { get { return new string[] {
			"default", "josh"
		};} }

		public new HashSet<WorldObjectBase> regionObjects = new HashSet<WorldObjectBase>() {
			new WorldObjectBase(900, 0, "default"),
			new WorldObjectBase(900, 16f, "default")
		};
	}
}
