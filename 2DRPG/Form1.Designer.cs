namespace _2DRPG {
	partial class Form1 {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.RenderControl = new OpenGL.GlControl();
			this.SuspendLayout();
			// 
			// RenderControl
			// 
			this.RenderControl.Animation = true;
			this.RenderControl.BackColor = System.Drawing.Color.DimGray;
			this.RenderControl.ColorBits = ((uint)(24u));
			this.RenderControl.DepthBits = ((uint)(24u));
			this.RenderControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RenderControl.Location = new System.Drawing.Point(0, 0);
			this.RenderControl.MultisampleBits = ((uint)(0u));
			this.RenderControl.Name = "RenderControl";
			this.RenderControl.Size = new System.Drawing.Size(992, 558);
			this.RenderControl.StencilBits = ((uint)(0u));
			this.RenderControl.SwapInterval = 0;
			this.RenderControl.TabIndex = 0;
			this.RenderControl.ContextCreated += new System.EventHandler<OpenGL.GlControlEventArgs>(this.RenderControl_ContextCreated);
			this.RenderControl.ContextDestroying += new System.EventHandler<OpenGL.GlControlEventArgs>(this.RenderControl_ContextDestroying);
			this.RenderControl.Render += new System.EventHandler<OpenGL.GlControlEventArgs>(this.RenderControl_Render);
			this.RenderControl.ContextUpdate += new System.EventHandler<OpenGL.GlControlEventArgs>(this.RenderControl_ContextUpdate);
			this.RenderControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownE);
			this.RenderControl.KeyUp += new System.Windows.Forms.KeyEventHandler(this.KeyUpE);
			this.RenderControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MClick);
			this.RenderControl.Resize += new System.EventHandler(this.ResizeE);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(992, 558);
			this.Controls.Add(this.RenderControl);
			this.Name = "Form1";
			this.Text = "Test Form 1";
			this.ResumeLayout(false);

		}

		#endregion;
		private OpenGL.GlControl RenderControl;
	}
}

