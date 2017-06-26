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

		List<WorldObjectBase> regionObjects = new List<WorldObjectBase>();

		public List<WorldObjectBase> LoadObjects() {
			regionObjects.Clear();
			WorldObjectBase j = new WorldObjectBase();
			WorldObjectAnimated flower = new WorldObjectAnimated(-1f,.5f,1,4,16,16,5, "flower");
			regionObjects.Add(j);
            regionObjects.Add(flower);
			//regionObjects.Add(new WorldObjectAnimated("Heart"));

			return regionObjects;
		}

		public void LoadTextures() {
            TextureManager.LoadTexture("Sprites/SpriteSheets/Flowers.png","flower");
			TextureManager.LoadTexture("Sprites/Default.png", "default");
			TextureManager.LoadTexture("Sprites/Heart.png", "heart");
			TextureManager.LoadTexture("Sprites/josh.png", "josh");
		}
		public void UnloadTextures() {
			TextureManager.UnloadTexture("default");
			TextureManager.UnloadTexture("heart");
			TextureManager.UnloadTexture("josh");
		}

	}
}
