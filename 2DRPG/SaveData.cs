using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Reflection;
using Ionic.Zip;

namespace _2DRPG {
	public static class SaveData {

		//TODO: add folders for multiple saves that are copied from the embeded master
#if DEBUG
		private static string saveLocation = "../../SaveData/";
#else
		private static string saveLocation = "SaveData/";
#endif
		private static string gameDataLocation = "../../GameData/";
		private static JsonSerializer serializer = new JsonSerializer();

		public static Settings GameSettings = new Settings();
		public static Dictionary<string, GameSave> RegionData = new Dictionary<string, GameSave>();

		static SaveData() {
			Directory.CreateDirectory(saveLocation);
		}
		/// <summary>
		/// Loads the player settings, creating new copies if there doesn't exist others
		/// </summary>
		public static void LoadGame() {
			GameSettings = DeSerializeObject<Settings>("Settings");
			if (GameSettings == null)
				GameSettings = new Settings();
			Quests.QuestData.ActiveQuests = DeSerializeObject<HashSet<string>>("PlayerQuests");
			if (Quests.QuestData.ActiveQuests == null)
				Quests.QuestData.ActiveQuests = new HashSet<string>();
		}
		/// <summary>
		/// Saves the player settings
		/// </summary>
		public static void SaveGame() {
			SerializeObject(GameSettings, "Settings");
			SerializeObject(Quests.QuestData.ActiveQuests, "PlayerQuests");

			GUI.Windows.NotificationWindow.NewNotification("Game Saved", 190);
		}

		/// <summary>
		/// Saves the game data to the packages
		/// </summary>
		public static void SaveGameData() {
			lock (RegionData) {
				lock (WorldData.currentRegions) {
					foreach (string r in WorldData.regionFiles.Keys) {
						GameSave s = new GameSave();
						foreach (World.Objects.WorldObjectBase b in WorldData.regionFiles[r].GetWorldObjects()) {
							s.worldObjects.Add(b.StoreObject());
						}
						SerializeObjectToZip(s, r, "Package_01");
					}
				}
			}
			SaveTasks();
			GUI.Windows.NotificationWindow.NewNotification("Data Saved", 60);
		}
		/// <summary>
		/// Loads the game data from packages
		/// </summary>
		public static void LoadGameData() {
			LoadTasks();
		}

		private static void LoadTasks() {
			using (ZipFile zip = ZipFile.Read(gameDataLocation + "Package_02.rzz")) {
				foreach(ZipEntry e in zip)
					using (StreamReader sw = new StreamReader(e.OpenReader()))
					using (JsonReader reader = new JsonTextReader(sw)) {
						Quests.TaskBase q = serializer.Deserialize<Quests.TaskBase>(reader);
						Quests.QuestData.QuestDatabase.Add(q.taskName, q);
					}
			}
			if (!Form1.devWin.IsDisposed)
				DevWindow.Quest.UpdateQuests();
		}

		private static void SaveTasks() {
			foreach (KeyValuePair<string, Quests.IQuest> q in Quests.QuestData.QuestDatabase) {
				SerializeObjectToZip(q.Value, q.Key, "Package_02");
			}
		}


		public static void LoadRegion(string s) {
			lock (RegionData) {
				if (!RegionData.ContainsKey(s))
					RegionData.Add(s, DeSerializeObjectFromZip<GameSave>(s, "Package_01"));
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
				SerializeObjectToZip(RegionData[s], s, "Package_01");
			}
		}


		public static void SerializeObject(object obj, string fileName) {
				try {
					using (StreamWriter sw = new StreamWriter(saveLocation + fileName + ".rz"))
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

		//Package_01: Region Data
		//Package_02: Tasks Data

		public static void SerializeObjectToZip(object obj, string fileName, string zipName) {
			using (ZipFile zip = ZipFile.Read(gameDataLocation + zipName + ".rzz"))
			using (Stream st = new MemoryStream())
			using (StreamWriter sw = new StreamWriter(st))
			using (JsonWriter writer = new JsonTextWriter(sw)) {
				writer.Formatting = Formatting.Indented;
				serializer.Serialize(writer, obj);
				writer.Flush();
				st.Position = 0;
				if (zip.ContainsEntry(fileName + ".rz"))
					zip.UpdateEntry(fileName + ".rz", st);
				else
					zip.AddEntry(fileName + ".rz", st);
				zip.Save();
			}
		}
		public static T DeSerializeObjectFromZip<T>(string fileName, string zipName) {
			using (ZipFile zip = ZipFile.Read(gameDataLocation + zipName + ".rzz")) {
				if (!zip.ContainsEntry(fileName + ".rz"))
					throw new IOException("File does not exist");
				using (StreamReader sw = new StreamReader(zip[fileName + ".rz"].OpenReader()))
				using (JsonReader reader = new JsonTextReader(sw)) {
					return serializer.Deserialize<T>(reader);
				}
			}
		}

	}
}
