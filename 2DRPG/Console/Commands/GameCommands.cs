using _2DRPG.Save;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2DRPG.Console.Commands {
	class GetCoords : ConsoleCommand {
		public override string Name => "getCoords";

		public override string Invoke(string[] args) {
			return "Coords: " + WorldData.CurrentX + ", " + WorldData.CurrentY;
		}
	}
	class PauseGame : ConsoleCommand {
		public override string Name => "pauseGame";

		public override string Invoke(string[] args) {
			if (GameState.CurrentState == GameState.GameStates.Game)
				GameState.SetGameState(GameState.GameStates.Paused);
			if (GameState.CurrentState == GameState.GameStates.Paused)
				GameState.SetGameState(GameState.GameStates.Game);
			return "Game paused, execute again to resume";
		}
	}
	class QuitGame : ConsoleCommand {
		public override string Name => "quit";

		public override string Invoke(string[] args) {
			Application.Exit();
			return "";
		}
	}
	class Teleport : ConsoleCommand {
		public override string Name => "teleport";

		public override string Help() {
			return "Usage: teleport 'x' 'y'";
		}

		public override string Invoke(string[] args) {
			if (args.Length < 2)
				return Help();
			if (int.TryParse(args[0], out int x) && int.TryParse(args[1], out int y)) {
				WorldData.player.Teleport(x, y);
				return "Teleported to: " + x + ", " + y;
			}
			return Help();
		}
	}
	class TeleP : Teleport {
		public override string Name => "tp";

		public override string Help() {
			return "Usage: tp 'x 'y'";
		}
	}
	class SetValue : ConsoleCommand {
		public override string Name => "setValue";

		public override string Help() {
			return "Usage: setValue 'debug/fullscreen/devWindow/worldBuilder' 'true/false'";
		}

		public override string Invoke(string[] args) {
			if (args.Length > 1) {
				switch (args[0]) {
					case "debug":
						return (bool.TryParse(args[1], out SaveData.GameSettings.debugEnabled)) ? "Set Value" : Help();
					case "fullScreen":
						return (bool.TryParse(args[1], out SaveData.GameSettings.fullScreen)) ? "Set Value" : Help();
					case "devWindow":
						return (bool.TryParse(args[1], out SaveData.GameSettings.devWindow)) ? "Set Value" : Help();
					case "worldBuilder":
						return (bool.TryParse(args[1], out SaveData.GameSettings.worldBuilder)) ? "Set Value" : Help();
					default:
						return Help();
				}
			} else {
				return Help();
			}
		}
	}
}
