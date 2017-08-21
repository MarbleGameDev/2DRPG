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

namespace _2DRPG {
	static class WorldData {
		static int CurrentRegionX = 0;
		static int CurrentRegionY = 0;
		public static float CurrentX = 0;
		public static float CurrentY = 0;
		public static Dictionary<string, RegionBase> regionFiles = new Dictionary<string, RegionBase>();
		public static Dictionary<string, HashSet<WorldObjectBase>> currentRegions = new Dictionary<string, HashSet<WorldObjectBase>>();
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
		/// Adds the Region files to the directory
		/// </summary>
		static WorldData() {
			regionFiles.Add("0x0", new Region0x0());
			regionFiles.Add("-1x0", new Region_1x0());
			regionFiles.Add("-1x-1", new Region_1x_1());
			regionFiles.Add("0x-1", new Region0x_1());
		}

		/// <summary>
		/// Loads the objects and their data into the currentObjects list
		/// </summary>
		public static void LoadRegionObjects() {
			for (int rx = CurrentRegionX - 1; rx <= CurrentRegionX + 1; rx++) {
				for (int ry = CurrentRegionY - 1; ry <= CurrentRegionY + 1; ry++) {
					LoadRegion(rx, ry);
				}
			}
		}

		public static void AddToRegion(float rx, float ry, WorldObjectBase b) {
			currentRegions[rx + "x" + ry].Add(b);
		}

		public static void RemoveObject(WorldObjectBase b) {
			if (b == null)
				return;
			lock (currentRegions) {
				foreach (HashSet<WorldObjectBase> l in currentRegions.Values) {
					if (l.Contains(b)) {
						l.Remove(b);
						return;
					}
				}
			}
		}

		public static int WorldToRegion(float coord) {
			return (int)Math.Ceiling(coord / 1000f) - 1;
		}

		private static void LoadRegion(int rx, int ry) {
			string reg = rx + "x" + ry;
			if (regionFiles.ContainsKey(reg) && !currentRegions.ContainsKey(reg)) {
				regionFiles[reg].LoadTextures();
				lock(currentRegions)
					currentRegions.Add(reg, regionFiles[reg].LoadObjects());
			}
		}
		private static void UnloadRegion(int rx, int ry) {
			string reg = rx + "x" + ry;
			if (regionFiles.ContainsKey(reg)) {
				regionFiles[reg].UnloadTextures();
				lock(currentRegions)
					currentRegions.Remove(reg);
			}
		}

		/// <summary>
		/// Returns the region file responsible for the passed world coordinates
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static RegionBase GetRegion(int x, int y) {
			int rx = (int)Math.Ceiling(x / 1000f) - 1;
			int ry = (int)Math.Ceiling(y / 1000f) - 1;
			string reg = rx + "x" + ry;
			if (regionFiles.ContainsKey(reg))
				return regionFiles[reg];
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
			worldUIs.Add(interChar);
			LoadRegionObjects();
			AddObjects();
		}

		public static void WorldRender() {
			lock (currentRegions) {
				foreach (HashSet<WorldObjectBase> l in currentRegions.Values) {
					foreach (WorldObjectBase o in l)
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
			if (oldX != 0) {
				UnloadRegion(CurrentRegionX - (oldX * 2), CurrentRegionY);
				LoadRegion(CurrentRegionX + oldX, CurrentRegionY);
			}
			if (oldY != 0) {
				UnloadRegion(CurrentRegionX, CurrentRegionY - (oldY * 2));
				LoadRegion(CurrentRegionX, CurrentRegionY + oldY);
			}
			Form1.ShiftOrtho(x, y);
		}
		public static void SetCenter(float x, float y) {
			CurrentX = x;
			CurrentY = y;
			Form1.SetOrtho(CurrentX, CurrentY);
		}

		public static NodeGrid GetNodeGrid(Point center) {
			NodeGrid grid = new NodeGrid();
			Node[,] nodes = new Node[150, 150];
			for (int x = -75; x < 75; x++) {
				for (int y = -75; y < 75; y++) {
					float tempx = center.X + x*4;
					float tempy = center.Y + y*4;
					bool open = true;
					Point t = new Point((int)tempx, (int)tempy);
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
