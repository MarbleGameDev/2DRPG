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
			commands.Add("help", help);
			commands.Add("getCoords", getCoords);
			commands.Add("pauseGame", pauseGame);
			commands.Add("quit", quitGame);
		}


		public static string ExecuteCommand(string command) {
			string[] args = command.Split(' ');
			System.Diagnostics.Debug.WriteLine(args[0]);
			if (commands.ContainsKey(args[0])) {
				return commands[args[0]].Invoke(args.Skip(1).ToArray());
			} else
				return "command not found";
		}

		private static string help(string[] args) {
			string output = "Commands: ";
			foreach (string s in commands.Keys)
				output += s + " ";
			return output;
		}
		private static string getCoords(string[] args) {
			return "Coords: " + WorldData.CurrentX + ", " + WorldData.CurrentY;
		}
		private static string pauseGame(string[] args) {
			if (GameState.CurrentState == GameState.GameStates.Game)
				GameState.SetGameState(GameState.GameStates.Paused);
			if (GameState.CurrentState == GameState.GameStates.Paused)
				GameState.SetGameState(GameState.GameStates.Game);
			return "Game Paused, execute again to resume";
		}
		private static string quitGame(string[] args) {
			Application.Exit();
			return "";
		}
	}
}
