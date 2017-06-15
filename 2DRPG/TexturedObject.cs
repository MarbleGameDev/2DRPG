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
			TextureManager.LoadTexture("Sprites/heart.png", "heart");
		}

		public void ContextDestroyed() {
			TextureManager.UnloadTexture("heart");
		}

		public void ContextUpdate() {
		}

		public float[] arrayPosition = new float[] {
			0.25f, 0.0f,
			0.25f, 1.0f,
			0.75f, 1f,
			.75f, 0.0f,

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
				Gl.Enable(EnableCap.Blend);
				Gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
				//Sets the texture used
				Gl.BindTexture(TextureTarget.Texture2d, TextureManager.GetTextureID("heart"));
				Gl.VertexPointer(2, VertexPointerType.Float, 0, vertexArrayLock.Address);	//Use the vertex array for vertex information
				Gl.EnableClientState(EnableCap.VertexArray);

				Gl.TexCoordPointer(2, TexCoordPointerType.Float, 0, vertexTextureLock.Address);		//Use the texture array for texture coordinates
				Gl.EnableClientState(EnableCap.TextureCoordArray);
				
				Gl.DrawArrays(PrimitiveType.Quads, 0, 4);   //Draw the quad
				Gl.BindTexture(TextureTarget.Texture2d, 0);
			}

		}
	}
}
