using _2DRPG.Save;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.Console.Commands {
	class DevWin : ConsoleCommand {
		public override string Name => "devWin";

		public override string Invoke(string[] args) {
			if (!Form1.devWin.IsDisposed) {
				Form1.devWin.Show();
			} else {
				Form1.devWin = new DevWindow();
				Form1.devWin.Show();
			}
			Screen.CloseWindow("console");
			return "Dev Window Opened";
		}
	}
	class BuilderWindow : ConsoleCommand {
		public override string Name => "builder";

		public override string Invoke(string[] args) {
			if (SaveData.GameSettings.worldBuilder) {
				Screen.CloseWindow("console");
				Screen.OpenWindow("worldBuilder");
				return "";
			} else {
				return "World Builder not enabled";
			}
		}
	}
	class AddItem : ConsoleCommand {
		public override string Name => "addItem";

		public override string Help() {
			return "Usage: addItem 'item name' [quantity] [custom name]";
		}

		public override string Invoke(string[] args) {
			if (args.Length > 0) {
				Items.Item n = null;
				if (args.Length > 1) {
					List<object> obs = new List<object>();
					if (int.TryParse(args[1], out int quant)) {
						obs.Add(quant);
					} else {
						return "Invalid item quantity. " + Help();
					}
					if (args.Length > 2) {
						obs.Add(args[2]);
					}
					n = Items.ItemDictionary.GetItem(args[0], obs.ToArray());
				} else {
					n = Items.ItemDictionary.GetItem(args[0], null);
				}
				if (n == null)
					return "Invalid item name/arguments. " + Help();
				WorldData.player.Data.inventory.AddItem(n);
				return "Item added to inventory";
			}
			return Help();
		}
	}
}
