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

namespace _2DRPG {
	static class WorldData {
		static int CurrentRegionX = 0;
		static int CurrentRegionY = 0;
		public static float CurrentX = 1;
		public static float CurrentY = 1;
		public static Dictionary<string, RegionBase> currentRegions = new Dictionary<string, RegionBase>();
		public static HashSet<UIBase> worldUIs = new HashSet<UIBase>();
		public static HashSet<WorldObjectBase> tempRender = new HashSet<WorldObjectBase>();
		public static WorldObjectControllable controllableOBJ;

		public static UIChar interChar = new UIChar(0, 0, 8f, 0, '!') {
			Visible = false
		};

		/// <summary>
		/// Used to add objects to the world that can't be done during runtime
		/// </summary>
		private static void AddObjects() {

		}

		/// <summary>
		/// Loads the regions surrounding the current region
		/// </summary>
		public static void LoadRegionObjects() {
			List<string> removeList = new List<string>();
			foreach (RegionBase b in currentRegions.Values) {
				if (Math.Sqrt(Math.Pow(b.RegionX, 2) + Math.Pow(b.RegionY, 2)) > 1.5d) {
					removeList.Add(b.RegionTag);
				}
			}
			foreach (string s in removeList)
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
			GameSave s = SaveData.LoadRegion(reg);
			if (s == null)
				s = new GameSave();
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
		public static RegionBase GetRegion(int x, int y) {
			int rx = (int)Math.Ceiling(x / 1000f) - 1;
			int ry = (int)Math.Ceiling(y / 1000f) - 1;
			string reg = rx + "x" + ry;
			if (currentRegions.ContainsKey(reg))
				return currentRegions[reg];
			else
				return null;
		}

		/// <summary>
		/// Loads all textures needed for the world objects to be added afterwards, This function is called once GL context is created, do not invoke manually
		/// </summary>
		public static void WorldStartup() {
			TextureManager.ClearTextures();
			TextureManager.RegisterTextures(new string[] { "character", "baseFont", "tempCharacter" });
			controllableOBJ = new Player.MCObject();
			controllableOBJ.SetWorldPosition(CurrentX, CurrentY);
			worldUIs.Add(interChar);
			LoadRegionObjects();
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

			controllableOBJ.Render();
		}

		/// <summary>
		/// Moves the Center of the Screen around the world and unloads and loads regions if necessary
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
			float tempx, tempy;
			bool open;
			s.Start();
			for (int x = -75; x < 75; x++) {
				for (int y = -75; y < 75; y++) {
					tempx = center.X + x*4;
					tempy = center.Y + y*4;
					open = true;
					t = new Point((int)tempx, (int)tempy);
					open = !GetRegion((int)tempx, (int)tempy).CollisionPoints.Contains(t);
					nodes[x + 75, y + 75] = new Node() { IsOpen = open, Location = t };
				}
			}
			grid.SetNodes(nodes);
			grid.SetCenter(center.X, center.Y);
			return grid;
		}

	}
}
