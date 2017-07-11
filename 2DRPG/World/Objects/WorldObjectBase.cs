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
		public WorldObjectBase(float x, float y, int layer, string textureName) : this(x, y, textureName) {
			SetLayer(layer);
		}

		public WorldObjectBase() : base() { }
		public WorldObjectBase(string textureName) : base(textureName) { }
		public WorldObjectBase(float x, float y, string textureName) : base(textureName) {
			SetWorldPosition(x, y);
		}

		public WorldObjectBase(GameSave.WorldObjectStorage store) : this(store.worldX, store.worldY, store.layer, store.textureName) { }
	
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
		/// Call when the WorldX and WorldY are updated outside of SetWorldPosition()
		/// </summary>
		public void UpdateWorldPosition() {
			arrayPosition[0] = worldX - size / 2;
			arrayPosition[3] = worldX - size / 2;
			arrayPosition[6] = worldX + size / 2;
			arrayPosition[9] = worldX + size / 2;
			arrayPosition[1] = worldY - size / 2;
			arrayPosition[10] = worldY - size / 2;
			arrayPosition[4] = worldY + size / 2;
			arrayPosition[7] = worldY + size / 2;
		}

		public virtual GameSave.WorldObjectStorage StoreObject() {
			GameSave.WorldObjectStorage store = new GameSave.WorldObjectStorage() {
				worldX = worldX, worldY = worldY, textureName = texName, layer = layer, objectType = GameSave.WorldObjectType.Base
			};
			return store;
		}

		/// <summary>
		/// Checks if the coordinates given are within the world object
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public bool CheckCoords(float x, float y){
			bool b = LogicUtils.Logic.CheckIntersection(arrayPosition, x, y);
			if (b)
				GUI.Windows.BuilderWindow.SetCurrentObject(this);
			return b;
		}

		public override string ToString() {
			return GetType().Name + "\nCoords: \n" + worldX + "," + worldY;
		}

		public virtual void ModificationAction() {
			UpdateWorldPosition();
		}
	}
}
