using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.Save;

namespace _2DRPG.GUI.Interaction {
	class InteractionPath : InteractionBase{

		public List<InteractionBase> items = new List<InteractionBase>();

		public string pathName;

		public InteractionPath() { }
		public InteractionPath(RegionSave.InteractionObjectStorage store) {
			pathName = store.text;
			foreach (RegionSave.InteractionObjectStorage s in store.subObjects)
				items.Add(RegionSave.ConstructInteractionObject(s));
		}

		public override void Render() {
			throw new NotImplementedException();
		}

		public override void Setup() {
			
		}
		public override void Takedown() {

		}

		public override RegionSave.InteractionObjectStorage StoreObject() {
			List<RegionSave.InteractionObjectStorage> stores = new List<RegionSave.InteractionObjectStorage>();
			foreach (InteractionBase b in items) {
				stores.Add(b.StoreObject());
			}
			RegionSave.InteractionObjectStorage store = new RegionSave.InteractionObjectStorage() {
				text = pathName, subObjects = stores, objectType = RegionSave.InteractionObjectType.Path
			};
			return store;
		}

		public override string ToString() {
			return pathName;
		}
	}
}
