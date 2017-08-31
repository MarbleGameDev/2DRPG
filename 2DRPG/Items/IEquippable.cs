using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.Items {
	interface IEquippable {

		/// <summary>
		/// Returns the valid player inventory slot for which this item is equippable
		/// </summary>
		int ValidSlot { get; }
	}
}
