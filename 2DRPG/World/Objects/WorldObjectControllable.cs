﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG {
	class WorldObjectControllable : World.Objects.WorldObjectMovable {


		/// <summary>
		/// Not Correctly Implemented Yet
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="layer"></param>
		/// <param name="textureName"></param>
		public WorldObjectControllable(float x, float y, int layer, string textureName, float width = 16, float height = 16) : base(x, y, textureName, width, height) {
			Input.InputCall += MoveOnKeys;
			MovementSpeed = .01f;
		}
		public WorldObjectControllable(String textureName) : base(textureName) {
			Input.InputCall += MoveOnKeys;
			MovementSpeed = .01f;
		}
		public WorldObjectControllable(GameSave.WorldObjectStorage store) : this(store.worldX, store.worldY, store.layer, store.textureName, store.width, store.height) { }

		private void MoveOnKeys(Input.KeyInputs[] k) {
			if (k.Contains(Input.KeyInputs.left))
				movementX = -MovementSpeed;
			if (k.Contains(Input.KeyInputs.right))
				movementX = MovementSpeed;
			if (k.Contains(Input.KeyInputs.up))
				movementY = MovementSpeed;
			if (k.Contains(Input.KeyInputs.down))
				movementY = -MovementSpeed;
		}
		public float movementX;
		public float movementY;

		public override void MoveRelative(float x = 0, float y = 0) {
			WorldData.MoveCenter(x, y);
			SetWorldPosition(worldX + x, worldY + y);
		}

		public void Position() {
			WorldData.MoveCenter(movementX, movementY);
			SetWorldPosition(worldX + movementX, worldY + movementY);
		}

		public override GameSave.WorldObjectStorage StoreObject() {
			GameSave.WorldObjectStorage store = new GameSave.WorldObjectStorage() {
				worldX = worldX, worldY = worldY, width = width, height = height, textureName = texName, layer = layer, objectType = GameSave.WorldObjectType.Controllable
			};
			return store;
		}

	}
}
