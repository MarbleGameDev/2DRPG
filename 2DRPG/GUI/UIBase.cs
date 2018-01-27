using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI {
	class UIBase : TexturedObject {
		protected int defaultLayer = 2;
		public bool Visible = true;
		public bool NineSliceRendering = true;
		public int NineSliceBoarder = 5;
		public int TextureSize = 15;

		/// <summary>
		/// Complete Declaration for UIBase
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="layer"></param>
		/// <param name="textureName"></param>
		/// <param name="width1"></param>
		/// <param name="height1"></param>
		/// <param name="width2"></param>
		/// <param name="height2"></param>
		/// <param name="width3"></param>
		/// <param name="height3"></param>
		/// <param name="width4"></param>
		/// <param name="height4"></param>
		public UIBase(float x, float y, int layer, Texture textureName, float width1 = 16, float height1 = 16, float width2 = 16, float height2 = 16, float width3 = 16, float height3 = 16, float width4 = 16, float height4 = 16) : base(x, y, layer, textureName, width1, height1, width2, height2, width3, height3, width4, height4) {

		}

		public UIBase(float x, float y, float width, float height, int layer, Texture textureName) : this(x, y, layer, textureName, width, height, width, height, width, height, width, height) {

		}

		public UIBase() : base() {
			SetScreenPosition(screenX, screenY, defaultLayer);
		}
		public UIBase(Texture textureName) : base(textureName) {
			SetScreenPosition(screenX, screenY, defaultLayer);
		}

		public override void Render() {
			if (!Visible)
				return;
			if (NineSliceRendering) {
				float[] tempTex = new float[8];
				float[] tempQuad = new float[12];
				tempQuad[2] = quadPosition[2];
				tempQuad[5] = quadPosition[5];
				tempQuad[8] = quadPosition[8];
				tempQuad[11] = quadPosition[11];

				float boarder = NineSliceBoarder / (float)TextureSize;

				Gl.BindTexture(TextureTarget.Texture2d, texName.glID);
				//Iterate clockwise from bottom left corner as 0
				for (int i = 0; i < 9; i++) {
					int row = i % 3;
					int col = i / 3;
					tempQuad[0] = (col == 0) ? (screenX - width3) : (col == 1) ? (screenX - width3 + NineSliceBoarder) : (screenX + width4 - NineSliceBoarder);
					tempQuad[3] = (col == 0) ? (screenX - width2) : (col == 1) ? (screenX - width2 + NineSliceBoarder) : (screenX + width1 - NineSliceBoarder);
					tempQuad[6] = (col == 0) ? (screenX - width2 + NineSliceBoarder) : (col == 1) ? (screenX + width1 - NineSliceBoarder) : (screenX + width1);
					tempQuad[9] = (col == 0) ? (screenX - width3 + NineSliceBoarder) : (col == 1) ? (screenX + width4 - NineSliceBoarder) : (screenX + width4);
					tempQuad[1] = (row == 0) ? (screenY - height3) : (row == 1) ? (screenY - height3 + NineSliceBoarder) : (screenY + height2 - NineSliceBoarder);
					tempQuad[10] = (row == 0) ? (screenY - height4) : (row == 1) ? (screenY - height4 + NineSliceBoarder) : (screenY + height1 - NineSliceBoarder);
					tempQuad[4] = (row == 0) ? (screenY - height3 + NineSliceBoarder) : (row == 1) ? (screenY + height2 - NineSliceBoarder) : (screenY + height2);
					tempQuad[7] = (row == 0) ? (screenY - height4 + NineSliceBoarder) : (row == 1) ? (screenY + height1 - NineSliceBoarder) : (screenY + height1);

					tempTex[0] = (col == 0) ? (0f) : (col == 1) ? (boarder) : (1f - boarder);
					tempTex[2] = tempTex[0];
					tempTex[4] = (col == 0) ? (boarder) : (col == 1) ? (1f - boarder) : (1f);
					tempTex[6] = tempTex[4];
					tempTex[1] = (row == 0) ? (0f) : (row == 1) ? (boarder) : (1f - boarder);
					tempTex[7] = tempTex[1];
					tempTex[3] = (row == 0) ? (boarder) : (row == 1) ? (1f - boarder) : (1f);
					tempTex[5] = tempTex[3];
					using (MemoryLock vertexArrayLock = new MemoryLock(tempQuad))
					using (MemoryLock vertexTextureLock = new MemoryLock(tempTex)) {
						Gl.VertexPointer(3, VertexPointerType.Float, 0, vertexArrayLock.Address);   //Use the vertex array for vertex information
						Gl.TexCoordPointer(2, TexCoordPointerType.Float, 0, vertexTextureLock.Address);     //Use the texture array for texture coordinates
						Gl.DrawArrays(PrimitiveType.Quads, 0, 4);   //Draw the quad
					}
				}
			} else {
				base.Render();
			}
		}

		public virtual void Cleanup() {

		}
		public virtual void Setup() {

		}
	}
}
