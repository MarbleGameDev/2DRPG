﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.World.Objects {
	class WorldObjectCollidable : WorldObjectBase, ICollidable{

		/// <summary>
		/// Complete Declaration for WorldObjectCollidable
		/// </summary>
		/// <param name="x">X position in the world</param>
		/// <param name="y">Y position in the world</param>
		/// <param name="layer">Render Layer</param>
		/// <param name="textureName">Name of the texture</param>
		public WorldObjectCollidable(float x, float y, int layer, string textureName = "default") : base(x, y, textureName) { }

		public WorldObjectCollidable(GameSave.WorldObjectStorage store) : this(store.worldX, store.worldY, store.layer, store.textureName) { }

		public override GameSave.WorldObjectStorage StoreObject() {
			GameSave.WorldObjectStorage store = new GameSave.WorldObjectStorage() {
				worldX = worldX, worldY = worldY, layer = layer, textureName = texName, objectType = GameSave.WorldObjectType.Collidable
			};
			return store;
		}
	}
}