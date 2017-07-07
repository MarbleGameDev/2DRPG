using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2DRPG {
	public static class Console {
		private delegate string ConsoleCommand(string[] args);

		private static Dictionary<string, ConsoleCommand> commands = new Dictionary<string, ConsoleCommand>();

		static Console() {
			commands.Add("openBuilder", Builder);
			commands.Add("help", Help);
			commands.Add("getCoords", GetCoords);
			commands.Add("pauseGame", PauseGame);
			commands.Add("quit", QuitGame);
			commands.Add("saveGame", SaveGame);
		}


		public static string ExecuteCommand(string command) {
			string[] args = command.Split(' ');
			if (commands.ContainsKey(args[0])) {
				return commands[args[0]].Invoke(args.Skip(1).ToArray());
			} else
				return "command not found";
		}

		private static string Help(string[] args) {
			string output = "Commands: ";
			foreach (string s in commands.Keys)
				output += s + " ";
			return output;
		}
		private static string GetCoords(string[] args) {
			return "Coords: " + WorldData.CurrentX + ", " + WorldData.CurrentY;
		}
		private static string PauseGame(string[] args) {
			if (GameState.CurrentState == GameState.GameStates.Game)
				GameState.SetGameState(GameState.GameStates.Paused);
			if (GameState.CurrentState == GameState.GameStates.Paused)
				GameState.SetGameState(GameState.GameStates.Game);
			return "Game Paused, execute again to resume";
		}
		private static string QuitGame(string[] args) {
			Application.Exit();
			return "";
		}
		private static string SaveGame(string[] args) {
			SaveData.SaveGame();
			return "Game Saved";
		}
		private static string Builder(string[] args) {
			Screen.OpenWindow("worldBuilder");
			return "";
		}
	}
}
