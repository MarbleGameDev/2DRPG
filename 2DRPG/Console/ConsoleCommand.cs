using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.Console {
	abstract class ConsoleCommand {
		public abstract string Invoke(string[] args);
		public virtual string Help() {
			return "Usage: " + Name;
		}
		public abstract string Name { get; }
	}
}
