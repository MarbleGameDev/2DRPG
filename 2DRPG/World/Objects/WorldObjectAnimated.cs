using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using _2DRPG.Save;

namespace _2DRPG.World.Objects {
	[Serializable]
	class WorldObjectAnimated : WorldObjectBase {
		[Editable]
        public int spritesAmount = 1;
		[Editable]
        public int spriteWidth = 16;
		[Editable]
        public int spriteHeight = 16;
		[Editable]
        public int frameInterval = 10;
		float sheetShiftHorizontal = 1f;
		float sheetShiftVertical = 1f;

		/// <summary>
		/// Complete Declaration for WorldObjectAnimated
		/// </summary>
		/// <param name="x">X position in the world</param>
		/// <param name="y">Y position in the world</param>
		/// <param name="layer">Render Layer</param>
		/// <param name="spritesAmt">Number of sprites on the spriteSheet</param>
		/// <param name="spriteWth">Width of each sprite</param>
		/// <param name="spriteHt">Height of each sprite</param>
		/// <param name="frameInt">Number of render calls between frame changes</param>
		/// <param name="textureName">Name of the texture</param>
        public WorldObjectAnimated(float x, float y, int layer, int spritesAmt, int spriteWth, int spriteHt, int frameInt, Texture textureName, float width = 16, float height = 16) : base(x, y, textureName, width, height) {
			SetAnimationSettings(spritesAmt, spriteWth, spriteHt, frameInt);
			SetLayer(layer);
        }

		private float[] texturePosition = new float[] {
			0.0f, 0.0f,
			0.0f, 1.0f,
			1.0f, 1.0f,
			1.0f, 0.0f
		};

		protected override float[] TexturePosition {
			get {
				return texturePosition;
			}
			set {
				texturePosition = value;
			}
		}

		public void SetAnimationSettings(int spritesAmt, int spriteWth, int spriteHt, int frameInt) {
			spritesAmount = spritesAmt;
			spriteWidth = spriteWth;
			spriteHeight = spriteHt;
			frameInterval = frameInt;
			sheetShiftHorizontal = ((spritesAmount > 10) ? .1f : (1f / spritesAmount));
			sheetShiftVertical = 1f / ((spritesAmount / 10) + 1);
		}

		int frameCount = 0;
        public void SpriteUpdate() {
			if (frameInterval == 0)
				return;
			if (frameCount % frameInterval == 0) {
				int frameNum = frameCount / frameInterval;
				TexturePosition[0] = 0 + sheetShiftHorizontal * (frameNum % 10);
				TexturePosition[2] = 0 + sheetShiftHorizontal * (frameNum % 10);
				TexturePosition[4] = sheetShiftHorizontal + sheetShiftHorizontal * (frameNum % 10);
				TexturePosition[6] = sheetShiftHorizontal + sheetShiftHorizontal * (frameNum % 10);
				TexturePosition[1] = 0 + sheetShiftVertical * (frameNum / 10);
				TexturePosition[3] = sheetShiftVertical + sheetShiftVertical * (frameNum / 10);
				TexturePosition[5] = sheetShiftVertical + sheetShiftVertical * (frameNum / 10);
				TexturePosition[7] = 0 + sheetShiftVertical * (frameNum / 10);
			}
			frameCount++;
			if (frameCount / frameInterval == spritesAmount)
				frameCount = 0;
			return;
        }

		public override void ModificationAction() {
			base.ModificationAction();
			sheetShiftHorizontal = ((spritesAmount > 10) ? .1f : (1f / spritesAmount));
			sheetShiftVertical = 1f / ((spritesAmount / 10) + 1);
		}
	}
}
