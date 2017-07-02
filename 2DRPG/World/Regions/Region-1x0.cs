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

		HashSet<WorldObjectBase> regionObjects = new HashSet<WorldObjectBase>();

		public HashSet<WorldObjectBase> LoadObjects() {

			Entities.StandardMob mob = new Entities.StandardMob(990, 100, "josh", 15);
			//mob.ReceiveAttack(new EffectAttack(new Entities.EntityEffect(() => { System.Diagnostics.Debug.WriteLine("Test Effect"); }, 1)));    //Attack the mob with an attack that deals an effect for 5 seconds that just outputs "Test Effect"
			regionObjects.Add(mob);
			regionObjects.Add(new WorldObjectBase(900, 0, "default"));
			regionObjects.Add(new WorldObjectBase(900, 25f, "default"));

			return regionObjects;
		}

		string[] textureNames = new string[] {
			"default", "josh"
		};

		public void LoadTextures() {
			TextureManager.RegisterTextures(textureNames);
		}

		public void UnloadTextures() {
			TextureManager.UnRegisterTextures(textureNames);
		}
	}
}
