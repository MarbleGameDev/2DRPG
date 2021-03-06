﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenGL;
using System.Diagnostics;

namespace _2DRPG {
    partial class Form1 : Form {

		public static bool contextCreated = false;

		private static bool updateResize = false;
		private static bool updateFullscreen = false;

		public static GUI.UIDraggable dragged;

		public static DevWindow devWin = new DevWindow();

		public Form1() {

			InitializeComponent();
			Save.SaveData.LoadGameData();
			Save.SaveData.LoadGame();
			if (Save.SaveData.GameSettings.devWindow)
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
			Gl.ClearColor(36 / 255f, 71 / 255f, 143 / 255f, 1f);
			Gl.Enable(EnableCap.AlphaTest);		//Set up Alpha tests for drawing pixels
			Gl.AlphaFunc(AlphaFunction.Greater, .05f);  //Don't draw transparent pixels on polygons
			Gl.Enable(EnableCap.ScissorTest);
			Gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
			Gl.EnableClientState(EnableCap.VertexArray);
			Gl.EnableClientState(EnableCap.TextureCoordArray);
			contextCreated = true;
			Console.Console.ConsoleSetup();
			WorldData.WorldStartup();
			Screen.ScreenStartup();

			MaximizeBox = false;

			if (Save.SaveData.GameSettings.fullScreen) {
				FormBorderStyle = FormBorderStyle.None;
				WindowState = FormWindowState.Maximized;
			}
			if (Save.SaveData.GameSettings.VSync) {
				Wgl.SwapIntervalEXT(-1); //Swap Interval (V-Sync Enabled at 1 or -1)
			} else {
				Wgl.SwapIntervalEXT(0);
			}
			ResizeWindow(Save.SaveData.GameSettings.windowx, Save.SaveData.GameSettings.windowy);
			ResizeE(sender, e);

			LogicUtils.Logic.LogicStart();
		}

		public static void ResizeWindow(int x, int y) {
			if (Save.SaveData.GameSettings.fullScreen) {
				Program.mainForm.FormBorderStyle = FormBorderStyle.FixedSingle;
				Program.mainForm.WindowState = FormWindowState.Normal;
				Save.SaveData.GameSettings.fullScreen = false;
			}
			Program.mainForm.ClientSize = new Size(x, y);
			Program.mainForm.CenterToScreen();
			Save.SaveData.GameSettings.windowx = x;
			Save.SaveData.GameSettings.windowy = y;
			updateResize = true;
		}
		public static void SetFullscreen() {
			Save.SaveData.GameSettings.fullScreen = true;
			updateFullscreen = true;
		}

		private void RenderControl_Render(object sender, GlControlEventArgs e) {
			RenderControl_Render_GL(sender, e);
		}

		private void RenderControl_ContextUpdate(object sender, GlControlEventArgs e) {

		}

		private void RenderControl_ContextDestroying(object sender, GlControlEventArgs e) {

		}

		public static void ShiftOrtho(double x, double y) {
			OrthoLeft += x;
			OrthoRight += x;
			OrthoTop += y;
			OrthoBottom += y;
		}

		public static void SetOrtho(double x, double y) {
			OrthoLeft = x - 320;
			OrthoRight = x + 320;
			OrthoTop = y + 180;
			OrthoBottom = y - 180;
		}
		private static double OrthoLeft = -320;
		private static double OrthoRight = 320;
		private static double OrthoTop = 180;
		private static double OrthoBottom = -180;

		public static event Action OrthoUpdate;

		public static void AddOrthoUpdate(Action a) {
			OrthoUpdate += a;
		}

		/// <summary>
		/// Method that gets called to render each frame
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RenderControl_Render_GL(object sender, GlControlEventArgs e) {
			if (!contextCreated)
				return;
			if (updateFullscreen) {
				FormBorderStyle = FormBorderStyle.None;
				WindowState = FormWindowState.Maximized;
				updateFullscreen = false;
			} else if (updateResize) {
				ResizeE(sender, e);
				updateResize = false;
			}

			Gl.LoadIdentity();
			if (OrthoUpdate != null) {
				OrthoUpdate.Invoke();
				OrthoUpdate = null;
			}
			Gl.Ortho(OrthoLeft, OrthoRight, OrthoBottom, OrthoTop, -0.1, 10.0);
			Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			if (GameState.CurrentState == GameState.GameStates.Paused || GameState.WindowOpen)
				Gl.Color3(.5f, .5f, .5f);
			//Render World Objects
			WorldData.WorldRender();

			if (GameState.CurrentState == GameState.GameStates.Paused || GameState.WindowOpen)
				Gl.Color3(1f, 1f, 1f);
			//Load a separete projection for GUI rendering that doesn't move with the character
			Gl.LoadIdentity();
			Gl.Ortho(-320, 320, -180, 180, -0.1, 10.0);
			Screen.RunQueue();
			lock (Screen.currentWindows) {
				foreach(HashSet<GUI.UIBase> b in Screen.currentWindows.Values)   //Render the GUI Objects
					foreach (GUI.UIBase u in b) {
						u.Render();
					}
			}
			Gl.BindTexture(TextureTarget.Texture2d, 0);

			//Other updates needed each frame
			Input.UpdateKeys();
			if (dragged != null)
				if (Math.Abs(Input.MouseX) < 320f && Math.Abs(Input.MouseY) < 180f)
					dragged.positionUpdate.Invoke();
			
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
			dragged = null;
		}

		private void Form1_KeyPress(object sender, KeyPressEventArgs e) {
			Input.KeyPress(e);
		}

		private void MScroll(object sender, MouseEventArgs e) {
			Input.MouseScroll(e);
		}
	}
}
