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
			Program.logic.Start();
		}

		private void RenderControl_ContextCreated(object sender, GlControlEventArgs e) {
			GlControl glControl = (GlControl)sender;
			// Here you can allocate resources or initialize state
			Gl.MatrixMode(MatrixMode.Projection);
			Gl.LoadIdentity();
			Gl.Ortho(0.0, 1.0, 0.0, 1.0, 0.0, 1.0);
			Gl.Enable(EnableCap.Texture2d);

			// Uses multisampling, if available
			if (glControl.MultisampleBits > 0)
				Gl.Enable(EnableCap.Multisample);

			object[] tobjects = WorldData.currentObjects.ToArray();
			foreach (object o in tobjects) {
				if (o is IRenderable)
					((IRenderable)o).ContextCreated();
			}
		}

		private void RenderControl_Render(object sender, GlControlEventArgs e) {
			RenderControl_Render_GL(sender, e);
		}

		private void RenderControl_ContextUpdate(object sender, GlControlEventArgs e) {
			object[] tobjects = WorldData.currentObjects.ToArray();
			foreach (object o in tobjects) {
				if (o is IRenderable)
				((IRenderable)o).ContextUpdate();
			}
			// Change triangle rotation

		}

		private void RenderControl_ContextDestroying(object sender, GlControlEventArgs e) {
			// Here you can dispose resources allocated in RenderControl_ContextCreated
			object[] tobjects = WorldData.currentObjects.ToArray();
			foreach (object o in tobjects) {
				if (o is IRenderable)
					((IRenderable)o).ContextDestroyed();
			}
		}

		#region Common Data



		#endregion

		#region GL Resources

		private void RenderControl_Render_GL(object sender, GlControlEventArgs e) {
			Control senderControl = (Control)sender;

			Gl.Viewport(0, 0, senderControl.ClientSize.Width, senderControl.ClientSize.Height);
			Gl.Clear(ClearBufferMask.ColorBufferBit);

			// Animate triangle
			Gl.MatrixMode(MatrixMode.Modelview);
			Gl.LoadIdentity();

			// Setup & enable client states to specify vertex arrays, and use Gl.DrawArrays instead of Gl.Begin/End paradigm
			object[] tobjects = WorldData.currentObjects.ToArray();
			foreach (object o in tobjects) {
				if (o is IRenderable)
					((IRenderable)o).Render();
			}
		}
		#endregion
	}
}
