using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Reflection;
using Ionic.Zip;
using System.Windows.Forms;

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

		static SaveData() {
			Directory.CreateDirectory(saveLocation);
			serializer.ObjectCreationHandling = ObjectCreationHandling.Replace;
		}
		/// <summary>
		/// Loads the player settings, creating new copies if there doesn't exist others
		/// </summary>
		public static void LoadGame() {
			GameSettings = DeSerializeObject<Settings>("Settings");
			if (GameSettings == null)
				GameSettings = new Settings();
			Player.MCObject.Data = DeSerializeObject<Player.PlayerData>("Player");
			if (Player.MCObject.Data == null)
				Player.MCObject.Data = new Player.PlayerData();
			Quests.QuestData.ActiveQuests = DeSerializeObject<HashSet<string>>("PlayerQuests");
			if (Quests.QuestData.ActiveQuests == null)
				Quests.QuestData.ActiveQuests = new HashSet<string>();
		}
		/// <summary>
		/// Saves the player settings
		/// </summary>
		public static void SaveGame() {
			SerializeObject(GameSettings, "Settings");
			SerializeObject(Player.MCObject.Data, "Player");
			SerializeObject(Quests.QuestData.ActiveQuests, "PlayerQuests");

			GUI.Windows.NotificationWindow.NewNotification("Game Saved", 190);
		}

		/// <summary>
		/// Checks that the Game Data directory is intact, will probably add zip verification later
		/// </summary>
		/// <returns></returns>
		public static bool CheckSaveState() {
			if (!Directory.Exists(gameDataLocation)) {
				return false;
			}
			return true;
		}

		/// <summary>
		/// Saves the open game data to the packages
		/// </summary>
		public static void SaveGameData() {
			lock (WorldData.currentRegions) {
				foreach (string r in WorldData.currentRegions.Keys) {
					GameSave s = new GameSave();
					foreach (World.Objects.WorldObjectBase b in WorldData.currentRegions[r].GetWorldObjects()) {
						s.worldObjects.Add(b.StoreObject());
					}
					WorldData.currentRegions[r].CompileCollisionHash();
					s.CollisionPoints = WorldData.currentRegions[r].CollisionPoints;
					SerializeObjectToZip(s, r, "Package_01");
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


		public static GameSave LoadRegion(string regionTag) {

			return DeSerializeObjectFromZip<GameSave>(regionTag, "Package_01");
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

		private static void SerializeObjectToZip(object obj, string fileName, string zipName) {
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
		private static T DeSerializeObjectFromZip<T>(string fileName, string zipName) {
			using (ZipFile zip = ZipFile.Read(gameDataLocation + zipName + ".rzz")) {
				if (!zip.ContainsEntry(fileName + ".rz")) {
					System.Diagnostics.Debug.WriteLine("File does not exist: " + fileName);
					return default(T);
				}
				using (StreamReader sw = new StreamReader(zip[fileName + ".rz"].OpenReader()))
				using (JsonReader reader = new JsonTextReader(sw)) {
					return serializer.Deserialize<T>(reader);
				}
			}
		}

		private static bool CheckObjectInZip(string fileName, string zipName) {
			using (ZipFile zip = ZipFile.Read(gameDataLocation + zipName + ".rzz")) {
				return zip.ContainsEntry(fileName + ".rz");
			}
		}

	}
}
