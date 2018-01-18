using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.World.Objects;
using System.Drawing;
using System.Runtime.Serialization;

namespace _2DRPG.World.Regions {
	[Serializable]
	class RegionBase {

		public RegionTag Tag { get; private set; }
		public List<Texture> TextureNames = new List<Texture>();
		protected HashSet<WorldObjectBase> regionObjects = new HashSet<WorldObjectBase>();
		public Dictionary<int, HashSet<int>> CollisionPoints = new Dictionary<int, HashSet<int>>();

		public RegionBase() {

		}
		public RegionBase(RegionTag reg) {
			Tag = reg;
		}

		public HashSet<WorldObjectBase> GetWorldObjects() {
			return regionObjects;
		}

		/// <summary>
		/// Called when the region is loaded into active world space
		/// </summary>
		public void Load() {

			TextureManager.RegisterTextures(TextureNames.ToArray());
		}

		/// <summary>
		/// Called when the region is unloaded from active world space
		/// </summary>
		public void Unload() {

			TextureManager.UnRegisterTextures(TextureNames.ToArray());
		}

		[OnSerializing]
		private void FindTextures(StreamingContext sc) {
			TextureNames.Clear();
			foreach (WorldObjectBase b in regionObjects)
				TextureNames.Add(b.texName);
			CompileCollisionHash();
		}

		public void CompileCollisionHash() {
			List<WorldObjectBase> regobs = new List<WorldObjectBase>();
			foreach (WorldObjectBase b in regionObjects)
				if (b is ICollidable) {
					regobs.Add(b);
				}

			CollisionPoints.Clear();
			for (int x = 0; x < 1000; x+= 4) {
				for (int y = 0; y < 1000; y+= 4) {
					int tempX = Tag.RegionX * 1000 + x;
					int tempY = Tag.RegionY * 1000 + y;
					bool open = true;
					foreach (WorldObjectBase b in regobs) {
						open = open && !LogicUtils.Logic.CheckIntersection(LogicUtils.PathLogic.ExpandPosition(b.quadPosition, 8), tempX, tempY);
					}
					if (!open) {
						if (CollisionPoints.ContainsKey(tempX))
							CollisionPoints[tempX].Add(tempY);
						else
							CollisionPoints.Add(tempX, new HashSet<int>() { tempY });
					}
				}
			}
		}
	}
}
