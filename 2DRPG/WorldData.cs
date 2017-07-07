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
		public static Dictionary<string, IWorldRegion> regionFiles = new Dictionary<string, IWorldRegion>();
		public static Dictionary<string, HashSet<WorldObjectBase>> currentRegions = new Dictionary<string, HashSet<WorldObjectBase>>();
		public static HashSet<UIBase> worldUIs = new HashSet<UIBase>();
		public static WorldObjectControllable controllableOBJ;

		public static UIChar interChar = new UIChar(0, 0, 8f, 0, '!') {
			Visible = false
		};

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
		}

		private static void LoadRegion(int rx, int ry) {
			string reg = rx + "x" + ry;
			if (regionFiles.ContainsKey(reg) && !currentRegions.ContainsKey(reg)) {
				regionFiles[reg].LoadTextures();
				HashSet<WorldObjectBase> tempReg = regionFiles[reg].LoadObjects();
				foreach (WorldObjectBase b in tempReg) {
					b.SetWorldPosition(b.worldX + 1000 * rx, b.worldY + 1000 * ry);
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
			TextureManager.RegisterTextures(new string[] { "heart", "baseFont" });
			controllableOBJ = new Player.MCObject();
			worldUIs.Add(interChar);
			LoadRegionObjects();
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
			//Screen.worldText.SetText("Coords: " + CurrentRegionX + ", " + CurrentRegionY + " : " + CurrentX + ", " + CurrentY);
		}
		public static void SetCenter(float x, float y) {
			CurrentX = x;
			CurrentY = y;
			Form1.SetOrtho(CurrentX, CurrentY);
		}

	}
}
