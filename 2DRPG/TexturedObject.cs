using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;
using System.Drawing.Imaging;

namespace _2DRPG {
	class TexturedObject : IRenderable {
		public void ContextCreated() {
			TextureManager.LoadTexture("Sprites/heart.png", "heart");	//Loads the png from the sprites folder and registers it as a texture named 'heart'
		}

		public void ContextDestroyed() {
			TextureManager.UnloadTexture("heart");
		}

		public void ContextUpdate() {

		}

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
				Gl.BindTexture(TextureTarget.Texture2d, TextureManager.GetTextureID("heart"));
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
		/// Shifts the quad by the values given in x,y,z
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public void MoveRelative(float x = 0, float y = 0, float z = 0) {
			for (int i = 0; i < 4; i++) {
				arrayPosition[i * 3] += x;
				arrayPosition[i * 3 + 1] += y;
				arrayPosition[i * 3 + 2] += z;
			}
		}

		/// <summary>
		/// Sets the position of the Quad
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public void MoveAbsolute(float x, float y, float z) {
			arrayPosition[0] = x - size;
			arrayPosition[3] = x - size;
			arrayPosition[6] = x + size;
			arrayPosition[9] = x + size;
			arrayPosition[1] = y - size;
			arrayPosition[10] = y - size;
			arrayPosition[4] = y + size;
			arrayPosition[7] = y + size;
			arrayPosition[2] = z;
			arrayPosition[5] = z;
			arrayPosition[8] = z;
			arrayPosition[11] = z;
		}
	}
}
