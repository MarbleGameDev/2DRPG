using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;
using System.Drawing.Imaging;
using _2DRPG.World.Objects;
using _2DRPG.World.Regions;
using _2DRPG.GUI;
using System.Diagnostics;
using _2DRPG.Player;

namespace _2DRPG {
	static class WorldData {
		static int CurrentRegionX = 0;
		static int CurrentRegionY = 0;
		public static float CurrentX = 1;
		public static float CurrentY = 1;
		public static Dictionary<string, RegionBase> currentRegions = new Dictionary<string, RegionBase>();
		public static HashSet<UIBase> worldUIs = new HashSet<UIBase>();
		//Hashset used for testing custom worldObjects, added via AddObjects()
		public static HashSet<WorldObjectBase> tempRender = new HashSet<WorldObjectBase>();


		public static MCObject player;
		public static MCObject partner = null;

		public static UIChar interChar = new UIChar(0, 0, 8f, 0, '!') {
			Visible = false
		};

		/// <summary>
		/// Used to add objects to the world that can't be done during runtime
		/// </summary>
		private static void AddObjects() {

		}

		/// <summary>
		/// Loads the regions surrounding the current region, unloading regions that are too far away
		/// </summary>
		public static void LoadRegionObjects() {
			List<string> removeList = new List<string>();
			foreach (RegionBase b in currentRegions.Values) {
				if (Math.Sqrt(Math.Pow(b.RegionX, 2) + Math.Pow(b.RegionY, 2)) > 1.5d) {
					removeList.Add(b.RegionTag);
				}
			}
			foreach (string s in removeList)
				lock(currentRegions)
					currentRegions.Remove(s);
			for (int rx = CurrentRegionX - 1; rx <= CurrentRegionX + 1; rx++) {
				for (int ry = CurrentRegionY - 1; ry <= CurrentRegionY + 1; ry++) {
					LoadRegion(rx, ry);
				}
			}
		}

		public static void AddToRegion(float rx, float ry, WorldObjectBase b) {
			currentRegions[rx + "x" + ry].GetWorldObjects().Add(b);
		}

		public static void RemoveObject(WorldObjectBase b) {
			if (b == null)
				return;
			lock (currentRegions) {
				currentRegions[WorldToRegion(b.worldX) + "x" + WorldToRegion(b.worldY)].GetWorldObjects().Remove(b);
			}
		}
		/// <summary>
		/// Converts a world coordinate to the correct region coordinate
		/// </summary>
		/// <param name="coord"></param>
		/// <returns></returns>
		public static int WorldToRegion(float coord) {
			return (int)Math.Ceiling(coord / 1000f) - 1;
		}

		private static void LoadRegion(int rx, int ry) {
			string reg = rx + "x" + ry;
			if (currentRegions.ContainsKey(reg))
				return;
			Save.RegionSave s = Save.SaveData.LoadRegion(reg);
			if (s == null)
				s = new Save.RegionSave();
			RegionBase ba = new RegionBase(reg, s);
			ba.Load();
			lock (currentRegions)
				currentRegions.Add(reg, ba);
		}
		private static void UnloadRegion(int rx, int ry) {
			string reg = rx + "x" + ry;
			if (currentRegions.ContainsKey(reg)) {
				currentRegions[reg].Unload();
				lock(currentRegions)
					currentRegions.Remove(reg);
			}
		}

		/// <summary>
		/// Returns the region file responsible for the passed world coordinates if loaded, null if not
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static RegionBase GetRegion(float x, float y) {
			string reg = GetRegionString(x, y);
			if (currentRegions.ContainsKey(reg)) {
				return currentRegions[reg];
			} else {
				return null;
			}
		}

		public static string GetRegionString(float x, float y) {
			int rx = (int)Math.Ceiling(x / 1000f) - 1;
			int ry = (int)Math.Ceiling(y / 1000f) - 1;
			return rx + "x" + ry;
		}

		/// <summary>
		/// Sets the current world save and loads it, returning bool for success
		/// </summary>
		/// <param name="saveName"></param>
		/// <returns></returns>
		public static bool LoadWorldSave(string saveName) { //TODO: Add support here for the hosted world loading
			if (!Save.SaveData.SetCurrentSave(saveName))
				return false;
			currentRegions.Clear();
			player = Save.SaveData.LoadPlayer();
			CurrentX = player.worldX;
			CurrentY = player.worldY;
			Form1.SetOrtho(CurrentX, CurrentY);
			LoadRegionObjects();
			return true;
		}

		/// <summary>
		/// Loads a player into the partner slot
		/// New player is loaded if an existing save isn't found
		/// </summary>
		/// <param name="uid"></param>
		public static void AddPlayer(Guid uid) {
			//TODO: Add functionality to import existing player files (maybe)
			partner = Save.SaveData.LoadPlayer(uid.ToString());
			partner.SetWorldPosition(player.worldX, player.worldY);

		}

		/// <summary>
		/// Loads all textures needed for the world objects to be added afterwards, This function is called once GL context is created, do not invoke manually
		/// </summary>
		public static void WorldStartup() {
			TextureManager.ClearTextures();
			TextureManager.RegisterTextures(new string[] { "character", "baseFont", "tempCharacter" });
			worldUIs.Add(interChar);

			LoadWorldSave("master");

			AddObjects();
		}

		public static void WorldRender() {
			lock (currentRegions) {
				foreach (RegionBase l in currentRegions.Values) {
					foreach (WorldObjectBase o in l.GetWorldObjects())
						o.Render();
				}

				foreach (UIBase b in worldUIs.ToArray())
					b.Render();
				foreach (WorldObjectBase b in tempRender)
					b.Render();
			}
			player.Render();
			if (partner != null)
				partner.Render();
		}

		/// <summary>
		/// Shifts the Center of the Screen by the offset given and unloads and loads regions if necessary
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public static void MoveCenter(float x, float y) {
			int oldX = CurrentRegionX;
			int oldY = CurrentRegionY;
			CurrentX += x;
			CurrentY += y;
			CurrentRegionX = (int)Math.Ceiling(CurrentX / 1000) - 1 ;
			CurrentRegionY = (int)Math.Ceiling(CurrentY / 1000) - 1;
			oldX = CurrentRegionX - oldX; 
			oldY = CurrentRegionY - oldY;
			if (oldX != 0 || oldY != 0) {
				LoadRegionObjects();
			}
			Form1.ShiftOrtho(x, y);

		}
		public static void SetCenter(float x, float y) {
			CurrentX = x;
			CurrentY = y;
			Form1.SetOrtho(CurrentX, CurrentY);

		}

		/// <summary>
		/// Returns a directionless 600x600 navigation grid centered on the point given
		/// </summary>
		/// <param name="center"></param>
		/// <returns></returns>
		public static NodeGrid GetNodeGrid(Point center) {
			Stopwatch s = new Stopwatch();
			NodeGrid grid = new NodeGrid();
			Node[,] nodes = new Node[150, 150];
			Point t = new Point();
			int tempx, tempy;
			bool open = true;
			s.Start();
			for (int x = -75; x < 75; x++) {
				for (int y = -75; y < 75; y++) {
					open = true;
					tempx = ((center.X + x*4) / 4) * 4;
					tempy = ((center.Y + y*4) / 4) * 4;
					t = new Point(tempx, tempy);
					foreach (RegionBase b in currentRegions.Values)
						if (b.CollisionPoints.ContainsKey(tempx))
							if (b.CollisionPoints[tempx].Contains(tempy))
								open = false;
					nodes[x + 75, y + 75] = new Node() { IsOpen = open, Location = t };
				}
			}
			grid.SetNodes(nodes);
			grid.SetCenter(center.X, center.Y);
			return grid;
		}

		public static void PacketToWorldObject(Net.UDPFrame frame) {
			if (frame.intData[1] == 0) {    //Single packet object
				if (!currentRegions.ContainsKey(frame.stringData[0])) {
					RegionBase ba = new RegionBase(frame.stringData[0], new Save.RegionSave());
					ba.Load();
					lock (currentRegions)
						currentRegions.Add(frame.stringData[0], ba);
				} else {
					lock (currentRegions)
						foreach (WorldObjectBase b in currentRegions[frame.stringData[0]].GetWorldObjects())
							if (b.uid.Equals(frame.uid))
								return;
				}
			} else if (frame.intData[1] == 1) { //Multi packet object
				if (currentRegions.ContainsKey(frame.stringData[0])) {
					lock (currentRegions)
						currentRegions[frame.stringData[0]].GetWorldObjects();	//help...
				}
			}
			switch (frame.intData[0]) {
				case 0:

					break;
			}
		}
	}
}
