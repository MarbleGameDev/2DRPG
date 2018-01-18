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

		public UIBase() : base() {
			SetScreenPosition(screenX, screenY, defaultLayer);
		}
		public UIBase(Texture textureName) : base(textureName) {
			SetScreenPosition(screenX, screenY, defaultLayer);
		}
		/// <summary>
		/// Complete Declaration for UIBase
		/// </summary>
		/// <param name="x">X position on the screen</param>
		/// <param name="y">Y position on the screen</param>
		/// <param name="width">Distance to the left and right</param>
		/// <param name="height">Distance to the top and bottom</param>
		/// <param name="layer">Render layer</param>
		/// <param name="textureName">Name of the texture</param>
		public UIBase(float x, float y, float width, float height, int layer, Texture textureName) : base(x, y, layer, width, height, textureName) {

		}
		/// <summary>
		/// Sets the position of the object on the screen
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public override void SetScreenPosition(float x, float y) {
			screenX = x;
			screenY = y;
			quadPosition[0] = (screenX - width);
			quadPosition[3] = (screenX - width);
			quadPosition[6] = (screenX + width);
			quadPosition[9] = (screenX + width);
			quadPosition[1] = (screenY - height);
			quadPosition[10] = (screenY - height);
			quadPosition[4] = (screenY + height);
			quadPosition[7] = (screenY + height);
		}
		public override void SetScreenPosition(float x, float y, int layer) {
			SetScreenPosition(x, y);
			SetLayer(layer);
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
					tempQuad[0] = (col == 0) ? (screenX - width) : (col == 1) ? (screenX - width + NineSliceBoarder) : (screenX + width - NineSliceBoarder);
					tempQuad[3] = tempQuad[0];
					tempQuad[6] = (col == 0) ? (screenX - width + NineSliceBoarder) : (col == 1) ? (screenX + width - NineSliceBoarder) : (screenX + width);
					tempQuad[9] = tempQuad[6];
					tempQuad[1] = (row == 0) ? (screenY - height) : (row == 1) ? (screenY - height + NineSliceBoarder) : (screenY + height - NineSliceBoarder);
					tempQuad[10] = tempQuad[1];
					tempQuad[4] = (row == 0) ? (screenY - height + NineSliceBoarder) : (row == 1) ? (screenY + height - NineSliceBoarder) : (screenY + height);
					tempQuad[7] = tempQuad[4];

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
