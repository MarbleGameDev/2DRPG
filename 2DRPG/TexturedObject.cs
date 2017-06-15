using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;
using System.Drawing.Imaging;

namespace _2DRPG {
    class TexturedObject : IRenderable
    {
        public void ContextCreated()
        {
        }

        public void ContextDestroyed()
        {
        }

        public void ContextUpdate()
        {
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

        public void Render()
        {
            using (MemoryLock vertexArrayLock = new MemoryLock(arrayPosition))
            using (MemoryLock vertexTextureLock = new MemoryLock(texturePosition))
            using (MemoryLock vertexColorLock = new MemoryLock(arrayColor))
            {
                Gl.Enable(EnableCap.Blend);
                Gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

                //Gl.BlendFunc(BlendingFactor.ConstantColor, BlendingFactor.OneMinusConstantColor);
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
                Bitmap bmp = new Bitmap("heart.png");
                bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
                BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Gl.DrawPixels(16, 16, OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            }

        }

        public uint LoadTexture(string path)
        {

            Bitmap texSource = new Bitmap("heart.png");
            uint id = Gl.GenTexture();
            Gl.BindTexture(TextureTarget.Texture2d, id);
            Gl.TexParameter(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            Gl.TexParameter(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            Gl.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgba, texSource.Width, texSource.Height, 0, OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
            BitmapData bitmap_data = texSource.LockBits(new Rectangle(0, 0, texSource.Width, texSource.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Gl.TexSubImage2D(TextureTarget.Texture2d, 0, 0, 0, texSource.Width, texSource.Height, OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmap_data.Scan0);
            texSource.UnlockBits(bitmap_data);
            Gl.BindTexture(TextureTarget.Texture2d, 0);
            Gl.Enable(EnableCap.Texture2d);

            return id;
        }


    }

}
