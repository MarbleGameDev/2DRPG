using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenGL;

namespace _2DRPG {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void RenderControl_ContextCreated(object sender, GlControlEventArgs e)
		{
			GlControl glControl = (GlControl)sender;
			// Here you can allocate resources or initialize state
			Gl.MatrixMode(MatrixMode.Projection);
			Gl.LoadIdentity();
			Gl.Ortho(0.0, 1.0, 0.0, 1.0, 0.0, 1.0);

			// Uses multisampling, if available
			if (glControl.MultisampleBits > 0)
				Gl.Enable(EnableCap.Multisample);
		}

		private void RenderControl_Render(object sender, GlControlEventArgs e)
		{
				RenderControl_Render_GL(sender, e);
		}

		private void RenderControl_ContextUpdate(object sender, GlControlEventArgs e)
		{
			// Change triangle rotation
			_Angle = (_Angle + 0.1f) % 45.0f;
		}

		private void RenderControl_ContextDestroying(object sender, GlControlEventArgs e)
		{
			// Here you can dispose resources allocated in RenderControl_ContextCreated
		}

		#region Common Data

		private static float _Angle;

		/// <summary>
		/// Vertex position array.
		/// </summary>
		private static readonly float[] _ArrayPosition = new float[] {
			0.0f, 0.0f,
			0.5f, 1.0f,
			1.0f, 0.0f
		};

		/// <summary>
		/// Vertex color array.
		/// </summary>
		private static readonly float[] _ArrayColor = new float[] {
			1.0f, 0.0f, 0.0f,
			0.0f, 1.0f, 0.0f,
			0.0f, 0.0f, 1.0f
		};

		#endregion

		#region GL Resources

		private void RenderControl_Render_GL(object sender, GlControlEventArgs e)
		{
            Control senderControl = (Control)sender;

			Gl.Viewport(0, 0, senderControl.ClientSize.Width, senderControl.ClientSize.Height);
			Gl.Clear(ClearBufferMask.ColorBufferBit);

			// Animate triangle
			Gl.MatrixMode(MatrixMode.Modelview);
			Gl.LoadIdentity();
			Gl.Rotate(_Angle, 0.0f, 0.0f, 1.0f);

			if (Gl.CurrentVersion >= Gl.Version_110) {
				// Old school OpenGL 1.1
				// Setup & enable client states to specify vertex arrays, and use Gl.DrawArrays instead of Gl.Begin/End paradigm
				using (MemoryLock vertexArrayLock = new MemoryLock(_ArrayPosition)) 
				using (MemoryLock vertexColorLock = new MemoryLock(_ArrayColor))
				{
					// Note: the use of MemoryLock objects is necessary to pin vertex arrays since they can be reallocated by GC
					// at any time between the Gl.VertexPointer execution and the Gl.DrawArrays execution

					Gl.VertexPointer(2, VertexPointerType.Float, 0, vertexArrayLock.Address);
					Gl.EnableClientState(EnableCap.VertexArray);

					Gl.ColorPointer(3, ColorPointerType.Float, 0, vertexColorLock.Address);
					Gl.EnableClientState(EnableCap.ColorArray);

					Gl.DrawArrays(PrimitiveType.Triangles, 0, 3);
				}
			} else {
				// Old school OpenGL
				Gl.Begin(PrimitiveType.Triangles);
				Gl.Color3(1.0f, 0.0f, 0.0f); Gl.Vertex2(0.0f, 0.0f);
				Gl.Color3(0.0f, 1.0f, 0.0f); Gl.Vertex2(0.5f, 1.0f);
				Gl.Color3(0.0f, 0.0f, 1.0f); Gl.Vertex2(1.0f, 0.0f);
				Gl.End();
			}
		}

		#endregion


    }
}
