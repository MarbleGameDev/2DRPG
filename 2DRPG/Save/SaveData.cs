using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Ionic.Zip;
using System.Windows.Forms;
using _2DRPG.Player;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Newtonsoft.Json;

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
		private static BinaryFormatter serializer = new BinaryFormatter();
		private static JsonSerializer jsonSerializer = new JsonSerializer();

		public static Settings GameSettings = new Settings();

		static SaveData() {
			Directory.CreateDirectory(saveLocation);
			jsonSerializer.ObjectCreationHandling = ObjectCreationHandling.Replace;
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
			GameSettings = DeSerializeObjectJSON<Settings>("Settings");
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
			SerializeObjectJSON(GameSettings, "Settings");
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
			if (Directory.Exists(gameDataLocation) && Directory.GetFiles(gameDataLocation).Contains(gameDataLocation + "Settings.rz")) {
				return true;
			}
			return false;
		}

		/// <summary>
		/// Saves the open game data to the packages
		/// </summary>
		public static void SaveGameData() {
			lock (WorldData.currentRegions) {
				foreach (World.Regions.RegionBase b in WorldData.currentRegions.Values) {
					SaveRegion(b);
				}
			}
			SaveTasks();
			SaveEntities();
			GUI.Windows.NotificationWindow.NewNotification("Data Saved", 60);
		}

		public static List<GUI.Interaction.InteractionBase> LoadInteractions(string interactionID) {
			return DeSerializeObjectFromZip<List<GUI.Interaction.InteractionBase>>(interactionID, "Package_04");
		}
		public static void SaveInteractions(List<GUI.Interaction.InteractionBase> interItems, string interactionID) {
			SerializeObjectToZip(interItems, interactionID, "Package_04");
		}

		/// <summary>
		/// Loads the game data from packages
		/// </summary>
		public static void LoadGameData() {
			LoadTasks();
			LoadEntities();
			//Loading of region files is handled by the WorldData class
		}

		private static void LoadTasks() {
			//Custom system for deserializing all objects from a zip without wasting streams
			using (ZipFile zip = ZipFile.Read(gameDataLocation + CurrentSaveName + "/" + "Package_02.rzz")) {
				foreach (ZipEntry e in zip)
					try {
						using (Stream sw = e.OpenReader()) {
							Quests.TaskBase q = (Quests.TaskBase)serializer.Deserialize(sw);
							Quests.QuestData.QuestDatabase.Add(q.taskName, q);
						}
					} catch (SerializationException) {
						System.Diagnostics.Debug.WriteLine("Serialization Error on LoadTasks()");
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

		private static void LoadEntities() {
			WorldData.worldEntities = DeSerializeObjectFromZip<HashSet<World.Entities.WorldEntity>>("E_01", "Package_01");
			if (WorldData.worldEntities == null)
				WorldData.worldEntities = new HashSet<World.Entities.WorldEntity>();
		}
		private static void SaveEntities() {
			SerializeObjectToZip(WorldData.worldEntities, "E_01", "Package_01");
		}

		//Move regions to and from Disk
		public static World.Regions.RegionBase LoadRegion(World.Regions.RegionTag regionTag) {
			return DeSerializeObjectFromZip<World.Regions.RegionBase>(regionTag.ToString(), "Package_01");
		}
		public static void SaveRegion(World.Regions.RegionBase reg) {
			SerializeObjectToZip(reg, reg.Tag.ToString(), "Package_01");
		}


		public static void SerializeObject(object obj, string fileName) {
			try {
				using (Stream sw = new FileStream(saveLocation + fileName + ".rz", FileMode.Create, FileAccess.Write, FileShare.None)) {
					serializer.Serialize(sw, obj);
				}
			} catch (SerializationException e) {
				System.Diagnostics.Debug.WriteLine("Serialization Error: " + e.Message);
			}

		}
		public static T DeSerializeObject<T>(string fileName) {
			try {
				using (Stream sw = new FileStream(saveLocation + fileName + ".rz", FileMode.Open, FileAccess.Read, FileShare.Read)) {
					return (T)serializer.Deserialize(sw);
				}
			} catch (FileNotFoundException e) {
				System.Diagnostics.Debug.WriteLine("Deserialization Error: " + e.Message);
			} catch (SerializationException e) {
				System.Diagnostics.Debug.WriteLine("Deserialization Error: " + e.Message);
			}
			return default(T);
		}

		public static void SerializeObjectJSON(object obj, string fileName) {
			try {
				using (StreamWriter sw = new StreamWriter(saveLocation + fileName + ".rz"))
				using (JsonWriter writer = new JsonTextWriter(sw)) {
					writer.Formatting = Formatting.Indented;
					jsonSerializer.Serialize(writer, obj);
				}
			} catch (JsonSerializationException e) {
				System.Diagnostics.Debug.WriteLine("Serialization Error: " + e.Message);
			}
		}
		public static T DeSerializeObjectJSON<T>(string fileName) {
			try {
				using (StreamReader sw = new StreamReader(saveLocation + fileName + ".rz"))
				using (JsonReader reader = new JsonTextReader(sw)) {
					return jsonSerializer.Deserialize<T>(reader);
				}
			} catch (JsonSerializationException e) {
				System.Diagnostics.Debug.WriteLine("Deserialization Error: " + e.Message);
			} catch (FileNotFoundException e) {
				System.Diagnostics.Debug.WriteLine("Deserialization Error: " + e.Message);
			}
			return default(T);
		}

		static readonly string[] packageNames = new string[] {
			"Package_01",	//Region Data
			"Package_02",	//Tasks Data
			"Package_03",	//Player Data
			"Package_04"	//Interaction Data
		};

		public static void SerializeObjectToZip(object obj, string fileName, string zipName) {
			using (ZipFile zip = ZipFile.Read(gameDataLocation + CurrentSaveName + "/" + zipName + ".rzz"))
			using (Stream st = new MemoryStream()) {
				serializer.Serialize(st, obj);
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
				try {
					using (Stream sw = zip[fileName + ".rz"].OpenReader()) {
						return (T)serializer.Deserialize(sw);
					}
				} catch (SerializationException e) {
					System.Diagnostics.Debug.WriteLine("Serialization Error in " + zipName + ": " + e.Message);
				}
			}
			return default(T);
		}

		private static bool CheckObjectInZip(string fileName, string zipName) {
			using (ZipFile zip = ZipFile.Read(gameDataLocation + CurrentSaveName + "/" + zipName + ".rzz")) {
				return zip.ContainsEntry(fileName + ".rz");
			}
		}

		/// <summary>
		/// Checks all game package files, generates new zips if the settings are set
		/// </summary>
		/// <returns></returns>
		public static bool ValidateZips() {
			bool genZips = false;
			string[] zips = Directory.GetFiles(gameDataLocation + CurrentSaveName + "/");

			for (int i = 0; i < packageNames.Length; i++) {
				if (!zips.Contains(gameDataLocation + CurrentSaveName + "/" + packageNames[i] + ".rzz") || !ZipFile.IsZipFile(gameDataLocation + CurrentSaveName + "/" + packageNames[i] + ".rzz")) {
					System.Diagnostics.Debug.WriteLine("Missing Zip " + packageNames[i]);
					if (CurrentSaveName  + "/" != MasterSaveLocation) {
						if (GameSettings.repairSaves) {
							File.Copy(gameDataLocation + MasterSaveLocation + packageNames[i], gameDataLocation + CurrentSaveName + "/" + packageNames[i]);
							genZips = true;
						} else {
							MessageBox.Show("Missing game package " + packageNames[i] + ".rzz, set 'repairSaves' in Settings.rz to automatically restore on next start", "Game Save Error");
						}
					} else {
						MessageBox.Show("Package " + packageNames[i] + "has been damaged, exit program now to avoid deletion", "Package Error");
						ZipFile n = new ZipFile();
						n.Save(gameDataLocation + CurrentSaveName + "/" + packageNames[i] + ".rzz");
						genZips = true;
					}
				}
			}
			return !genZips;
		}

	}
}
