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
using _2DRPG.Player;

namespace _2DRPG.Save {
	static class SaveData {

		//TODO: add folders for multiple saves that are copied from the embeded master
#if DEBUG
		private static string saveLocation = "../../SaveData/";
		private static string gameDataLocation = "../../SaveData/";
		public static string MasterSaveLocation = "master/";
#else
		private static string saveLocation = "SaveData/";
		private static string gameDataLocation = "GameData/";
		public static string MasterSaveLocation = "master/";
#endif

		public static string CurrentSaveName = "master";
		private static JsonSerializer serializer = new JsonSerializer();

		public static Settings GameSettings = new Settings();

		static SaveData() {
			Directory.CreateDirectory(saveLocation);
			serializer.ObjectCreationHandling = ObjectCreationHandling.Replace;
		}

		/// <summary>
		/// Lists the number of save files by directories in the gamedata directory
		/// </summary>
		/// <returns></returns>
		public static string[] ListSaves() {
			string[] d = Directory.GetDirectories(gameDataLocation);
			for (int i = 0; i < d.Length; i++)
				d[i] = d[i].Replace(gameDataLocation, "");
			return d;
		}

		/// <summary>
		/// Sets the current save file name, returns true on success
		/// </summary>
		/// <param name="saveName"></param>
		/// <returns></returns>
		public static bool SetCurrentSave(string saveName) {
			if (ListSaves().Contains(saveName)) {
				CurrentSaveName = saveName;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Creates a new directory and copies over the master files for a new save
		/// returns true on success
		/// </summary>
		/// <param name="saveName"></param>
		/// <returns></returns>
		public static bool CreateNewSave(string saveName) {
			if (ListSaves().Contains(saveName))
				return false;
			string nw = gameDataLocation + saveName + "/";
			string ms = gameDataLocation + MasterSaveLocation;
			Directory.CreateDirectory(gameDataLocation + saveName + "/");
			foreach (string s in Directory.GetFiles(ms)) {
				File.Copy(s, s.Replace(ms, nw));
			}
			return true;
		}

		/// <summary>
		/// Loads the player settings, creating new copies if there doesn't exist others
		/// </summary>
		public static void LoadGame() {
			GameSettings = DeSerializeObject<Settings>("Settings");
			if (GameSettings == null)
				GameSettings = new Settings();
			Quests.QuestData.ActiveQuests = DeSerializeObjectFromZip<HashSet<string>>("PlayerQuests", "Package_03");
			if (Quests.QuestData.ActiveQuests == null)
				Quests.QuestData.ActiveQuests = new HashSet<string>();
		}
		/// <summary>
		/// Saves the player settings
		/// </summary>
		public static void SaveGame() {
			SerializeObject(GameSettings, "Settings");
			SavePlayer(WorldData.player);
			if (WorldData.partner != null)
				SavePlayer(WorldData.partner);
			SerializeObjectToZip(Quests.QuestData.ActiveQuests, "PlayerQuests", "Package_03");

			GUI.Windows.NotificationWindow.NewNotification("Game Saved", 190);
		}


		/// <summary>
		/// Saves the player passed to disk
		/// Save file is named player for main character, and by uid for any guests
		/// </summary>
		/// <param name="pl"></param>
		private static void SavePlayer(MCObject pl) {
			if (pl == WorldData.player)
				SerializeObjectToZip(pl, "player", "Package_03");
			else
				SerializeObjectToZip(pl, pl.uid.ToString(), "Package_03");
		}


		/// <summary>
		/// Loads a player object from disk, if main player, leave uid as null
		/// </summary>
		/// <param name="uid">leave null to signify loading of the player</param>
		/// <returns></returns>
		public static MCObject LoadPlayer(string uid = null) {
			MCObject o = null;
			if (uid == null) {
				o = DeSerializeObjectFromZip<MCObject>("player", "Package_03");
			} else {
				o = DeSerializeObjectFromZip<MCObject>(uid, "Package_03");
			}
			if (o == null)
				o = new MCObject();
			return o;
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
					RegionSave s = new RegionSave();
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
			//Loading of region files is handled by the WorldData class
		}

		private static void LoadTasks() {
			using (ZipFile zip = ZipFile.Read(gameDataLocation + CurrentSaveName + "/" + "Package_02.rzz")) {
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


		public static RegionSave LoadRegion(string regionTag) {

			return DeSerializeObjectFromZip<RegionSave>(regionTag, "Package_01");
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
		//Package_03: Player Data

		public static void SerializeObjectToZip(object obj, string fileName, string zipName) {
			using (ZipFile zip = ZipFile.Read(gameDataLocation + CurrentSaveName + "/" + zipName + ".rzz"))
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
			using (ZipFile zip = ZipFile.Read(gameDataLocation + CurrentSaveName + "/" + zipName + ".rzz")) {
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
			using (ZipFile zip = ZipFile.Read(gameDataLocation + CurrentSaveName + "/" + zipName + ".rzz")) {
				return zip.ContainsEntry(fileName + ".rz");
			}
		}

	}
}
