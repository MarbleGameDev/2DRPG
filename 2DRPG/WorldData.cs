using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;
using System.Drawing.Imaging;

namespace _2DRPG {
	public static class WorldData {
		public static List<object> currentObjects = new List<object>(); //TODO: replace with a more accurate object type
		/// <summary>
		/// Loads the objects and their data into the currentObjects list
		/// </summary>
		public static void LoadCurrentObjects() {
			ControllableObject t = new ControllableObject("heart");
			TexturedObject j = new TexturedObject();
			t.MoveRelative(0.2f, 0.1f);
			t.SetLayer(2);
			//t.MoveAbsolute(0f, 0, 0);
			currentObjects.Add(t);
			currentObjects.Add(j);

			Entities.StandardMob mob = new Entities.StandardMob("josh", 15);
			mob.ReceiveAttack(new EffectAttack(new Entities.EntityEffect(() => { System.Diagnostics.Debug.WriteLine("Test Effect"); }, 5)));	//Attack the mob with an attack that deals an effect for 5 seconds that just outputs "Test Effect"
			currentObjects.Add(mob);
		}

		/// <summary>
		/// Loads all textures needed for the world objects to be added afterwards, This function is called once GL context is created, do not invoke manually
		/// </summary>
		public static void LoadCurrentTextures() {
			TextureManager.ClearTextures();
			TextureManager.LoadTexture("Sprites/Default.png", "default");
			TextureManager.LoadTexture("Sprites/Heart.png", "heart");
			TextureManager.LoadTexture("Sprites/josh.png", "josh");


			LoadCurrentObjects();
		}

	}
}
