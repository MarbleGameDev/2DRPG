using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.Entities;

namespace _2DRPG.Player {
	[Serializable]
	class MCObject : World.Objects.WorldObjectControllable, IDamagable, IEffectable {

		public PlayerData Data = new PlayerData();

		public MCObject() : base(0, 0, 10, TextureManager.TextureNames.character, 15, 33, 15, 33, 15, 33, 15, 33) {
			MovementSpeed = 2.5f ;
			//float[] size = { 15, 33 };
			//SetScreenDimensions(size, size, size, size);
		}

		public void Teleport(float x, float y) {
			WorldData.SetCenter(x, y);
			SetWorldPosition(x, y);
		}

		/// <summary>
		/// Moves the character relatively based on passed parameters
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public override void MoveRelative(float x = 0, float y = 0) {
			WorldData.MoveCenter(x, y);
			ShiftPosition(quadPosition, x, y);
			worldX += x;
			worldY += y;
			if (Save.SaveData.GameSettings.coOp == true)
				Net.SessionManager.SendFrame(new Net.UDPFrame() {
					subject = Net.UDPFrame.FrameType.Player,
					intData = new int[] { 1 },
					floatData = new float[] { worldX, worldY }
				});
		}

		public void AddEffect(EntityEffect e) {
			throw new NotImplementedException();
		}

		public void EffectTick() {

		}

		public void ReceiveAttack(Attack a) {

		}
	}
}
