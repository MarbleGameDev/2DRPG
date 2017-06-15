using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;
using System.Drawing;
using System.Drawing.Imaging;

namespace _2DRPG {
	public static class TextureManager {
		public static Dictionary<string, uint> loadedTextureIDs = new Dictionary<string, uint>();

		public static uint GetTextureID(string textureName) {
			if (loadedTextureIDs.ContainsKey(textureName))
				return loadedTextureIDs[textureName];
			return 0;
		}


		public static void LoadTexture(string path, string textureName) {
			if (!loadedTextureIDs.ContainsKey(textureName)) {
				try {
					Bitmap texSource = new Bitmap(path);    //Graps the bitmap data from the path
					texSource.RotateFlip(RotateFlipType.RotateNoneFlipY);
					uint id = Gl.GenTexture();
					Gl.BindTexture(TextureTarget.Texture2d, id);
					Gl.TexParameter(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest); //Mipmap options
					Gl.TexParameter(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
					Gl.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgba, texSource.Width, texSource.Height, 0, OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);     //Sets up the blank GL 2d Texture
					BitmapData bitmap_data = texSource.LockBits(new Rectangle(0, 0, texSource.Width, texSource.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);    //extracts the bitmap data
					Gl.TexSubImage2D(TextureTarget.Texture2d, 0, 0, 0, texSource.Width, texSource.Height, OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmap_data.Scan0);      //Adds the bitmap data to the texture
					texSource.UnlockBits(bitmap_data);
					loadedTextureIDs.Add(textureName, id);
					Gl.BindTexture(TextureTarget.Texture2d, 0);
				} catch (System.ArgumentException) {
					System.Diagnostics.Debug.WriteLine("Could not find file: " + path);
				}

			}
		}
		public static void UnloadTexture(string textureName) {
			if (loadedTextureIDs.ContainsKey(textureName))
				loadedTextureIDs.Remove(textureName);
		}

	}
}
