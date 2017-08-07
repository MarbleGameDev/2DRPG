using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Reflection;

namespace _2DRPG {
	public static class SaveData {

		//TODO: add folders for multiple saves that are copied from the embeded master
#if DEBUG
		private static string saveLocation = "../../SaveData/";
#else
		private static string saveLocation = "SaveData/";
#endif

		private static JsonSerializer serializer = new JsonSerializer();

		public static Settings GameSettings = new Settings();
		public static Dictionary<string, GameSave> RegionData = new Dictionary<string, GameSave>();

		static SaveData() {
			Directory.CreateDirectory(saveLocation);
			Directory.CreateDirectory(saveLocation + "Quests/");
		}
		/// <summary>
		/// Loads the game settings, but not world data
		/// </summary>
		public static void LoadGame() {
			GameSettings = DeSerializeObject<Settings>("Settings");
			if (GameSettings == null)
				GameSettings = new Settings();

			LoadTasks();
		}
		/// <summary>
		/// Saves all game data, including settings and world data
		/// </summary>
		public static void SaveGame() {
			SerializeObject(GameSettings, "Settings");
			lock (RegionData) {
				lock (WorldData.currentRegions) {
					foreach (string r in WorldData.regionFiles.Keys) {
						GameSave s = new GameSave();
						foreach (World.Objects.WorldObjectBase b in WorldData.regionFiles[r].GetWorldObjects()) {
							s.worldObjects.Add(b.StoreObject());
						}
						SerializeObject(s, r);
					}
				}
			}
			SaveTasks();
			GUI.Windows.NotificationWindow.NewNotification("Game Saved", 190);
		}

		private static void LoadTasks() {
			foreach (string path in Directory.EnumerateFiles(saveLocation + "Quests/")) {
				Quests.TaskBase q = DeSerializeObject<Quests.TaskBase>(path.TrimEnd(".rz".ToCharArray()).Replace(saveLocation, ""));
				Quests.QuestData.QuestDatabase.Add(q.taskName, q);
			}
			if (!Form1.devWin.IsDisposed)
				DevWindow.Quest.UpdateQuests();
			Quests.QuestData.ActiveQuests = DeSerializeObject<HashSet<string>>("PlayerQuests");
		}

		private static void SaveTasks() {
			foreach (KeyValuePair<string, Quests.IQuest> q in Quests.QuestData.QuestDatabase) {
				SerializeObject(q.Value, "Quests/" + q.Key);
			}
			SerializeObject(Quests.QuestData.ActiveQuests, "PlayerQuests");
		}


		public static void LoadRegion(string s) {
			lock (RegionData) {
				if (!RegionData.ContainsKey(s))
					RegionData.Add(s, DeSerializeObject<GameSave>(s));
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
