using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;
using System.Drawing.Imaging;

namespace _2DRPG {
	public class TexturedObject {

		public float screenX;
		public float screenY;
		[World.Editable]
		public float width = 16;
		[World.Editable]
		public float height = 16;
		[World.Editable]
		public int layer;
		[World.Editable]
		public string texName;

		public TexturedObject(float x, float y, int layer, string textureName) {
			texName = textureName;

			SetScreenPosition(x, y, layer);

		}

		public TexturedObject(float x, float y, int layer, float width, float height, string textureName) {
			texName = textureName;
			this.height = height;
			this.width = width;

			SetScreenPosition(x, y, layer);

		}

		public TexturedObject() : this(0, 0, 10, "default") { }

		public TexturedObject(string textureName) : this(0, 0, 10, textureName) { }

		public void ContextCreated() { }

		public void ContextDestroyed() { }

		public void ContextUpdate() { }

		public float[] quadPosition = new float[] {
			0.25f, 0.25f, 0f,
			0.25f, 0.75f, 0f,
			0.75f, 0.75f, 0f,
			.75f, 0.25f, 0f
		};

		private static float[] texturePosition = new float[] {
			0.0f, 0.0f,
			0.0f, 1.0f,
			1.0f, 1.0f,
			1.0f, 0.0f
		};

		protected virtual float[] TexturePosition {
			get {
				return texturePosition;
			}
			set {
				texturePosition = value;
			}
		}

		public virtual void Render() {
			using (MemoryLock vertexArrayLock = new MemoryLock(quadPosition))
			using (MemoryLock vertexTextureLock = new MemoryLock(TexturePosition)) {
				//Sets the texture used
				Gl.BindTexture(TextureTarget.Texture2d, TextureManager.GetTextureID(texName));
				Gl.VertexPointer(3, VertexPointerType.Float, 0, vertexArrayLock.Address);	//Use the vertex array for vertex information
				Gl.TexCoordPointer(2, TexCoordPointerType.Float, 0, vertexTextureLock.Address);		//Use the texture array for texture coordinates
				Gl.DrawArrays(PrimitiveType.Quads, 0, 4);   //Draw the quad
			}
		}

		/// <summary>
		/// Sets the position of the Quad on the Screen
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public virtual void SetScreenPosition(float x, float y) {
			screenX = x;
			screenY = y;
			quadPosition[0] = x - width;
			quadPosition[3] = x - width;
			quadPosition[6] = x + width;
			quadPosition[9] = x + width;
			quadPosition[1] = y - height;
			quadPosition[10] = y - height;
			quadPosition[4] = y + height;
			quadPosition[7] = y + height;
		}
		public virtual void SetScreenPosition(float x, float y, int layer) {
			SetScreenPosition(x, y);
			SetLayer(layer);
		}
		public virtual void SetScreenDimensions(float width, float height) {
			this.width = width;
			this.height = height;
			SetScreenPosition(screenX, screenY);
		}
		/// <summary>
		/// Sets the location and dimensions of the object
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public virtual void SetScreenDimensions(float x, float y, float width, float height) {
			this.width = width;
			this.height = height;
			SetScreenPosition(x, y);
		}

		/// <summary>
		/// Moves the Screen Position by the floats passed
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void ShiftScreenPosition(float x, float y) {
			SetScreenPosition(screenX + x, screenY + y);
		}

		public void SetLayer(int layer) {
			this.layer = layer;
			quadPosition[2] = -(float)layer / 10;
			quadPosition[5] = -(float)layer / 10;
			quadPosition[8] = -(float)layer / 10;
			quadPosition[11] = -(float)layer / 10;
		}
	}
}
