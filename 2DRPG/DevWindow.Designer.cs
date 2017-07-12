namespace _2DRPG {
	partial class DevWindow {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DevWindow));
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.Overview = new System.Windows.Forms.TabPage();
			this.InteractionTree = new System.Windows.Forms.TabPage();
			this.panel1 = new System.Windows.Forms.Panel();
			this.delButt = new System.Windows.Forms.Button();
			this.applyButt = new System.Windows.Forms.Button();
			this.groupBox = new System.Windows.Forms.GroupBox();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
			this.choiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.dialogueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.questToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.save = new System.Windows.Forms.Button();
			this.SaveButton = new System.Windows.Forms.Button();
			this.TextBox = new System.Windows.Forms.RichTextBox();
			this.treeView = new System.Windows.Forms.TreeView();
			this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
			this.worldXLabel = new System.Windows.Forms.Label();
			this.worldYLabel = new System.Windows.Forms.Label();
			this.updateButton = new System.Windows.Forms.Button();
			this.tabControl1.SuspendLayout();
			this.Overview.SuspendLayout();
			this.InteractionTree.SuspendLayout();
			this.panel1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.Overview);
			this.tabControl1.Controls.Add(this.InteractionTree);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(1027, 567);
			this.tabControl1.TabIndex = 0;
			// 
			// Overview
			// 
			this.Overview.Controls.Add(this.updateButton);
			this.Overview.Controls.Add(this.worldYLabel);
			this.Overview.Controls.Add(this.worldXLabel);
			this.Overview.Location = new System.Drawing.Point(4, 22);
			this.Overview.Name = "Overview";
			this.Overview.Padding = new System.Windows.Forms.Padding(3);
			this.Overview.Size = new System.Drawing.Size(1019, 541);
			this.Overview.TabIndex = 0;
			this.Overview.Text = "Overview";
			this.Overview.UseVisualStyleBackColor = true;
			// 
			// InteractionTree
			// 
			this.InteractionTree.Controls.Add(this.panel1);
			this.InteractionTree.Controls.Add(this.treeView);
			this.InteractionTree.Location = new System.Drawing.Point(4, 22);
			this.InteractionTree.Name = "InteractionTree";
			this.InteractionTree.Padding = new System.Windows.Forms.Padding(3);
			this.InteractionTree.Size = new System.Drawing.Size(1019, 541);
			this.InteractionTree.TabIndex = 1;
			this.InteractionTree.Text = "InteractionTree";
			this.InteractionTree.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.delButt);
			this.panel1.Controls.Add(this.applyButt);
			this.panel1.Controls.Add(this.groupBox);
			this.panel1.Controls.Add(this.toolStrip1);
			this.panel1.Controls.Add(this.save);
			this.panel1.Controls.Add(this.SaveButton);
			this.panel1.Controls.Add(this.TextBox);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(340, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(676, 535);
			this.panel1.TabIndex = 1;
			// 
			// delButt
			// 
			this.delButt.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.delButt.Location = new System.Drawing.Point(387, 508);
			this.delButt.Name = "delButt";
			this.delButt.Size = new System.Drawing.Size(81, 25);
			this.delButt.TabIndex = 7;
			this.delButt.Text = "Delete Item";
			this.delButt.UseVisualStyleBackColor = true;
			this.delButt.Click += new System.EventHandler(this.DelButt_Click);
			// 
			// applyButt
			// 
			this.applyButt.Location = new System.Drawing.Point(213, 151);
			this.applyButt.Name = "applyButt";
			this.applyButt.Size = new System.Drawing.Size(75, 23);
			this.applyButt.TabIndex = 6;
			this.applyButt.Text = "Apply";
			this.applyButt.UseVisualStyleBackColor = true;
			this.applyButt.Click += new System.EventHandler(this.ApplyButt_Click);
			// 
			// groupBox
			// 
			this.groupBox.Location = new System.Drawing.Point(7, 151);
			this.groupBox.Name = "groupBox";
			this.groupBox.Size = new System.Drawing.Size(200, 100);
			this.groupBox.TabIndex = 5;
			this.groupBox.TabStop = false;
			this.groupBox.Text = "Item Properties";
			// 
			// toolStrip1
			// 
			this.toolStrip1.BackColor = System.Drawing.Color.Gainsboro;
			this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStrip1.GripMargin = new System.Windows.Forms.Padding(0);
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1});
			this.toolStrip1.Location = new System.Drawing.Point(301, 508);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.toolStrip1.Size = new System.Drawing.Size(72, 25);
			this.toolStrip1.TabIndex = 4;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripDropDownButton1
			// 
			this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.choiceToolStripMenuItem,
            this.dialogueToolStripMenuItem,
            this.questToolStripMenuItem});
			this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
			this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
			this.toolStripDropDownButton1.Size = new System.Drawing.Size(69, 22);
			this.toolStripDropDownButton1.Text = "Add Item";
			this.toolStripDropDownButton1.ToolTipText = "Adds a child to the currently selected node if Choice, parent node if not";
			// 
			// choiceToolStripMenuItem
			// 
			this.choiceToolStripMenuItem.Name = "choiceToolStripMenuItem";
			this.choiceToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
			this.choiceToolStripMenuItem.Text = "Choice";
			this.choiceToolStripMenuItem.Click += new System.EventHandler(this.ChoiceToolStripMenuItem_Click);
			// 
			// dialogueToolStripMenuItem
			// 
			this.dialogueToolStripMenuItem.Name = "dialogueToolStripMenuItem";
			this.dialogueToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
			this.dialogueToolStripMenuItem.Text = "Dialogue";
			this.dialogueToolStripMenuItem.Click += new System.EventHandler(this.DialogueToolStripMenuItem_Click);
			// 
			// questToolStripMenuItem
			// 
			this.questToolStripMenuItem.Name = "questToolStripMenuItem";
			this.questToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
			this.questToolStripMenuItem.Text = "Quest";
			this.questToolStripMenuItem.Click += new System.EventHandler(this.QuestToolStripMenuItem_Click);
			// 
			// save
			// 
			this.save.Location = new System.Drawing.Point(596, 151);
			this.save.Name = "save";
			this.save.Size = new System.Drawing.Size(75, 23);
			this.save.TabIndex = 3;
			this.save.Text = "Save";
			this.save.UseVisualStyleBackColor = true;
			this.save.Click += new System.EventHandler(this.Save_Click);
			// 
			// SaveButton
			// 
			this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.SaveButton.Location = new System.Drawing.Point(550, 503);
			this.SaveButton.Name = "SaveButton";
			this.SaveButton.Size = new System.Drawing.Size(126, 32);
			this.SaveButton.TabIndex = 1;
			this.SaveButton.Text = "Save Game";
			this.SaveButton.UseVisualStyleBackColor = true;
			this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
			// 
			// TextBox
			// 
			this.TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TextBox.Location = new System.Drawing.Point(0, 0);
			this.TextBox.Name = "TextBox";
			this.TextBox.Size = new System.Drawing.Size(676, 144);
			this.TextBox.TabIndex = 0;
			this.TextBox.Text = "";
			// 
			// treeView
			// 
			this.treeView.Dock = System.Windows.Forms.DockStyle.Left;
			this.treeView.Location = new System.Drawing.Point(3, 3);
			this.treeView.Name = "treeView";
			this.treeView.Size = new System.Drawing.Size(337, 535);
			this.treeView.TabIndex = 0;
			// 
			// printPreviewDialog1
			// 
			this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
			this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
			this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
			this.printPreviewDialog1.Enabled = true;
			this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
			this.printPreviewDialog1.Name = "printPreviewDialog1";
			this.printPreviewDialog1.Visible = false;
			// 
			// worldXLabel
			// 
			this.worldXLabel.AutoSize = true;
			this.worldXLabel.Location = new System.Drawing.Point(9, 7);
			this.worldXLabel.Name = "worldXLabel";
			this.worldXLabel.Size = new System.Drawing.Size(51, 13);
			this.worldXLabel.TabIndex = 0;
			this.worldXLabel.Text = "World X: ";
			// 
			// worldYLabel
			// 
			this.worldYLabel.AutoSize = true;
			this.worldYLabel.Location = new System.Drawing.Point(9, 20);
			this.worldYLabel.Name = "worldYLabel";
			this.worldYLabel.Size = new System.Drawing.Size(51, 13);
			this.worldYLabel.TabIndex = 1;
			this.worldYLabel.Text = "World Y: ";
			// 
			// updateButton
			// 
			this.updateButton.Location = new System.Drawing.Point(936, 7);
			this.updateButton.Name = "updateButton";
			this.updateButton.Size = new System.Drawing.Size(75, 23);
			this.updateButton.TabIndex = 2;
			this.updateButton.Text = "Update";
			this.updateButton.UseVisualStyleBackColor = true;
			this.updateButton.Click += new System.EventHandler(this.UpdateButton_Click);
			// 
			// DevWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1027, 567);
			this.Controls.Add(this.tabControl1);
			this.Name = "DevWindow";
			this.Text = "DevWindow";
			this.tabControl1.ResumeLayout(false);
			this.Overview.ResumeLayout(false);
			this.Overview.PerformLayout();
			this.InteractionTree.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage Overview;
		private System.Windows.Forms.TabPage InteractionTree;
		private System.Windows.Forms.TreeView treeView;
		private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.RichTextBox TextBox;
		private System.Windows.Forms.Button SaveButton;
		private System.Windows.Forms.Button save;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
		private System.Windows.Forms.ToolStripMenuItem choiceToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem dialogueToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem questToolStripMenuItem;
		private System.Windows.Forms.GroupBox groupBox;
		private System.Windows.Forms.Button applyButt;
		private System.Windows.Forms.Button delButt;
		private System.Windows.Forms.Label worldYLabel;
		private System.Windows.Forms.Label worldXLabel;
		private System.Windows.Forms.Button updateButton;
	}
}