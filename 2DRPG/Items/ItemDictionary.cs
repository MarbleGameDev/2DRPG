using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.Items.ItemClasses;

namespace _2DRPG.Items {
	static class ItemDictionary {

		private static Dictionary<string, Type> itemClasses = new Dictionary<string, Type>();

		static ItemDictionary(){
			RegisterItem("basicChest", typeof(BasicChestplate));
			RegisterItem("basicFood", typeof(FoodBase));
		}

		public static void RegisterItem(string name, Item i) {
			itemClasses.Add(name, i.GetType());
		}
		public static void RegisterItem(string name, Type t) {
			itemClasses.Add(name, t);
		}

		/// <summary>
		/// Attempts to create a new instance of the Item given by the string and construct it with the arguments passed, returning null otherwise
		/// </summary>
		/// <param name="itemName"></param>
		/// <param name="constructorArgs"></param>
		/// <returns></returns>
		public static Item GetItem(string itemName, object[] constructorArgs) {
			if (itemClasses.ContainsKey(itemName)) {
				try {
					return (Item)Activator.CreateInstance(itemClasses[itemName], constructorArgs);
				} catch (ArgumentException) {
					return null;
				} catch (MissingMethodException) {
					return null;
				}
			}
			return null;
		}
	}
}
