using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.World.Objects;
using System.Drawing;

namespace _2DRPG.World.Regions {
	abstract class RegionBase {

		public abstract int RegionX { get; }
		public abstract int RegionY { get; }
		public abstract string RegionTag { get; }
		public abstract string[] TextureNames { get; }
		protected HashSet<WorldObjectBase> regionObjects = new HashSet<WorldObjectBase>();
		public HashSet<Point> CollisionPoints = new HashSet<Point>();

		public virtual HashSet<WorldObjectBase> LoadObjects() {
			regionObjects.Clear();
			if (SaveData.RegionData[RegionTag] != null) {
				foreach (GameSave.WorldObjectStorage st in SaveData.RegionData[RegionTag].worldObjects)
					regionObjects.Add(GameSave.ConstructWorldObject(st));
			}
			return regionObjects;
		}
		public virtual HashSet<WorldObjectBase> GetWorldObjects() {
			return regionObjects;
		}

		public void LoadTextures() {
			SaveData.LoadRegion(RegionTag);
			if (SaveData.RegionData[RegionTag] != null)
				CollisionPoints = SaveData.RegionData[RegionTag].CollisionPoints;
			TextureManager.RegisterTextures(TextureNames);
		}
		public void UnloadTextures() {
			TextureManager.UnRegisterTextures(TextureNames);
			SaveData.UnloadRegion(RegionTag);
		}

		public void CompileCollisionHash() {
			for (int x = 0; x < 1000; x++) {
				for (int y = 0; y < 1000; y++) {
					int tempX = RegionX * 1000 + x;
					int tempY = RegionY * 1000 + y;
					bool open = true;
					foreach (WorldObjectBase b in regionObjects) {
						if (b is ICollidable)
							open = open & !LogicUtils.Logic.CheckIntersection(LogicUtils.PathLogic.ExpandPosition(b.quadPosition, 8), tempX, tempY);
					}
					if (!open)
						CollisionPoints.Add(new Point(tempX, tempY));
				}
			}
		}
	}
}
