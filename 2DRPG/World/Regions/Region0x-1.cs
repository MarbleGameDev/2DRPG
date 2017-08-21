using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.World.Regions {
	class Region0x_1 : RegionBase {
		public override int RegionX { get { return 0; } }
		public override int RegionY { get { return -1; } }
		public override string RegionTag { get { return "0x-1"; } }
		public override string[] TextureNames { get { return new string[] {
			"default"
		};} }
	}
}
