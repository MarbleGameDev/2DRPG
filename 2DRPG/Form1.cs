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

		public static bool contextCreated = false;

		public Form1() {
			InitializeComponent();
			Program.logic.Start();
		}

		private void RenderControl_ContextCreated(object sender, GlControlEventArgs e) {
			GlControl glControl = (GlControl)sender;
			// Here you can allocate resources or initialize state
			Gl.MatrixMode(MatrixMode.Projection);
			Gl.LoadIdentity();
			Gl.Ortho(OrthoLeft, OrthoRight, OrthoBottom, OrthoTop, -0.1, 10.0);
			Gl.Enable(EnableCap.Texture2d);
			Gl.Enable(EnableCap.Blend);
			Gl.Enable(EnableCap.DepthTest);		//Set up Z Depth tests for drawing pixels
			Gl.DepthFunc(DepthFunction.Lequal);		//Only draw something new if its z value is closer to the 'camera'
			Gl.DepthMask(true);
			Gl.ClearDepth(10f);
			Gl.Enable(EnableCap.AlphaTest);		//Set up Alpha tests for drawing pixels
			Gl.AlphaFunc(AlphaFunction.Greater, .05f);  //Don't draw transparent pixels on polygons

			Gl.Enable(EnableCap.ScissorTest);

			ResizeE(sender, e);

			// Uses multisampling, if available
			contextCreated = true;
			WorldData.WorldStartup();
			Screen.ScreenStartup();
			List<World.Objects.WorldObjectBase>[] tobjects = WorldData.currentRegions.Values.ToArray();		//Render the World Objects
			foreach (List<World.Objects.WorldObjectBase> l in tobjects) {
				foreach(World.Objects.WorldObjectBase o in l)
					o.ContextCreated();
			}

		}

		private void RenderControl_Render(object sender, GlControlEventArgs e) {
			RenderControl_Render_GL(sender, e);
		}

		private void RenderControl_ContextUpdate(object sender, GlControlEventArgs e) {
			List<World.Objects.WorldObjectBase>[] tobjects = WorldData.currentRegions.Values.ToArray();     //Render the World Objects
			foreach (List<World.Objects.WorldObjectBase> l in tobjects) {
				foreach (World.Objects.WorldObjectBase o in l)
					o.ContextUpdate();
			}

		}

		private void RenderControl_ContextDestroying(object sender, GlControlEventArgs e) {
			// Here you can dispose resources allocated in RenderControl_ContextCreated
			List<World.Objects.WorldObjectBase>[] tobjects = WorldData.currentRegions.Values.ToArray();     //Render the World Objects
			foreach (List<World.Objects.WorldObjectBase> l in tobjects) {
				foreach (World.Objects.WorldObjectBase o in l)
					o.ContextDestroyed();
			}
		}

		public static void ShiftOrtho(double x, double y) {
			OrthoLeft += x;
			OrthoRight += x;
			OrthoTop += y;
			OrthoBottom += y;
		}

		public static void SetOrtho(double x, double y) {
			OrthoLeft = x;
			OrthoRight = x;
			OrthoTop = y;
			OrthoBottom = y;
		}
		private static double OrthoLeft = 0;
		private static double OrthoRight = 0;
		private static double OrthoTop = 0;
		private static double OrthoBottom = 0;


		private void RenderControl_Render_GL(object sender, GlControlEventArgs e) {
			Gl.MatrixMode(MatrixMode.Projection);
			Gl.LoadIdentity();
			//Gl.Ortho(-Screen.screenWidth / 2 + OrthoLeft * Screen.screenHeight / 2, Screen.screenWidth / 2 + OrthoRight * Screen.screenHeight / 2, (-1 + OrthoBottom) * Screen.screenHeight / 2, (1 + OrthoTop) * Screen.screenHeight / 2, -0.1, 10.0);
			Gl.Ortho(0 + OrthoLeft, Screen.pixelWidth + OrthoRight, 0 + OrthoBottom, Screen.pixelHeight + OrthoTop, -.1, 10);
			Gl.ClearColor(Color.White.R, Color.White.G, Color.White.B, Color.White.A);
			Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			List<World.Objects.WorldObjectBase>[] tobjects = WorldData.currentRegions.Values.ToArray();     //Render the World Objects
			foreach (List<World.Objects.WorldObjectBase> l in tobjects) {
				foreach (World.Objects.WorldObjectBase o in l)
					o.Render();
			}

			//Load a separete projection for GUI rendering that doesn't move with the character
			Gl.MatrixMode(MatrixMode.Projection);
			Gl.LoadIdentity();
			Gl.Ortho(0, Screen.pixelWidth, 0, Screen.pixelHeight, -.1, 10);

			GUI.UIBase[] guiObjects = Screen.UIObjects.ToArray();   //Render the GUI Objects
			foreach (GUI.UIBase u in guiObjects) {
				u.Render();
			}
			Input.UpdateKeys();
		}

		private void KeyDownE(object sender, KeyEventArgs e) {
			Input.KeyDown(sender, e);
		}

		private void MClick(object sender, MouseEventArgs e) {
			Input.MouseSent(sender, e);
		}

		private void KeyUpE(object sender, KeyEventArgs e) {
			Input.KeyUp(sender, e);
		}

		private void ResizeE(object sender, EventArgs e) {
			Control senderControl = (Control)sender;
			int idealWidth = (int)(senderControl.ClientSize.Height * (16f / 9));        //Center the 16:9 Viewport in the middle of the window regardless of window dimensions
			if (senderControl.ClientSize.Width > idealWidth) {
				Gl.Viewport((senderControl.ClientSize.Width - idealWidth) / 2, 0, idealWidth, senderControl.ClientSize.Height);
				Gl.Scissor((senderControl.ClientSize.Width - idealWidth) / 2, 0, idealWidth, senderControl.ClientSize.Height);
				Screen.SetScreenDimensions(idealWidth, senderControl.ClientSize.Height);
			} else if (senderControl.ClientSize.Width < idealWidth) {
				int idealheight = (int)(senderControl.ClientSize.Width * (9f / 16));
				Gl.Viewport(0, (senderControl.Height - idealheight) / 2, senderControl.ClientSize.Width, idealheight);
				Gl.Scissor(0, (senderControl.Height - idealheight) / 2, senderControl.ClientSize.Width, idealheight);
				Screen.SetScreenDimensions(senderControl.ClientSize.Width, idealheight);
			} else {
				Gl.Viewport(0, 0, senderControl.ClientSize.Width, senderControl.ClientSize.Height);
				Gl.Scissor(0, 0, senderControl.ClientSize.Width, senderControl.ClientSize.Height);
				Screen.SetScreenDimensions(senderControl.ClientSize.Width, senderControl.ClientSize.Height);
			}
			Screen.SetWindowDimensions(senderControl.ClientSize.Width, senderControl.ClientSize.Height);
		}
	}
}
