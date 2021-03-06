﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.World.Regions;
using _2DRPG.Entities;

namespace _2DRPG.World.Entities {
	class StandardMob : World.Entities.WorldEntity, IEffectable, IDamagable {
		public float entityHealth;
		List<EntityEffect> effectList = new List<EntityEffect>();
		List<Node> navigationPath = new List<Node>();

		public AIBase mobAI;

		/// <summary>
		/// Complete Declaration for StandardMob
		/// </summary>
		/// <param name="x">X position in the world</param>
		/// <param name="y">Y position in the world</param>
		/// <param name="textureName">Name of the texture</param>
		/// <param name="type">AIType to use for the Mob</param>
		public StandardMob(float x, float y, int layer, Texture textureName, AIBase.AIType type, float width = 16, float height = 16) : base(x, y, layer, textureName, width, height) {
			mobAI = new AIBase(this) { type = type};
		}

		public StandardMob(float x, float y, Texture textureName, AIBase.AIType type = AIBase.AIType.Passive) : base(x, y, textureName) {
			mobAI = new AIBase(this) { type = type};
		}

		public override void UpdatePosition() {
			lock (navigationPath) {
				if (navigationPath.Count == 0)
					return;
				float dx, dy;
				if (navigationPath.Count > 1) {
					dx = (navigationPath[navigationPath.Count - 1].Location.X + navigationPath[navigationPath.Count - 2].Location.X) / 2f - worldX;
					dy = (navigationPath[navigationPath.Count - 1].Location.Y + navigationPath[navigationPath.Count - 2].Location.Y) / 2f - worldY;
				} else {
					dx = navigationPath[navigationPath.Count - 1].Location.X - worldX;
					dy = navigationPath[navigationPath.Count - 1].Location.Y - worldY;
				}
				if (Math.Abs(dx) < MovementSpeed / 2f)
					dx = 0;
				if (Math.Abs(dy) < MovementSpeed / 2f)
					dy = 0;

				if (Math.Abs(dx) < MovementSpeed && Math.Abs(dy) < MovementSpeed) {
					movementQueueX = 0;
					movementQueueY = 0;
					navigationPath.RemoveAt(navigationPath.Count - 1);
				} else {
					movementQueueX = (dx == 0) ? (0) : (dx / Math.Abs(dx) * MovementSpeed);
					movementQueueY = (dy == 0) ? (0) : (dy / Math.Abs(dy) * MovementSpeed);
				}
			}
			base.UpdatePosition();
		}
		/// <summary>
		/// Sets a navigation path from start to finish for the mob to follow
		/// </summary>
		/// <param name="points"></param>
		public void SetPath(List<Node> points) {
			lock (navigationPath) {
				navigationPath = points;
				navigationPath.Reverse();
				if (navigationPath.Count > 2)
					navigationPath.RemoveAt(navigationPath.Count - 1);

			}
		}



		public void AddEffect(EntityEffect e) {
			if (e != null)
				effectList.Add(e);
		}

		public void EffectTick() {
			EntityEffect[] effects = effectList.ToArray();
			foreach (EntityEffect e in effects) {
				e.EffectUpdate();
				if (e.effectDuration <= 0)
					effectList.Remove(e);
			}
		}

		public void AITick() {
			if (mobAI != null)
				mobAI.AITick();
		}

		public void ReceiveAttack(Attack a) {
			a.GetAttackAction(this).Invoke();
			entityHealth -= a.attackDamage;
		}
	}
}
