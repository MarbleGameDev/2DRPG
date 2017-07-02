using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Specialized;
using System.Collections;

namespace _2DRPG {
	public static class TextureManager {

		private static Dictionary<string, uint> loadedTextureIDs = new Dictionary<string, uint>();

		private static HybridDictionary texturePaths = new HybridDictionary() {
			{"flower", "Sprites/SpriteSheets/Flowers.png" },
			{"default", "Sprites/Default.png" },
			{"heart", "Sprites/Heart.png" },
			{"baseFont", "Sprites/SpriteSheets/BaseFont.png" },
			{"button", "Sprites/Button.png" },
			{"josh", "Sprites/josh.png" }
		};

		private static Dictionary<string, int> textureUses = new Dictionary<string, int>();

		static TextureManager() {
			foreach (string s in texturePaths.Keys) {
				textureUses.Add(s, 0);
			}
		}
		/// <summary>
		/// Returns the Texture ID for the given textureName as set up earlier by LoadTexture
		/// </summary>
		/// <param name="textureName"></param>
		/// <returns></returns>
		public static uint GetTextureID(string textureName) {
			if (loadedTextureIDs.ContainsKey(textureName))
				return loadedTextureIDs[textureName];
			return 0;
		}

		/// <summary>
		/// Loads a texture from the file path specified into the manager for access via the textureName given
		/// </summary>
		/// <param name="path"></param>
		/// <param name="textureName"></param>
		private static void LoadTexture(string textureName) {
				try {
					Bitmap texSource = new Bitmap((string)texturePaths[textureName]);    //Graps the bitmap data from the path
					texSource.RotateFlip(RotateFlipType.RotateNoneFlipY);
					uint id = Gl.GenTexture();
					Gl.BindTexture(TextureTarget.Texture2d, id);
					Gl.TexStorage2D(TextureTarget.Texture2d, 12, InternalFormat.Rgba, texSource.Width, texSource.Height);
					Gl.TexParameter(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest); //Mipmap options
					Gl.TexParameter(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
					Gl.TexParameter(TextureTarget.Texture2d, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
					Gl.TexParameter(TextureTarget.Texture2d, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
					Gl.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgba, texSource.Width, texSource.Height, 0, OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);     //Sets up the blank GL 2d Texture
					BitmapData bitmap_data = texSource.LockBits(new Rectangle(0, 0, texSource.Width, texSource.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);    //extracts the bitmap data
					Gl.TexSubImage2D(TextureTarget.Texture2d, 0, 0, 0, texSource.Width, texSource.Height, OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmap_data.Scan0);      //Adds the bitmap data to the texture
					Gl.GenerateMipmap(TextureTarget.Texture2d);
					texSource.UnlockBits(bitmap_data);
					loadedTextureIDs.Add(textureName, id);
					Gl.BindTexture(TextureTarget.Texture2d, 0);
				} catch (Exception) {
					System.Diagnostics.Debug.WriteLine("Could not find file: " + textureName);
				}
			//System.Diagnostics.Debug.WriteLine("Loaded Texture: " + textureName);
        }

		public static void RegisterTextures(string[] textureNames) {
			foreach (string s in textureNames) {
				if (texturePaths.Contains(s) && textureUses.ContainsKey(s)) {
					textureUses[s]++;
					if (!loadedTextureIDs.ContainsKey(s))
						LoadTexture(s);
				}
			}
		}

		public static void UnRegisterTextures(string[] textureNames) {
			foreach (string s in textureNames) {
				if (texturePaths.Contains(s) && textureUses.ContainsKey(s)) {
					textureUses[s]--;
					if (textureUses[s] <= 0) {
						if (loadedTextureIDs.ContainsKey(s)) {
							Gl.DeleteTextures(loadedTextureIDs[s]);
							loadedTextureIDs.Remove(s);
						}
						textureUses[s] = 0;
					}
				}
			}
		}

		public static void ClearTextures() {
			loadedTextureIDs.Clear();
		}

	}
}
