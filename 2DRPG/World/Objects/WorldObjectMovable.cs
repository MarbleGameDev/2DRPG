﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.Save;

namespace _2DRPG.World.Objects {
	[Serializable]
	class WorldObjectMovable : WorldObjectBase, IMovable {

		public float MovementSpeed { get; set; }

		/// <summary>
		/// Complete Declaration for WorldObjectMovable
		/// </summary>
		/// <param name="x">X position in world</param>
		/// <param name="y">Y position in world</param>
		/// <param name="textureName">Name of the texture</param>
		public WorldObjectMovable(float x, float y, int layer, Texture textureName, float width1 = 16, float height1 = 16, float width2 = 16, float height2 = 16, float width3 = 16, float height3 = 16, float width4 = 16, float height4 = 16) : base(x, y, layer, textureName, width1, height1, width2, height2, width3, height3, width4, height4) { }

		public WorldObjectMovable() : base() { }
		public WorldObjectMovable(Texture textureName) : base(textureName) { }

		/// <summary>
		/// Shifts the position of the object by the values passed
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public virtual void MoveRelative(float x = 0, float y = 0) {
			ShiftPosition(quadPosition, x, y);
			worldX += x;
			worldY += y;
			SendPacket();
		}
		[NonSerialized]
		public float movementQueueX;
		[NonSerialized]
		public float movementQueueY;

		/// <summary>
		/// Method called every logic tick to update the position changes queued
		/// </summary>
		public virtual void UpdatePosition() {
			if (movementQueueX != 0 || movementQueueY != 0)
				MoveRelative(movementQueueX, movementQueueY);
			movementQueueX = 0;
			movementQueueY = 0;
		}
	}
}
