﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Specialized;
using System.Collections;
using System.Reflection;

namespace _2DRPG {
	public static class TextureManager {

		public static class TextureNames {
			public static readonly Texture
				flower = new Texture("flower", "SpriteSheets/Flowers.png"),
				DEFAULT = new Texture("default", "Default.png"),
				heart = new Texture("heart", "Heart.png"),
				character = new Texture("character", "SpriteSheets/MainCharacter.png"),
				tempCharacter = new Texture("tempCharacter", "SpriteSheets/Character.png"),
				baseFont = new Texture("baseFont", "SpriteSheets/BaseFont.png"),
				button = new Texture("button", "Button.png"),
				lightBack = new Texture("lightBack", "LightBackground.png"),
				darkBack = new Texture("darkBack", "DarkBackground.png"),
				textBox = new Texture("textBox", "TextBox.png"),
				exit = new Texture("exit", "Exit.png"),
				none = new Texture("none", "None.png"),
				selected = new Texture("selected", "Selected.png")
		;
		}

		private static Dictionary<string, Texture> textureLookup = new Dictionary<string, Texture>();

		static TextureManager() {
			textureLookup = typeof(TextureNames).GetFields(BindingFlags.Public | BindingFlags.Static).ToDictionary(f => ((Texture)f.GetValue(null)).name, f => (Texture)f.GetValue(null));
		}

		public static Texture RetrieveTexture(string textureName) {
			if (textureLookup != null && textureLookup.ContainsKey(textureName))
				return textureLookup[textureName];
			return TextureNames.DEFAULT;
		}

		private static string spriteLocation = "../../Sprites/";

		/// <summary>
		/// Loads a texture from the file path specified into the manager for access via the textureName given
		/// </summary>
		/// <param name="textureName">Name of the texture as registed in TextureManager</param>
		private static void LoadTexture(Texture textureName) {
			try {
				Bitmap texSource = new Bitmap(spriteLocation + textureName.path);    //Graps the bitmap data from the path
				texSource.RotateFlip(RotateFlipType.RotateNoneFlipY);
				uint id = Gl.GenTexture();
				Gl.BindTexture(TextureTarget.Texture2d, id);
				Gl.TexStorage2D(TextureTarget.Texture2d, 12, InternalFormat.Rgba, texSource.Width, texSource.Height);
				Gl.TexParameter(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest); //Mipmap options
				Gl.TexParameter(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
				Gl.TexParameter(TextureTarget.Texture2d, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
				Gl.TexParameter(TextureTarget.Texture2d, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
				Gl.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgba, texSource.Width, texSource.Height, 0, OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);     //Sets up the blank GL 2d Texture
				BitmapData bitmap_data = texSource.LockBits(new Rectangle(0, 0, texSource.Width, texSource.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);    //extracts the bitmap data
				Gl.TexSubImage2D(TextureTarget.Texture2d, 0, 0, 0, texSource.Width, texSource.Height, OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmap_data.Scan0);      //Adds the bitmap data to the texture
				Gl.GenerateMipmap(TextureTarget.Texture2d);
				texSource.UnlockBits(bitmap_data);
				textureName.glID = id;
				Gl.BindTexture(TextureTarget.Texture2d, 0);
			} catch (Exception) {
				System.Diagnostics.Debug.WriteLine("Could not find file: " + textureName);
			}
        }

		/// <summary>
		/// Registers all textures in the array if necessary
		/// </summary>
		/// <param name="textureNames"></param>
		public static void RegisterTextures(Texture[] textures) {
			if (textures.Length == 0)
				return;
			foreach (Texture s in textures) {
				s.uses++;
				if (s.glID == 0)
					LoadTexture(s);
			}
		}
		/// <summary>
		/// Unregisters all textures in the array if necesary
		/// </summary>
		/// <param name="textureNames"></param>
		public static void UnRegisterTextures(Texture[] textures) {
			if (textures.Length == 0)
				return;
			foreach (Texture s in textures) {
				if (s.uses == 1) {
					Gl.DeleteTextures(s.glID);
					s.glID = 0;
				}
				s.uses--;
			}
		}

	}
}
