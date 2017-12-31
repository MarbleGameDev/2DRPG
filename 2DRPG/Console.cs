using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _2DRPG.Net;
using _2DRPG.Save;

namespace _2DRPG {
	public static class Console {
		private delegate string ConsoleCommand(string[] args);

		private static Dictionary<string, ConsoleCommand> commands = new Dictionary<string, ConsoleCommand>();

		static Console() {
			commands.Add("addItem", AddItem);
			commands.Add("devWindow", DevWindow);
			commands.Add("echo", Echo);
			commands.Add("getCoords", GetCoords);
			commands.Add("help", Help);
			commands.Add("openBuilder", Builder);
			commands.Add("pauseGame", PauseGame);
			commands.Add("quit", QuitGame);
			commands.Add("saveGame", SaveGame);
			commands.Add("saveGameData", SaveGameData);
			commands.Add("saveAll", SaveAll);
			commands.Add("setValue", SetValue);
			commands.Add("connection", Connection);
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
		private static string SaveGameData(string[] args) {
			SaveData.SaveGameData();
			return "Game Data Saved";
		}
		private static string SaveAll(string[] args) {
			SaveData.SaveGameData();
			SaveData.SaveGame();
			return "All Data Saved";
		}
		private static string Builder(string[] args) {
			if (SaveData.GameSettings.worldBuilder) {
				Screen.CloseWindow("console");
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
					case "devWindow":
						return (bool.TryParse(args[1], out SaveData.GameSettings.devWindow)) ? "Set Value" : "Invalid Value";
					case "worldBuilder":
						return (bool.TryParse(args[1], out SaveData.GameSettings.worldBuilder)) ? "Set Value" : "Invalid Value";
					default:
						return "Invalid Setting. Options: debug, fullScreen, devWindow, worldBuilder";
				}
			} else {
				return "Invalid Arguments";
			}
		}
		private static string DevWindow(string[] args) {
			if (!Form1.devWin.IsDisposed) {
				Form1.devWin.Show();
			} else {
				Form1.devWin = new DevWindow();
				Form1.devWin.Show();
			}
			Screen.CloseWindow("console");
			return "Dev Window Opened";
		}
		private static string AddItem(string[] args) {
			if (args.Length > 0) {
				Items.Item n = null;
				if (args.Length > 1) {
					List<object> obs = new List<object>();
					if (int.TryParse(args[1], out int quant)) {
						obs.Add(quant);
					} else {
						return "Invalid item quantity";
					}
					if (args.Length > 2) {
						obs.Add(args[2]);
					}
					n = Items.ItemDictionary.GetItem(args[0], obs.ToArray());
				} else {
					n = Items.ItemDictionary.GetItem(args[0], null);
				}
				if (n == null)
					return "Invalid item name / arguments";
				WorldData.player.Data.inventory.AddItem(n);
				return "Item added to inventory";
			}
			return "Invalid Arguments. Usage: addItem 'item name' [quantity] [custom name]";
		}
		private static string Connection(string[] args) {
			if (args.Length > 0) {
				switch (args[0]) {
					case "join":
						if (args.Length > 1)
							if (System.Net.IPAddress.TryParse(args[1], out System.Net.IPAddress ad))
								SessionManager.JoinPartner(new System.Net.IPEndPoint(ad, UDPMessager.port));
							else
								return "Invalid ip address";
						return "New Session Attempted with " + args[1];
					case "halt":
						SessionManager.EndPartner();
						return "Halted Session";
					case "enable":
						SaveData.GameSettings.coOp = true;
						SessionManager.StartRetreival();
						return "CoOp enabled";
					case "host":
						SessionManager.isHost = true;
						if (SaveData.GameSettings.coOp)
							return "Game now Hosting on " + UDPMessager.GetLocal().Address.ToString();
						SaveData.GameSettings.coOp = true;
						SessionManager.StartRetreival();
						return "CoOp enabled, Game now Hosting on " + UDPMessager.GetLocal().Address.ToString();
					case "msg":
						if (args.Length > 1)
							if (SessionManager.SendFrame(new UDPFrame() { subject = UDPFrame.FrameType.Connection, intData = new int[] { 2 }, stringData = new string[] { args[1]} }) == 1)
								return "Message sent to partner";
						return "Message failed to send";
					case "list":
						return SessionManager.ConnectionInfo();
					default:
						return "Usage: connection {join,halt,enable,host,msg, list} [ip address]";
				}
			}
			return "";
		}
	}
}
