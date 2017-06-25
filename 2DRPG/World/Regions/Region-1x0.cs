using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.World.Objects;

namespace _2DRPG.World.Regions {
	class Region_1x0 : IWorldRegion {
		public int RegionX { get { return -1; } }
		public int RegionY { get { return 0; } }

		List<WorldObjectBase> regionObjects = new List<WorldObjectBase>();

		public List<WorldObjectBase> LoadObjects() {

			Entities.StandardMob mob = new Entities.StandardMob(99, 1, "josh", 15);
			//mob.ReceiveAttack(new EffectAttack(new Entities.EntityEffect(() => { System.Diagnostics.Debug.WriteLine("Test Effect"); }, 1)));    //Attack the mob with an attack that deals an effect for 5 seconds that just outputs "Test Effect"
			//regionObjects.Add(mob);

			return regionObjects;
		}

		public void LoadTextures() {
			TextureManager.LoadTexture("Sprites/Default.png", "default");
			TextureManager.LoadTexture("Sprites/josh.png", "josh");
		}

		public void UnloadTextures() {
			TextureManager.UnloadTexture("default");
			TextureManager.UnloadTexture("josh");
		}
	}
}
