using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;
using System.Drawing.Imaging;
using System.Runtime.Serialization;

namespace _2DRPG {
	[Serializable]
	public class TexturedObject {

		public float screenX;
		public float screenY;

		public bool automaticTextureMapping = true;

		//Widths and Heights are numbered based on cartesian quadrants (counterclockwise starting in right top corner)
		[World.Editable]
		public float width1 = 16;
		[World.Editable]
		public float width2 = 16;
		[World.Editable]
		public float width3 = 16;
		[World.Editable]
		public float width4 = 16;
		[World.Editable]
		public float height1 = 16;
		[World.Editable]
		public float height2 = 16;
		[World.Editable]
		public float height3 = 16;
		[World.Editable]
		public float height4 = 16;
		[World.Editable]
		public int layer;
		private float angle = 0;

		[World.Editable]
		public float Rotation {		//Rotation angle in degrees
			get {
				return angle;
			}
			set {
				Rotate(value % 360);
				angle = value % 360;
			}
		}

		[NonSerialized]
		public Texture texName = TextureManager.TextureNames.DEFAULT;
		private string storeTex;

		public Guid uid;

		public TexturedObject(float x, float y, int layer, Texture textureName) {
			texName = textureName;
			SetScreenPosition(x, y, layer);
			uid = Guid.NewGuid();
		}

		public TexturedObject(float x, float y, int layer, Texture textureName, float width1, float height1, float width2, float height2, float width3, float height3, float width4, float height4) {
			texName = textureName;
			this.width1 = width1;
			this.height1 = height1;
			this.width2 = width2;
			this.height2 = height2;
			this.width3 = width3;
			this.height3 = height3;
			this.width4 = width4;
			this.height4 = height4;
			SetScreenPosition(x, y, layer);
			uid = Guid.NewGuid();
		}

		public TexturedObject() : this(0, 0, 10, TextureManager.TextureNames.DEFAULT) { }

		public TexturedObject(Texture textureName) : this(0, 0, 10, textureName) { }

		public float[] quadPosition = new float[] {
			0.25f, 0.25f, 0f,
			0.25f, 0.75f, 0f,
			0.75f, 0.75f, 0f,
			.75f, 0.25f, 0f
		};
		protected float[] texturePosition = new float[] {
			0.0f, 0.0f,
			0.0f, 1.0f,
			1.0f, 1.0f,
			1.0f, 0.0f
		};

		public virtual void Render() {
			using (MemoryLock vertexArrayLock = new MemoryLock(quadPosition))
			using (MemoryLock vertexTextureLock = new MemoryLock(texturePosition)) {
				//Sets the texture used
				Gl.PushMatrix();
				Gl.BindTexture(TextureTarget.Texture2d, texName.glID);
				Gl.VertexPointer(3, VertexPointerType.Float, 0, vertexArrayLock.Address);	//Use the vertex array for vertex information
				Gl.TexCoordPointer(2, TexCoordPointerType.Float, 0, vertexTextureLock.Address);     //Use the texture array for texture coordinates
				Gl.DrawArrays(PrimitiveType.Quads, 0, 4);   //Draw the quad
				Gl.PopMatrix();
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
			UpdateQuadPosition();
		}

		public virtual void SetScreenPosition(float x, float y, float angle) {
			screenX = x;
			screenY = y;
			Rotate(angle);
		}

		protected virtual void UpdateQuad() {
			quadPosition[0] = screenX - width3;
			quadPosition[1] = screenY - height3;
			quadPosition[3] = screenX - width2;
			quadPosition[4] = screenY + height2;
			quadPosition[6] = screenX + width1;
			quadPosition[7] = screenY + height1;
			quadPosition[9] = screenX + width4;
			quadPosition[10] = screenY - height4;
			if (automaticTextureMapping)
				ResetTexturePosition();
		}

		public virtual void UpdateQuadPosition() {
			if (angle <= -.001f || angle >= .001f) {
				UpdateQuad();
				Rotate(0);
			} else {
				UpdateQuad();
			}
		}

		/// <summary>
		/// Rotates the current quadrilateral by the angle in degrees
		/// </summary>
		/// <param name="angle"></param>
		protected virtual void Rotate(float angle) {
			//UpdateQuad();
			float anglRad = angle * (float)(Math.PI / 180);
			//Working counterclockwise through quadrants
			float currentang = (float)Math.Atan(height1/ width1) + anglRad;    //Fundamental angle of the vertex plus the current rotation
			float hyp = (float)Math.Sqrt(height1 * height1 + width1 * width1);
			quadPosition[6] = screenX +  hyp * (float)Math.Cos(currentang);
			quadPosition[7] = screenY + hyp * (float)Math.Sin(currentang);

			currentang = (float)Math.Atan(height2 / -width2) + (float)Math.PI + anglRad;
			hyp = (float)Math.Sqrt(height2 * height2 + width2 * width2);
			quadPosition[3] = screenX + hyp * (float)Math.Cos(currentang);
			quadPosition[4] = screenY + hyp * (float)Math.Sin(currentang);

			currentang = (float)Math.Atan(height3 / width3) + (float)Math.PI + anglRad;
			hyp = (float)Math.Sqrt(height3 * height3 + width3 * width3);
			quadPosition[0] = screenX + hyp * (float)Math.Cos(currentang);
			quadPosition[1] = screenY + hyp * (float)Math.Sin(currentang);

			currentang = (float)Math.Atan(-height4 / width4) + anglRad;
			hyp = (float)Math.Sqrt(height4 * height4 + width4 * width4);
			quadPosition[9] = screenX + hyp * (float)Math.Cos(currentang);
			quadPosition[10] = screenY + hyp * (float)Math.Sin(currentang);
		}

		protected virtual void ResetTexturePosition() {
			float[] sizes = GetObjectBounds();

			float totalW = sizes[0] - sizes[1];
			float totalH = sizes[2] - sizes[3];
			int counter = 0;
			for (int i = 0; i < 12; i++) {
				if ((i - 2) % 3 == 0)	//skip over z values
					continue;
				if ((i - 1) % 3 == 0) {     //y values
					texturePosition[counter++] = (quadPosition[i] - sizes[3]) / totalH;
				} else {                    //x values
					texturePosition[counter++] = (quadPosition[i] - sizes[1]) / totalW;
				}
			}
		}

		/// <summary>
		/// Returns the farthest bounds of the object width and height
		/// </summary>
		/// <returns>[maxWidth, minWidth, maxHeight, minHeight]</returns>
		protected virtual float[] GetObjectBounds() {
			float[] sizes = new float[] { float.MinValue, float.MaxValue, float.MinValue, float.MaxValue };     //maxWidth, minWidth, maxHeight, minHeight
			for (int i = 0; i <= 9; i += 3) {
				if (quadPosition[i] > sizes[0])
					sizes[0] = quadPosition[i];
				else if (quadPosition[i] < sizes[1])
					sizes[1] = quadPosition[i];
			}
			for (int i = 1; i <= 10; i += 3) {
				if (quadPosition[i] > sizes[2])
					sizes[2] = quadPosition[i];
				else if (quadPosition[i] < sizes[3])
					sizes[3] = quadPosition[i];
			}
			return sizes;
		}

		public virtual void SetScreenPosition(float x, float y, int layer) {
			SetScreenPosition(x, y);
			SetLayer(layer);
		}
		public virtual void SetScreenDimensions(float[] dim1, float[] dim2, float[] dim3, float[] dim4) {
			width1 = dim1[0];
			height1 = dim1[1];
			width2 = dim2[0];
			height2 = dim2[1];
			width3 = dim3[0];
			height3 = dim3[1];
			width4 = dim4[0];
			height4 = dim4[1];
			UpdateQuadPosition();
		}
		/// <summary>
		/// Sets the location and dimensions of the object
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public virtual void SetScreenDimensions(float x, float y, float[] dim1, float[] dim2, float[] dim3, float[] dim4) {
			screenX = x;
			screenY = y;
			SetScreenDimensions(dim1, dim2, dim3, dim4);
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

		[OnSerializing]
		private void StoreTexture(StreamingContext sc) {
			storeTex = texName.name;
		}
		[OnDeserialized]
		private void LoadTexture(StreamingContext sc) {
			System.Diagnostics.Debug.WriteLine("three");
			texName = TextureManager.RetrieveTexture(storeTex);
		}
	}
}
