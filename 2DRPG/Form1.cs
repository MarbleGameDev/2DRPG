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

		private static bool updateResize = false;
		private static bool updateFullscreen = false;

		public static DevWindow devWin = new DevWindow();

		public Form1() {
			InitializeComponent();
			SaveData.LoadGame();
			if (SaveData.GameSettings.interactionEditor)
				devWin.Show();
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
			Wgl.SwapIntervalEXT(1);	//Swap Interval (V-Sync Enabled at 1 or -1)
			contextCreated = true;
			WorldData.WorldStartup();
			Screen.ScreenStartup();
			lock (WorldData.currentRegions) {
				foreach (HashSet<World.Objects.WorldObjectBase> l in WorldData.currentRegions.Values) {
					foreach (World.Objects.WorldObjectBase o in l)
						o.ContextCreated();
				}
			}
			if (SaveData.GameSettings.fullScreen) {
				FormBorderStyle = FormBorderStyle.None;
				WindowState = FormWindowState.Maximized;
			}
			ResizeWindow(SaveData.GameSettings.windowx, SaveData.GameSettings.windowy);
			ResizeE(sender, e);

			LogicUtils.Logic.LogicStart();
		}

		public static void ResizeWindow(int x, int y) {
			if (SaveData.GameSettings.fullScreen) {
				Program.mainForm.FormBorderStyle = FormBorderStyle.FixedSingle;
				Program.mainForm.WindowState = FormWindowState.Normal;
				SaveData.GameSettings.fullScreen = false;
			}
			Program.mainForm.ClientSize = new Size(x, y);
			Program.mainForm.CenterToScreen();
			SaveData.GameSettings.windowx = x;
			SaveData.GameSettings.windowy = y;
			updateResize = true;
		}
		public static void SetFullscreen() {
			SaveData.GameSettings.fullScreen = true;
			updateFullscreen = true;
		}

		private void RenderControl_Render(object sender, GlControlEventArgs e) {
			RenderControl_Render_GL(sender, e);
		}

		private void RenderControl_ContextUpdate(object sender, GlControlEventArgs e) {
			lock (WorldData.currentRegions) {     //Render the World Objects
				foreach (HashSet<World.Objects.WorldObjectBase> l in WorldData.currentRegions.Values) {
					foreach (World.Objects.WorldObjectBase o in l)
						o.ContextUpdate();
				}
			}

		}

		private void RenderControl_ContextDestroying(object sender, GlControlEventArgs e) {
			// Here you can dispose resources allocated in RenderControl_ContextCreated
			lock (WorldData.currentRegions) {     //Render the World Objects
				foreach (HashSet<World.Objects.WorldObjectBase> l in WorldData.currentRegions.Values) {
					foreach (World.Objects.WorldObjectBase o in l)
						o.ContextDestroyed();
				}
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
		private static double OrthoLeft = -320;
		private static double OrthoRight = 320;
		private static double OrthoTop = 180;
		private static double OrthoBottom = -180;


		private void RenderControl_Render_GL(object sender, GlControlEventArgs e) {
			if (updateFullscreen) {
				FormBorderStyle = FormBorderStyle.None;
				WindowState = FormWindowState.Maximized;
				updateFullscreen = false;
			} else if (updateResize) {
				ResizeE(sender, e);
				updateResize = false;
			}
			Gl.MatrixMode(MatrixMode.Projection);
			Gl.LoadIdentity();
			//Gl.Ortho(-Screen.screenWidth / 2 + OrthoLeft * Screen.screenHeight / 2, Screen.screenWidth / 2 + OrthoRight * Screen.screenHeight / 2, (-1 + OrthoBottom) * Screen.screenHeight / 2, (1 + OrthoTop) * Screen.screenHeight / 2, -0.1, 10.0);
			Gl.Ortho(OrthoLeft, OrthoRight, OrthoBottom, OrthoTop, -0.1, 10.0);
			Gl.ClearColor(36 / 255f, 71 / 255f, 143 / 255f, 1f);
			Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			if (GameState.CurrentState == GameState.GameStates.Paused || GameState.WindowOpen)
				Gl.Color3(.5f, .5f, .5f);

			//Render World Objects
			lock (WorldData.currentRegions) {
				foreach (HashSet<World.Objects.WorldObjectBase> l in WorldData.currentRegions.Values) {
					foreach (World.Objects.WorldObjectBase o in l)
						o.Render();
				}

				foreach (GUI.UIBase b in WorldData.worldUIs.ToArray())
					b.Render();
			}
			
			WorldData.controllableOBJ.Render();

			if (GameState.CurrentState == GameState.GameStates.Paused || GameState.WindowOpen)
				Gl.Color3(1f, 1f, 1f);

			//Load a separete projection for GUI rendering that doesn't move with the character
			Gl.MatrixMode(MatrixMode.Projection);
			Gl.LoadIdentity();
			Gl.Ortho(-320, 320, -180, 180, -0.1, 10.0);

			Screen.RunQueue();

			lock (Screen.currentWindows) {
				foreach(HashSet<GUI.UIBase> b in Screen.currentWindows.Values)   //Render the GUI Objects
					foreach (GUI.UIBase u in b) {
						u.Render();
					}
			}
			Input.UpdateKeys();
		}

		private void ResizeE(object sender, EventArgs e) {
			Input.ClearKeys();
			Control senderControl = (Control)sender;
			int idealWidth = (int)(senderControl.ClientSize.Height * (16f / 9));        //Center the 16:9 Viewport in the middle of the window regardless of window dimensions
			if (senderControl.ClientSize.Width > idealWidth) {
				Gl.Viewport((senderControl.ClientSize.Width - idealWidth) / 2, 0, idealWidth, senderControl.ClientSize.Height);
				Gl.Scissor((senderControl.ClientSize.Width - idealWidth) / 2, 0, idealWidth, senderControl.ClientSize.Height);
				Screen.SetScreenDimensions((senderControl.ClientSize.Width - idealWidth) / 2, 0, idealWidth, senderControl.ClientSize.Height);
			} else if (senderControl.ClientSize.Width < idealWidth) {
				int idealheight = (int)(senderControl.ClientSize.Width * (9f / 16));
				Gl.Viewport(0, (senderControl.Height - idealheight) / 2, senderControl.ClientSize.Width, idealheight);
				Gl.Scissor(0, (senderControl.Height - idealheight) / 2, senderControl.ClientSize.Width, idealheight);
				Screen.SetScreenDimensions(0, (senderControl.Height - idealheight) / 2, senderControl.ClientSize.Width, idealheight);
			} else {
				Gl.Viewport(0, 0, senderControl.ClientSize.Width, senderControl.ClientSize.Height);
				Gl.Scissor(0, 0, senderControl.ClientSize.Width, senderControl.ClientSize.Height);
				Screen.SetScreenDimensions(0, 0, senderControl.ClientSize.Width, senderControl.ClientSize.Height);
			}
			Screen.SetWindowDimensions(senderControl.ClientSize.Width, senderControl.ClientSize.Height);
		}

        private void Form1_Load(object sender, EventArgs e) { }

		private void KeyDownE(object sender, KeyEventArgs e) {
			Input.KeyDown(e);
		}

		private void MClick(object sender, MouseEventArgs e) {
			Input.MouseHeld = true;
			Input.MouseSent(e);
		}

		private void KeyUpE(object sender, KeyEventArgs e) {
			Input.KeyUp(e);
		}

		private void LostFocusE(object sender, EventArgs e) {
			Input.ClearKeys();
		}

		private void MMove(object sender, MouseEventArgs e) {
			Input.MouseMove(e);
		}

		private void MUp(object sender, MouseEventArgs e) {
			Input.MouseHeld = false;
		}

		private void Form1_KeyPress(object sender, KeyPressEventArgs e) {
			Input.KeyPress(e);
		}

		private void MScroll(object sender, MouseEventArgs e) {
			Input.MouseScroll(e);
		}
	}
}
