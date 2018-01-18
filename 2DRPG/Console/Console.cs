using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _2DRPG.Net;
using _2DRPG.Save;
using System.Reflection;

namespace _2DRPG.Console {
	public static class Console {

		private static Dictionary<string, ConsoleCommand> commands = new Dictionary<string, ConsoleCommand>();
		private static HashSet<string> commandNames = new HashSet<string>();

		public static void ConsoleSetup() {
			foreach (Type p in AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => p.GetTypeInfo().IsSubclassOf(typeof(ConsoleCommand)) && !p.IsAbstract)) {
				ConsoleCommand c = (ConsoleCommand)Activator.CreateInstance(p);
				commands.Add(c.Name.ToUpper(), c);
				commandNames.Add(c.Name);
			}
		}


		public static string ExecuteCommand(string command) {
			string[] args = command.Split(' ');
			if (commands.ContainsKey(args[0].ToUpper())) {
				return commands[args[0].ToUpper()].Invoke(args.Skip(1).ToArray());
			} else
				return "command not found";
		}

		class HelpCommand : ConsoleCommand {
			public override string Name => "help";

			public override string Help() {
				return "Usage: Help [command Name]";
			}
			public override string Invoke(string[] args) {
				if (args.Length == 0) {
					StringBuilder output = new StringBuilder("Commands: ");
					foreach (string s in commandNames.OrderBy(s => s))
						output.Append(s + " ");
					output.Remove(output.Length - 1, 1);
					output.Append(".\nType Help [command Name] for specifics");
					return output.ToString();
				} else {
					if (commands.ContainsKey(args[0].ToUpper()))
						return commands[args[0].ToUpper()].Help();
					return Help();
				}
			}
		}
		class EchoCommand : ConsoleCommand {
			public override string Name => "echo";

			public override string Invoke(string[] args) {
				string tmp = "";
				foreach (string s in args) {
					tmp += s + " ";
				}
				return tmp;
			}
		}

		[Obsolete]
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
