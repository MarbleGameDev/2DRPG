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
			commands.Add("devWindow", DevWindow);
			commands.Add("echo", Echo);
			commands.Add("getCoords", GetCoords);
			commands.Add("help", Help);
			commands.Add("openBuilder", Builder);
			commands.Add("pauseGame", PauseGame);
			commands.Add("quit", QuitGame);
			commands.Add("saveGame", SaveGame);
			commands.Add("setValue", SetValue);
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
			if (SaveData.GameSettings.worldBuilder) {
				Screen.OpenWindow("worldBuilder");
				return "";
			} else {
				return "World Builder not enabled";
			}
		}
		private static string Echo(string[] args) {
			string tmp = "";
			foreach (string s in args) {
				tmp += s + " ";
			}
			return tmp;
		}
		private static string SetValue(string[] args) {
			if (args.Length > 1) {
				switch (args[0]) {
					case "debug":
						return (bool.TryParse(args[1], out SaveData.GameSettings.debugEnabled)) ? "Set Value" : "Invalid Value";
					case "fullScreen":
						return (bool.TryParse(args[1], out SaveData.GameSettings.fullScreen)) ? "Set Value" : "Invalid Value";
					case "interactionEditor":
						return (bool.TryParse(args[1], out SaveData.GameSettings.interactionEditor)) ? "Set Value" : "Invalid Value";
					case "worldBuilder":
						return (bool.TryParse(args[1], out SaveData.GameSettings.worldBuilder)) ? "Set Value" : "Invalid Value";
					default:
						return "Invalid Setting. Options: debug, fullScreen, interactionEditor, worldBuilder";
				}
			} else {
				return "Invalid Arguments";
			}
		}
		private static string DevWindow(string[] args) {
			Form1.devWin.Show();
			return "Dev Window Opened";
		}
	}
}
