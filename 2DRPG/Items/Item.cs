using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.Save;

namespace _2DRPG.Items {
	[Serializable]
	abstract class Item {

		public bool Stackable { get; private set; }

		public int Quantity { get; set; }

		public virtual string Name { get; set; } = "Unnamed Item";

		public Item(int quantity, bool stackable) {
			Quantity = quantity;
			Stackable = stackable;
		}

		/// <summary>
		/// Adds two stackable Items of the same type, throwing exceptions otherwise
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <exception cref="InvalidOperationException"></exception>
		/// <returns></returns>
		public static Item operator +(Item a, Item b) {
			if (a.GetType() == b.GetType() && a.Stackable && b.Stackable)
				return a + b.Quantity;
			else
				throw new InvalidOperationException();
		}
		/// <summary>
		/// Adds i to the quantity of item a, throwing exceptions if a is not stackable
		/// </summary>
		/// <param name="a"></param>
		/// <param name="i"></param>
		/// <exception cref="InvalidOperationException"></exception>
		/// <returns></returns>
		public static Item operator +(Item a, int i) {
			if (!a.Stackable)
				throw new InvalidOperationException();
			a.Quantity += i;
			if (a.Quantity <= 0)
				return null;
			return a;
		}
		/// <summary>
		/// Subtracts i from the quantity of item a, throwing exceptions if a is not stackable
		/// </summary>
		/// <param name="a"></param>
		/// <param name="i"></param>
		/// <exception cref="InvalidOperationException"></exception>
		/// <returns></returns>
		public static Item operator -(Item a, int i) {
			if (a == null)
				return a;
			if (!a.Stackable)
				throw new InvalidOperationException();
			a.Quantity -= i;
			if (a.Quantity <= 0)
				a = null;
			return a;
		}

		public override string ToString() {
			return Name;
		}

	}
}
