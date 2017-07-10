using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.World.Objects;

namespace _2DRPG.World.Regions {
	abstract class RegionBase {

		public abstract int RegionX { get; }
		public abstract int RegionY { get; }
		public abstract string RegionTag { get; }
		public abstract string[] TextureNames { get; }
		protected HashSet<WorldObjectBase> regionObjects = new HashSet<WorldObjectBase>();

		public virtual HashSet<WorldObjectBase> LoadObjects() {
			if (SaveData.RegionData[RegionTag] != null) {
				regionObjects.Clear();
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
			TextureManager.RegisterTextures(TextureNames);
		}
		public void UnloadTextures() {
			TextureManager.UnRegisterTextures(TextureNames);
			SaveData.UnloadRegion(RegionTag);
		}
	}
}
