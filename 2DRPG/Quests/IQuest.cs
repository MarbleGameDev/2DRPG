using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.Quests {
	public interface IQuest {
		int CheckStatus();
		string ToString();
		bool Completed { get; set; }
	}
}
