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
			this.updateButton = new System.Windows.Forms.Button();
			this.worldYLabel = new System.Windows.Forms.Label();
			this.worldXLabel = new System.Windows.Forms.Label();
			this.InteractionTree = new System.Windows.Forms.TabPage();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.DownButton = new System.Windows.Forms.Button();
			this.UpButton = new System.Windows.Forms.Button();
			this.delButt = new System.Windows.Forms.Button();
			this.applyButt = new System.Windows.Forms.Button();
			this.groupBox = new System.Windows.Forms.GroupBox();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
			this.choiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.dialogueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.questToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.save = new System.Windows.Forms.Button();
			this.SaveButton = new System.Windows.Forms.Button();
			this.TextBox = new System.Windows.Forms.RichTextBox();
			this.treeView = new System.Windows.Forms.TreeView();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.button4 = new System.Windows.Forms.Button();
			this.panel2 = new System.Windows.Forms.Panel();
			this.button3 = new System.Windows.Forms.Button();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.button2 = new System.Windows.Forms.Button();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.button1 = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.tabControl1.SuspendLayout();
			this.Overview.SuspendLayout();
			this.InteractionTree.SuspendLayout();
			this.panel1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.Overview);
			this.tabControl1.Controls.Add(this.InteractionTree);
			this.tabControl1.Controls.Add(this.tabPage1);
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
			// worldYLabel
			// 
			this.worldYLabel.AutoSize = true;
			this.worldYLabel.Location = new System.Drawing.Point(9, 20);
			this.worldYLabel.Name = "worldYLabel";
			this.worldYLabel.Size = new System.Drawing.Size(51, 13);
			this.worldYLabel.TabIndex = 1;
			this.worldYLabel.Text = "World Y: ";
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
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.DownButton);
			this.panel1.Controls.Add(this.UpButton);
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
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(7, 454);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(76, 13);
			this.label1.TabIndex = 10;
			this.label1.Text = "Shift Selected:";
			// 
			// DownButton
			// 
			this.DownButton.Location = new System.Drawing.Point(7, 503);
			this.DownButton.Name = "DownButton";
			this.DownButton.Size = new System.Drawing.Size(75, 23);
			this.DownButton.TabIndex = 9;
			this.DownButton.Text = "Down";
			this.DownButton.UseVisualStyleBackColor = true;
			this.DownButton.Click += new System.EventHandler(this.DownButton_Click);
			// 
			// UpButton
			// 
			this.UpButton.Location = new System.Drawing.Point(7, 473);
			this.UpButton.Name = "UpButton";
			this.UpButton.Size = new System.Drawing.Size(75, 23);
			this.UpButton.TabIndex = 8;
			this.UpButton.Text = "Up";
			this.UpButton.UseVisualStyleBackColor = true;
			this.UpButton.Click += new System.EventHandler(this.UpButton_Click);
			// 
			// delButt
			// 
			this.delButt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.delButt.Location = new System.Drawing.Point(407, 508);
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
			this.groupBox.Size = new System.Drawing.Size(200, 279);
			this.groupBox.TabIndex = 5;
			this.groupBox.TabStop = false;
			this.groupBox.Text = "Item Properties";
			// 
			// toolStrip1
			// 
			this.toolStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.toolStrip1.BackColor = System.Drawing.Color.Gainsboro;
			this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStrip1.GripMargin = new System.Windows.Forms.Padding(0);
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1});
			this.toolStrip1.Location = new System.Drawing.Point(332, 508);
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
            this.questToolStripMenuItem,
            this.pathToolStripMenuItem});
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
			// pathToolStripMenuItem
			// 
			this.pathToolStripMenuItem.Name = "pathToolStripMenuItem";
			this.pathToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
			this.pathToolStripMenuItem.Text = "Path";
			this.pathToolStripMenuItem.Click += new System.EventHandler(this.PathToolStripMenuItem_Click);
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
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.button4);
			this.tabPage1.Controls.Add(this.panel2);
			this.tabPage1.Controls.Add(this.button3);
			this.tabPage1.Controls.Add(this.checkBox2);
			this.tabPage1.Controls.Add(this.button2);
			this.tabPage1.Controls.Add(this.checkBox1);
			this.tabPage1.Controls.Add(this.button1);
			this.tabPage1.Controls.Add(this.groupBox1);
			this.tabPage1.Controls.Add(this.label2);
			this.tabPage1.Controls.Add(this.listBox1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(1019, 541);
			this.tabPage1.TabIndex = 2;
			this.tabPage1.Text = "Quest";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(394, 506);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(59, 23);
			this.button4.TabIndex = 14;
			this.button4.Text = "AddItem";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.AddItemClick);
			// 
			// panel2
			// 
			this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.panel2.AutoScroll = true;
			this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel2.Location = new System.Drawing.Point(187, 318);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(204, 212);
			this.panel2.TabIndex = 13;
			// 
			// button3
			// 
			this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button3.Location = new System.Drawing.Point(890, 506);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(126, 32);
			this.button3.TabIndex = 12;
			this.button3.Text = "Save Game";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.SaveGameClick);
			// 
			// checkBox2
			// 
			this.checkBox2.AutoSize = true;
			this.checkBox2.Location = new System.Drawing.Point(294, 9);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(76, 17);
			this.checkBox2.TabIndex = 11;
			this.checkBox2.Text = "Completed";
			this.checkBox2.UseVisualStyleBackColor = true;
			this.checkBox2.CheckedChanged += new System.EventHandler(this.CompletedCheckChanged);
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.Location = new System.Drawing.Point(941, 6);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 9;
			this.button2.Text = "Save";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.SaveClick);
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(210, 9);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(56, 17);
			this.checkBox1.TabIndex = 8;
			this.checkBox1.Text = "Active";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.ActiveCheckChanged);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(128, 7);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(52, 20);
			this.button1.TabIndex = 7;
			this.button1.Text = "Update";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.UpdateQuestClick);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox1.Location = new System.Drawing.Point(186, 32);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(204, 279);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Quest Data";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(9, 7);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(57, 17);
			this.label2.TabIndex = 1;
			this.label2.Text = "Quests:";
			// 
			// listBox1
			// 
			this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.listBox1.FormattingEnabled = true;
			this.listBox1.Location = new System.Drawing.Point(8, 32);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(172, 498);
			this.listBox1.TabIndex = 0;
			this.listBox1.SelectedIndexChanged += new System.EventHandler(this.IndexChanged);
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
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage Overview;
		private System.Windows.Forms.TabPage InteractionTree;
		private System.Windows.Forms.TreeView treeView;
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
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button DownButton;
		private System.Windows.Forms.Button UpButton;
		private System.Windows.Forms.ToolStripMenuItem pathToolStripMenuItem;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button button4;
	}
}