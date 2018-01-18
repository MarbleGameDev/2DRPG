using _2DRPG.Save;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.Console.Commands {
	class SaveGame : ConsoleCommand {
		public override string Name => "saveGame";

		public override string Invoke(string[] args) {
			SaveData.SaveGame();
			return "Game Saved";
		}
	}
	class SaveGameData : ConsoleCommand {
		public override string Name => "saveGameData";

		public override string Invoke(string[] args) {
			SaveData.SaveGameData();
			return "Game Data Saved";
		}
	}
	class SaveAll : ConsoleCommand {
		public override string Name => "saveAll";

		public override string Invoke(string[] args) {
			SaveData.SaveGameData();
			SaveData.SaveGame();
			return "All Data Saved";
		}
	}
}
