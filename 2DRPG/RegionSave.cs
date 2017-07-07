using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.World.Objects;

namespace _2DRPG {
	[Serializable]
	public class RegionSave {
		public List<WorldObjectStorage> worldObjects = new List<WorldObjectStorage>();

		public static WorldObjectBase ConstructWorldObject(WorldObjectStorage store) {
			switch (store.objectType) {
				case WorldObjectType.Base:
					return new WorldObjectBase(store);
				case WorldObjectType.Animated:
					return new WorldObjectAnimated(store);
				case WorldObjectType.Collidable:
					return new WorldObjectCollidable(store);
				case WorldObjectType.Controllable:
					return new WorldObjectControllable(store);
				case WorldObjectType.Interactable:
					return new WorldObjectInteractable(store);
				case WorldObjectType.Movable:
					return new WorldObjectMovable(store);
				case WorldObjectType.MovableAnimated:
					return new WorldObjectMovableAnimated(store);
				default:
					return null;


			}
		}

		public class WorldObjectStorage {
			public WorldObjectType objectType;
			public float worldX, worldY;
			public int layer;
			public string textureName;
			public object[] extraData;
		}


		public enum WorldObjectType { Animated, Base, Collidable, Controllable, Interactable, Movable, MovableAnimated};

	}
}
