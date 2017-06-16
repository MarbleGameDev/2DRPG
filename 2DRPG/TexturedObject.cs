using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;
using System.Drawing.Imaging;

namespace _2DRPG {
	class TexturedObject {

        private float x;
        private float y;
        private int layer;
        private String texName;

        public TexturedObject(float x, float y, int layer, String textureName)
        {

            this.x = x;
            this.y = y;
            this.layer = layer;
            this.texName = textureName;

            SetPosition(x,y,layer);

        }

        public TexturedObject()
        {

            x = 0;
            y = 0;
            layer = 5;
            texName = "default";
            SetPosition(x, y, layer);

        }

		public TexturedObject(string textureName) {
			x = 0;
			y = 0;
			layer = 5;
			texName = textureName;
			SetPosition(x, y, layer);
		}

        public void ContextCreated() { }

		public void ContextDestroyed() { }

		public void ContextUpdate() { }

		public float size = .25f;

		public float[] arrayPosition = new float[] {
			0.25f, 0.25f, 0f,
			0.25f, 0.75f, 0f,
			0.75f, 0.75f, 0f, 
			.75f, 0.25f, 0f

		};
		public float[] texturePosition = new float[] {
			0.0f, 0.0f,
			0f, 1.0f,
			1.0f, 1.0f,
			1.0f, 0.0f
		};

		public void Render() {
			using (MemoryLock vertexArrayLock = new MemoryLock(arrayPosition))
			using (MemoryLock vertexTextureLock = new MemoryLock(texturePosition)) {
				Gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
				//Sets the texture used
				Gl.BindTexture(TextureTarget.Texture2d, TextureManager.GetTextureID(texName));
				Gl.VertexPointer(3, VertexPointerType.Float, 0, vertexArrayLock.Address);	//Use the vertex array for vertex information
				Gl.EnableClientState(EnableCap.VertexArray);

				Gl.TexCoordPointer(2, TexCoordPointerType.Float, 0, vertexTextureLock.Address);		//Use the texture array for texture coordinates
				Gl.EnableClientState(EnableCap.TextureCoordArray);
				
				Gl.DrawArrays(PrimitiveType.Quads, 0, 4);   //Draw the quad
				Gl.BindTexture(TextureTarget.Texture2d, 0);
			}
			//System.Diagnostics.Debug.WriteLine(Gl.GetError());
		}

		/// <summary>
		/// Sets the position of the Quad
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public void SetPosition(float x, float y, int layer) {
			arrayPosition[0] = x - size;
			arrayPosition[3] = x - size;
			arrayPosition[6] = x + size;
			arrayPosition[9] = x + size;
			arrayPosition[1] = y - size;
			arrayPosition[10] = y - size;
			arrayPosition[4] = y + size;
			arrayPosition[7] = y + size;
			SetLayer(layer);
		}

		public void SetLayer(int layer) {
			arrayPosition[2] = -(float)layer / 10;
			arrayPosition[5] = -(float)layer / 10;
			arrayPosition[8] = -(float)layer / 10;
			arrayPosition[11] = -(float)layer / 10;
		}
	}
}
