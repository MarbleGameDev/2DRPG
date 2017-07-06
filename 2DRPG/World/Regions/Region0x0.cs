using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.World.Objects;
using _2DRPG.Player;

namespace _2DRPG.World.Regions {
	/// <summary>
	/// Region for 0x0 to 99x99
	/// </summary>
	class Region0x0 : IWorldRegion {
		public int RegionX { get { return 0; } }
		public int RegionY { get { return 0; } }

		HashSet<WorldObjectBase> regionObjects = new HashSet<WorldObjectBase>();

		public HashSet<WorldObjectBase> LoadObjects() {
			regionObjects.Clear();
			WorldObjectBase j = new WorldObjectBase("default");
			WorldObjectAnimated flower = new WorldObjectAnimated(70f, 0f, 5, 4, 16, 16, 10, "flower") {
				size = 16f
			};
			regionObjects.Add(j);
            regionObjects.Add(flower);
			regionObjects.Add(new WorldObjectInteractable(180f, 58f, 16f) {
				interAction = () => { System.Diagnostics.Debug.WriteLine("Square One opened"); }
			});
			regionObjects.Add(new WorldObjectCollidable(-200f, -10f));
			//regionObjects.Add(new WorldObjectAnimated("Heart"));

			return regionObjects;
		}

		string[] textureNames = new string[] {
			"flower", "default", "heart", "josh"
		};

		public void LoadTextures() {
			TextureManager.RegisterTextures(textureNames);
		}
		public void UnloadTextures() {
			TextureManager.UnRegisterTextures(textureNames);
		}

	}
}
