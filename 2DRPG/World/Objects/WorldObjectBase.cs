using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace _2DRPG.World.Objects {
	public class WorldObjectBase : TexturedObject {
		[Editable]
		public float worldX;
		[Editable]
		public float worldY;


		/// <summary>
		/// Complete Declaration for WorldObjectBase
		/// </summary>
		/// <param name="x">X position in the world</param>
		/// <param name="y">Y position in the world</param>
		/// <param name="layer">Render Layer</param>
		/// <param name="textureName">Name of the texture</param>
		public WorldObjectBase(float x, float y, int layer, string textureName, float width = 16, float height = 16) : this(x, y, textureName, width, height) {
			SetLayer(layer);
		}

		public WorldObjectBase() : base() { }
		public WorldObjectBase(string textureName) : base(textureName) {

        }
		public WorldObjectBase(float x, float y, string textureName, float width = 16, float height = 16) : base(textureName) {
			this.width = width;
			this.height = height;
			SetWorldPosition(x, y);
		}

		public WorldObjectBase(GameSave.WorldObjectStorage store) : this(store.worldX, store.worldY, store.layer, store.textureName, store.width, store.height) { }
	
		/// <summary>
		/// Sets the position of the object in the world
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void SetWorldPosition(float x, float y) {
			worldX = x;
			worldY = y;
			UpdateWorldPosition();
		}
		/// <summary>
		/// Returns the location of the object as a Point
		/// </summary>
		/// <returns></returns>
		public System.Drawing.Point GetPointLocation() {
			return new System.Drawing.Point((int)worldX, (int)worldY);
		}

		/// <summary>
		/// Call when the WorldX and WorldY are updated outside of SetWorldPosition()
		/// </summary>
		public void UpdateWorldPosition() {
			quadPosition[0] = worldX - width / 2;
			quadPosition[3] = worldX - width / 2;
			quadPosition[6] = worldX + width / 2;
			quadPosition[9] = worldX + width / 2;
			quadPosition[1] = worldY - height / 2;
			quadPosition[10] = worldY - height / 2;
			quadPosition[4] = worldY + height / 2;
			quadPosition[7] = worldY + height / 2;
		}

		public virtual GameSave.WorldObjectStorage StoreObject() {
			GameSave.WorldObjectStorage store = new GameSave.WorldObjectStorage() {
				worldX = worldX, worldY = worldY, width = width, height = height, textureName = texName, layer = layer, objectType = GameSave.WorldObjectType.Base
			};
			return store;
		}

		/// <summary>
		/// Checks if the coordinates given are within the world object, sets up the worldBuilder window
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public bool CheckCoords(float x, float y){
			bool b = LogicUtils.Logic.CheckIntersection(quadPosition, x, y);
			if (b)
				GUI.Windows.BuilderWindow.SetCurrentObject(this);
			return b;
		}

		public override string ToString() {
			return GetType().Name + "\nCoords: \n" + worldX + "," + worldY;
		}

		public virtual void ModificationAction() {
			UpdateWorldPosition();
			SetLayer(layer);
			//Moves the world object to the correct region if it's misplaced
			Regions.RegionBase bs = WorldData.GetRegion((int)worldX, (int)worldY);
			if (!bs.GetWorldObjects().Contains(this)) {
				lock (WorldData.currentRegions) {
					foreach (HashSet<WorldObjectBase> hsh in WorldData.currentRegions.Values)
						if (hsh.Contains(this))
							hsh.Remove(this);
					bs.GetWorldObjects().Add(this);
				}
			}
		}
	}
}
