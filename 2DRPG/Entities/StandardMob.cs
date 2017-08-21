using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.World.Regions;

namespace _2DRPG.Entities {
	class StandardMob : World.Objects.WorldObjectMovable, IEffectable, IDamagable {
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
		public StandardMob(float x, float y, string textureName, AIBase.AIType type, float width = 16, float height = 16) : base(x, y, textureName, width, height) {
			mobAI = new AIBase(this) { type = type};
		}

		public StandardMob(float x, float y, string textureName, AIBase.AIType type = AIBase.AIType.Passive) : base(x, y, textureName) {
			mobAI = new AIBase(this) { type = type};
		}
		public StandardMob(string textureName, AIBase.AIType type = AIBase.AIType.Passive) : base(textureName) {
			mobAI = new AIBase(this) { type = type};
		}

		public StandardMob(GameSave.WorldObjectStorage store) : base(store) {
			MovementSpeed = (float)((double)store.extraData[0]);
			mobAI = new AIBase(this) { type = (AIBase.AIType)Enum.Parse(typeof(AIBase.AIType), (string)store.extraData[1]) };
			effectList = ((JArray)store.extraData[2]).ToObject<List<EntityEffect>>();
		}

		public override void UpdatePosition() {
			lock (navigationPath) {
				if (navigationPath.Count == 0)
					return;
				float dx = navigationPath[navigationPath.Count - 1].Location.X - worldX;
				float dy = navigationPath[navigationPath.Count - 1].Location.Y - worldY;
				if (Math.Abs(dx) < MovementSpeed && Math.Abs(dy) < MovementSpeed) {
					movementQueueX = dx;
					movementQueueY = dy;
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

		public override GameSave.WorldObjectStorage StoreObject() {
			GameSave.WorldObjectStorage s = base.StoreObject();
			s.extraData = new object[] { MovementSpeed, mobAI.type.ToString(), effectList };
			s.objectType = GameSave.WorldObjectType.StandardMob;
			return s;
		}
	}
}
