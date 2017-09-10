using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.World.Objects;
using System.Drawing;

namespace _2DRPG.World.Regions {
	class RegionBase {

		public int RegionX { get; private set; }
		public int RegionY { get; private set; }
		public string RegionTag { get; private set; }
		public List<string> TextureNames = new List<string>();
		protected HashSet<WorldObjectBase> regionObjects = new HashSet<WorldObjectBase>();
		public Dictionary<int, HashSet<int>> CollisionPoints = new Dictionary<int, HashSet<int>>();

		public RegionBase(string regionTag, GameSave save) {
			RegionTag = regionTag;
			string[] coords = regionTag.Split('x');
			RegionX = int.Parse(coords[0]);
			RegionY = int.Parse(coords[1]);
			foreach (GameSave.WorldObjectStorage s in save.worldObjects) {
				WorldObjectBase b = GameSave.ConstructWorldObject(s);
				regionObjects.Add(b);
				if (!TextureNames.Contains(b.texName))
					TextureNames.Add(b.texName);
			}
			CollisionPoints = save.CollisionPoints;
		}

		public virtual HashSet<WorldObjectBase> GetWorldObjects() {
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

		public void CompileCollisionHash() {
			List<WorldObjectBase> regobs = new List<WorldObjectBase>();
			foreach (WorldObjectBase b in regionObjects)
				if (b is ICollidable) {
					regobs.Add(b);
				}

			CollisionPoints.Clear();
			for (int x = 0; x < 1000; x+= 4) {
				for (int y = 0; y < 1000; y+= 4) {
					int tempX = RegionX * 1000 + x;
					int tempY = RegionY * 1000 + y;
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
