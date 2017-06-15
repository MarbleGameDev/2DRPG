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
		}

		public void ContextDestroyed() {
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
				LoadTexture("josh.png");	//Sets the texture used as josh.png
				Gl.VertexPointer(2, VertexPointerType.Float, 0, vertexArrayLock.Address);	//Use the vertex array for vertex information
				Gl.EnableClientState(EnableCap.VertexArray);

				Gl.TexCoordPointer(2, TexCoordPointerType.Float, 0, vertexTextureLock.Address);		//Use the texture array for texture coordinates
				Gl.EnableClientState(EnableCap.TextureCoordArray);
				
				Gl.DrawArrays(PrimitiveType.Quads, 0, 4);	//Draw the quad
			}

		}
		//Sets the texture located at the path specified as the active bound texture
		public void LoadTexture(string path) {
			Gl.Enable(EnableCap.Texture2d);
			Bitmap texSource = new Bitmap(path);	//Graps the bitmap data from the path
			texSource.RotateFlip(RotateFlipType.RotateNoneFlipY);
			uint id = Gl.GenTexture();
			Gl.BindTexture(TextureTarget.Texture2d, id);
			Gl.TexParameter(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter , (int)TextureMagFilter.Nearest);	//Mipmap options
			Gl.TexParameter(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
			Gl.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgba, texSource.Width, texSource.Height, 0, OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);		//Sets up the blank GL 2d Texture
			BitmapData bitmap_data = texSource.LockBits(new Rectangle(0, 0, texSource.Width, texSource.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);    //extracts the bitmap data
			Gl.TexSubImage2D(TextureTarget.Texture2d, 0, 0, 0, texSource.Width, texSource.Height, OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmap_data.Scan0);		//Adds the bitmap data to the texture
			texSource.UnlockBits(bitmap_data);
		}
	}
}
