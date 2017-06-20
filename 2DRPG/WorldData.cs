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
		public static List<WorldObjectBase> currentObjects = new List<WorldObjectBase>(); //TODO: replace with a more accurate object type
		static Dictionary<string, IWorldRegion> regionObjects = new Dictionary<string, IWorldRegion>();
		static List<IWorldRegion> loadedRegions = new List<IWorldRegion>();

		/// <summary>
		/// Adds the Region files to the directory
		/// </summary>
		static WorldData() {
			regionObjects.Add("0x0", new Region0x0());
			regionObjects.Add("-1x0", new Region_1x0());
		}

		/// <summary>
		/// Loads the objects and their data into the currentObjects list
		/// </summary>
		public static void LoadRegionObjects() {
			for (int rx = CurrentRegionX - 1; rx <= CurrentRegionX + 1; rx++) {
				for (int ry = CurrentRegionY - 1; ry <= CurrentRegionY + 1; ry++) {
					string reg = rx + "x" + ry;
					if (regionObjects.ContainsKey(reg)) {
						regionObjects[reg].LoadTextures();
						foreach (WorldObjectBase b in regionObjects[reg].LoadObjects()) {
							b.worldX += 100 * rx;
							b.worldY += 100 * ry;
							currentObjects.Add(b);
						}
					}
				}
			}
			SetScreenCoords();
		}

		/// <summary>
		/// Loads all textures needed for the world objects to be added afterwards, This function is called once GL context is created, do not invoke manually
		/// </summary>
		public static void WorldStartup() {
			TextureManager.ClearTextures();

			LoadRegionObjects();
		}

		public static void MoveCenter(float x, float y) {
			CurrentX += x;
			CurrentY += y;
			Form1.ShiftOrtho(x, y);
		}
		public static void SetCenter(float x, float y) {
			CurrentX = x;
			CurrentY = y;
			SetScreenCoords();
		}

		/// <summary>
		/// Adjusts the world object coordinates to line up with the current center of the screen.
		/// </summary>
		static void SetScreenCoords() {
			WorldObjectBase[] worldObjects = currentObjects.ToArray();
			foreach (WorldObjectBase o in worldObjects) {
				o.SetScreenPosition(o.worldX - CurrentX, o.worldY - CurrentY);
			}
		}

	}
}
