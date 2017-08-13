using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI.Interaction {
	class InteractionPath : InteractionBase{

		public List<InteractionBase> items = new List<InteractionBase>();

		public string pathName;

		public InteractionPath() { }
		public InteractionPath(GameSave.InteractionObjectStorage store) {
			pathName = store.text;
			foreach (GameSave.InteractionObjectStorage s in store.subObjects)
				items.Add(GameSave.ConstructInteractionObject(s));
		}

		public override void Render() {
			throw new NotImplementedException();
		}

		public override void Setup() {
			
		}
		public override void Takedown() {

		}

		public override GameSave.InteractionObjectStorage StoreObject() {
			List<GameSave.InteractionObjectStorage> stores = new List<GameSave.InteractionObjectStorage>();
			foreach (InteractionBase b in items) {
				stores.Add(b.StoreObject());
			}
			GameSave.InteractionObjectStorage store = new GameSave.InteractionObjectStorage() {
				text = pathName, subObjects = stores, objectType = GameSave.InteractionObjectType.Path
			};
			return store;
		}

		public override string ToString() {
			return pathName;
		}
	}
}
