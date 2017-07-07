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

		static WorldObjectInteractable inventory = new WorldObjectInteractable(180f, 58f, 16f) {
			interAction = () => { System.Diagnostics.Debug.WriteLine("Square One opened"); }
		};
		static WorldObjectAnimated flower = new WorldObjectAnimated(70f, 0f, 5, 4, 16, 16, 10, "flower") {
			size = 16f
		};

		public HashSet<WorldObjectBase> regionObjects = new HashSet<WorldObjectBase>() {
			inventory,
			new WorldObjectCollidable(-200f, -10f),
			flower
		};

		public ref HashSet<WorldObjectBase> LoadObjects() {
			inventory.LoadSavedWorldPosition("0x0", 1);
			flower.LoadSavedWorldPosition("0x0", 2);
			return ref regionObjects;
		}

		string[] textureNames = new string[] {
			"flower", "default", "heart", "josh"
		};

		public void LoadTextures() {
			SaveData.LoadRegion("0x0");
			TextureManager.RegisterTextures(textureNames);
		}
		public void UnloadTextures() {
			TextureManager.UnRegisterTextures(textureNames);
			SaveData.UnloadRegion("0x0");
		}

	}
}
