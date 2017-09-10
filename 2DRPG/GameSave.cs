using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.World.Objects;
using _2DRPG.GUI.Interaction;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Drawing;

namespace _2DRPG {
	[Serializable]
	public class GameSave {
		public List<WorldObjectStorage> worldObjects = new List<WorldObjectStorage>();

		public Dictionary<int, HashSet<int>> CollisionPoints = new Dictionary<int, HashSet<int>>();

		/// <summary>
		/// Constructs a WorldObject based on the storage object passed
		/// </summary>
		/// <param name="store">WorldObjectStorage object containing all data needed for the object</param>
		/// <returns></returns>
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
				case WorldObjectType.Dialogue:
					return new WorldObjectDialogue(store);
				case WorldObjectType.Inventory:
					return new WorldObjectInventory(store);
				case WorldObjectType.SimpleItem:
					return new WorldObjectSimpleItem(store);
				case WorldObjectType.Movable:
					return new WorldObjectMovable(store);
				case WorldObjectType.MovableAnimated:
					return new WorldObjectMovableAnimated(store);
				case WorldObjectType.StandardMob:
					return new Entities.StandardMob(store);
				default:
					System.Diagnostics.Debug.WriteLine("Could not find type of object being constructed from save file, check GameSave");
					return null;


			}
		}
		public static InteractionBase ConstructInteractionObject(InteractionObjectStorage store) {
			switch (store.objectType) {
				case InteractionObjectType.Choice:
					return new InteractionChoice(store);
				case InteractionObjectType.Dialogue:
					return new InteractionDialogue(store);
				case InteractionObjectType.Quests:
					return new InteractionQuests(store);
				case InteractionObjectType.Path:
					return new InteractionPath(store);
				default:
					return null;
			}
		}

		public class WorldObjectStorage {
			[JsonConverter(typeof(StringEnumConverter))]
			public WorldObjectType objectType;
			public float worldX, worldY, width, height;
			public int layer;
			public string textureName;
			public object[] extraData;
		}

		public class InteractionObjectStorage {
			[JsonConverter(typeof(StringEnumConverter))]
			public InteractionObjectType objectType;
			public string text;
			public List<InteractionObjectStorage> subObjects;
			public object[] extraData;
		}

		public class ItemStorage {
			public Type type;
			public string Name;
			public int Quantity;
			public bool Stackable;
			public object[] extraData;
		}


		public enum WorldObjectType { Animated, Base, Collidable, Controllable, Dialogue, Inventory, SimpleItem, Movable, MovableAnimated, StandardMob};
		public enum InteractionObjectType { Choice, Dialogue, Quests, Path};

	}
}
