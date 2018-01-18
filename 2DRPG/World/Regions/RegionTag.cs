using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.World.Regions {
	[Serializable]
	class RegionTag : IEquatable<RegionTag>{
		public readonly int RegionX;
		public readonly int RegionY;

		public RegionTag(int rx, int ry) {
			RegionX = rx;
			RegionY = ry;
		}
		public RegionTag() { }

		public override bool Equals(object obj) {
			if (obj is RegionTag t) {
				return Equals(t);
			}
			return false;
		}

		public bool Equals(RegionTag t) {
			return t.RegionX == RegionX && t.RegionY == RegionY;
		}

		public static bool operator == (RegionTag a, RegionTag b) {
			return a.Equals(b);
		}

		public static bool operator != (RegionTag a, RegionTag b) {
			return !a.Equals(b);
		}

		public override int GetHashCode() {
			//Verified hashing algorithm
			int hash = 23;
			hash += RegionX;
			hash = (hash * 89) + RegionY;
			return hash;
		}

		public override string ToString() {
			return RegionX + "x" + RegionY;
		}
	}
}
