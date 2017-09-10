using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.World.Objects {
	/// <summary>
	/// Interface allowing the player to interact with an object
	/// </summary>
	public interface IInteractable {
		void Interact();
		string GetName();
	}
}
