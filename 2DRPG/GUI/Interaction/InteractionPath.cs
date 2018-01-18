using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.Save;

namespace _2DRPG.GUI.Interaction {
	[Serializable]
	class InteractionPath : InteractionBase{

		public List<InteractionBase> items = new List<InteractionBase>();

		public string pathName;

		public InteractionPath() {

		}

		public override void Render() {
			throw new NotImplementedException();	//What the heck is this here for?
		}

		public override void Setup() {
			
		}
		public override void Takedown() {

		}

		public override string ToString() {
			return pathName;
		}
	}
}
