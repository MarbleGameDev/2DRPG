﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace _2DRPG.World.Objects {
	public class WorldObjectBase : TexturedObject {
		public float worldX;
		public float worldY;

		public WorldObjectBase() : base() { }
		public WorldObjectBase(string textureName) : base(textureName) { }
		public WorldObjectBase(float x, float y, string textureName) : base(textureName) {
			SetWorldPosition(x, y);
		}
		public WorldObjectBase(float x, float y, int layer, string textureName) : this(x, y, textureName) {
			SetLayer(layer);
		}

		public WorldObjectBase(RegionSave.WorldObjectStorage store) : this(store.worldX, store.worldY, store.layer, store.textureName) { }
	
		/// <summary>
		/// Sets the position of the object in the world
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void SetWorldPosition(float x, float y) {
			worldX = x;
			worldY = y;
			arrayPosition[0] = worldX - size / 2;
			arrayPosition[3] = worldX - size / 2;
			arrayPosition[6] = worldX + size / 2;
			arrayPosition[9] = worldX + size / 2;
			arrayPosition[1] = worldY - size / 2;
			arrayPosition[10] = worldY - size / 2;
			arrayPosition[4] = worldY + size / 2;
			arrayPosition[7] = worldY + size / 2;
		}

		public virtual RegionSave.WorldObjectStorage StoreObject() {
			RegionSave.WorldObjectStorage store = new RegionSave.WorldObjectStorage() {
				worldX = worldX, worldY = worldY, textureName = texName, layer = layer, objectType = RegionSave.WorldObjectType.Base
			};
			return store;
		}
	}
}
