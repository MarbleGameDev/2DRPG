﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.Entities {
	class StandardMob : MovableAnimatedObject, IEffectable, IDamagable {
		public float entityHealth;
		List<EntityEffect> effectList = new List<EntityEffect>();

		public StandardMob() : base() {
			entityHealth = 10;
		}
		public StandardMob(float x, float y, int layer, string textureName, float health = 10) : base(x, y, layer, textureName) {
			entityHealth = health;
		}
		public StandardMob(string textureName, float health = 10) : base(textureName) {
			entityHealth = health;
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

		public void ReceiveAttack(Attack a) {
			a.GetAttackAction(this).Invoke();
			entityHealth -= a.attackDamage;
		}
	}
}