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
			0.0f, 0.0f,
			0.25f, 1.0f,
			0.75f, .9f,
			1.0f, 0.0f,

		};
		public float[] arrayColor = new float[] {
			1.0f, 0.0f, 0.0f,
			0.0f, 1.0f, 0.0f,
			0.0f, 0.0f, 1.0f,
			0.0f, 0.0f, 0.0f
		};
		public float[] texturePosition = new float[] {
			0.0f, 0.0f,
			0f, 1.0f,
			1.0f, 1.0f,
			1.0f, 0.0f
		};

		public void Render() {
			using (MemoryLock vertexArrayLock = new MemoryLock(arrayPosition))
			using (MemoryLock vertexTextureLock = new MemoryLock(texturePosition))
			using (MemoryLock vertexColorLock = new MemoryLock(arrayColor)) {
				Gl.Enable(EnableCap.Blend);
				Gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
				//Gl.BindTexture(TextureTarget.Texture2d, LoadTexture("josh.png"));
				//Gl.EnableClientState(EnableCap.Texture2d);
				// Note: the use of MemoryLock objects is necessary to pin vertex arrays since they can be reallocated by GC
				// at any time between the Gl.VertexPointer execution and the Gl.DrawArrays execution

				Gl.VertexPointer(2, VertexPointerType.Float, 0, vertexArrayLock.Address);
				Gl.EnableClientState(EnableCap.VertexArray);

				Gl.ColorPointer(3, ColorPointerType.Float, 0, vertexColorLock.Address);
				Gl.EnableClientState(EnableCap.ColorArray);

				Gl.TexCoordPointer(2, TexCoordPointerType.Float, 0, vertexTextureLock.Address);
				Gl.EnableClientState(EnableCap.TextureCoordArray);

				Gl.DrawArrays(PrimitiveType.Quads, 0, 4);
				uint id = Gl.GenTexture();
				Gl.BindTexture(TextureTarget.Texture2d, id);
				Bitmap bmp = new Bitmap("josh.png");
				bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
				BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
				Gl.DrawPixels(16, 16, OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte, data.Scan0);
			}

		}

		public uint LoadTexture(string path) {
			uint id = Gl.GenTexture();
			Gl.BindTexture(TextureTarget.Texture2d, id);
			Bitmap bmp = new Bitmap(path);
			BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			Gl.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgba, data.Width, data.Height, 0, OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

			bmp.UnlockBits(data);

			Gl.TexParameter(TextureTarget.Texture2d, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
			Gl.TexParameter(TextureTarget.Texture2d, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
			Gl.TexParameter(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			Gl.TexParameter(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

			return id;
		}
	}
}
