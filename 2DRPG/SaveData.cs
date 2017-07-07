using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

namespace _2DRPG {
	public static class SaveData {

		private static string saveLocation = "SaveData/";
		private static JsonSerializer serializer = new JsonSerializer();

		public static Settings GameSettings = new Settings();
		public static Dictionary<string, RegionSave> RegionData = new Dictionary<string, RegionSave>();

		public static void LoadGame() {
			GameSettings = DeSerializeObject<Settings>("Settings");
		}
		public static void SaveGame() {
			SerializeObject(GameSettings, "Settings");
			lock (RegionData) {
				foreach (string r in RegionData.Keys) {
					SerializeObject(RegionData[r], r);
				}
			}
		}
		public static void LoadRegion(string s) {
			lock (RegionData) {
				if (!RegionData.ContainsKey(s))
					RegionData.Add(s, DeSerializeObject<RegionSave>(s));
			}
		}
		public static void UnloadRegion(string s) {
			lock (RegionData) {
				if (RegionData.ContainsKey(s))
					RegionData.Remove(s);
			}
		}
		public static void SaveRegion(string s) {
			lock (RegionData) {
				SerializeObject(RegionData[s], s);
			}
		}


		public static void SerializeObject(object obj, string saveName) {
			try {
				using (StreamWriter sw = new StreamWriter(saveLocation + saveName + ".rz"))
				using (JsonWriter writer = new JsonTextWriter(sw)) {
					writer.Formatting = Formatting.Indented;
					serializer.Serialize(writer, obj);
				}
			} catch (JsonSerializationException e) {
				System.Diagnostics.Debug.WriteLine("Serialization Error: " + e.Message);
			}
			
		}
		public static T DeSerializeObject<T>(string fileName) {
			try {
				using (StreamReader sw = new StreamReader(saveLocation + fileName + ".rz"))
				using (JsonReader reader = new JsonTextReader(sw)) {
					return serializer.Deserialize<T>(reader);
				}
			} catch (JsonSerializationException e) {
				System.Diagnostics.Debug.WriteLine("Deserialization Error: " + e.Message);
			} catch (FileNotFoundException e) {
				System.Diagnostics.Debug.WriteLine("Deserialization Error: " + e.Message);
			}
			return default(T);
		}

	}
}
