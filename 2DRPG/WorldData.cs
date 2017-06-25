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

namespace _2DRPG {
	static class WorldData {
		static int CurrentRegionX = 0;
		static int CurrentRegionY = 0;
		public static float CurrentX = 0;
		public static float CurrentY = 0;
		static Dictionary<string, IWorldRegion> regionFiles = new Dictionary<string, IWorldRegion>();
		static List<IWorldRegion> loadedRegions = new List<IWorldRegion>();
		public static Dictionary<string, List<WorldObjectBase>> currentRegions = new Dictionary<string, List<WorldObjectBase>>();

		/// <summary>
		/// Adds the Region files to the directory
		/// </summary>
		static WorldData() {
			regionFiles.Add("0x0", new Region0x0());
			regionFiles.Add("-1x0", new Region_1x0());
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
			SetScreenCoords();
		}

		private static void LoadRegion(int rx, int ry) {
			string reg = rx + "x" + ry;
			if (regionFiles.ContainsKey(reg)) {
				regionFiles[reg].LoadTextures();
				List<WorldObjectBase> tempReg = regionFiles[reg].LoadObjects();
				foreach (WorldObjectBase b in tempReg) {
					b.worldX += 100 * rx;
					b.worldY += 100 * ry;
				}
				currentRegions.Add(reg, tempReg);
			}
		}
		private static void UnloadRegion(int rx, int ry) {
			string reg = rx + "x" + ry;
			if (regionFiles.ContainsKey(reg)) {
				regionFiles[reg].UnloadTextures();
				currentRegions.Remove(reg);
			}
		}

		/// <summary>
		/// Loads all textures needed for the world objects to be added afterwards, This function is called once GL context is created, do not invoke manually
		/// </summary>
		public static void WorldStartup() {
			TextureManager.ClearTextures();

			LoadRegionObjects();
		}

		public static void MoveCenter(float x, float y) {
			int oldX = CurrentRegionX;
			int oldY = CurrentRegionY;
			CurrentX += x;
			CurrentY += y;
			CurrentRegionX = (int)CurrentX % 100;
			CurrentRegionY = (int)CurrentY % 100;
			Form1.ShiftOrtho(x, y);

			oldX = oldX - CurrentRegionX;
			oldY = oldY - CurrentRegionY;
			if (oldX != 0) {
				UnloadRegion(CurrentRegionX + 1 + oldX, CurrentRegionY);
			}
			if (oldY != 0) {
				UnloadRegion(CurrentRegionX, CurrentRegionY + 1 + oldY);
			}
		}
		public static void SetCenter(float x, float y) {
			CurrentX = x;
			CurrentY = y;
			//SetScreenCoords();
		}

		/// <summary>
		/// Adjusts the world object coordinates to line up with the current center of the screen.
		/// </summary>
		///
		static void SetScreenCoords() {
			List<WorldObjectBase>[] tobjects = WorldData.currentRegions.Values.ToArray();     //Render the World Objects
			foreach (List<WorldObjectBase> l in tobjects)
				foreach (WorldObjectBase o in l) {
				o.SetScreenPosition(o.worldX - CurrentX, o.worldY - CurrentY);
			}
		}

	}
}
