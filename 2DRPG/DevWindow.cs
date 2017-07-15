using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _2DRPG.GUI.Interaction;

namespace _2DRPG {
	public partial class DevWindow : Form {
		public DevWindow() {
			InitializeComponent();
			Interaction.displayTree = treeView;
			Interaction.displayTree.NodeMouseClick += new TreeNodeMouseClickEventHandler(Interaction.UpdateNodeData);
			Interaction.text = TextBox;
			Interaction.save = save;
			Interaction.group = groupBox;
			Interaction.toolButton = (ToolStripDropDownButton)toolStrip1.Items[0];
			TextBox.Visible = false;
			save.Visible = false;
			applyButt.Visible = false;
			groupBox.Visible = false;
			Interaction.toolButton.DropDown.Items[3].Visible = false;
		}


		public static class Interaction {
			public static World.Objects.WorldObjectInteractable interactedObject;
			public static TreeView displayTree;
			public static RichTextBox text;
			public static GroupBox group;
			public static Button save;
			static InteractionBase currentObject;
			public static ToolStripDropDownButton toolButton;

			static Dictionary<TreeNode, InteractionBase> nodeBase = new Dictionary<TreeNode, InteractionBase>();

			public static void SetupTree() {
				if (displayTree == null)
					return;
				displayTree.Nodes.Clear();
				nodeBase.Clear();
				text.Visible = false;
				save.Visible = false;
				foreach (InteractionBase b in interactedObject.InterItems) {
					if (b is InteractionChoice c) {
						TreeNode tn = new TreeNode(c.ToString(), GetTreeArray(c));
						displayTree.Nodes.Add(tn);
						nodeBase.Add(tn, c);
					} else {
						TreeNode tn = new TreeNode(b.ToString());
						displayTree.Nodes.Add(tn);
						nodeBase.Add(tn, b);
					}
				}
			}

			private static TreeNode[] GetTreeArray(InteractionChoice c) {
				TreeNode[] nodes = new TreeNode[c.paths.Count];
				int count = 0;
				foreach (InteractionPath  ic in c.paths) {
					List<TreeNode> subNodes = new List<TreeNode>();
					foreach (InteractionBase b in ic.items) {
						if (b is InteractionChoice ch) {
							TreeNode tn = new TreeNode(ch.ToString(), GetTreeArray(ch));
							subNodes.Add(tn);
							nodeBase.Add(tn, ch);
						} else {
							TreeNode tn = new TreeNode(b.ToString());
							subNodes.Add(tn);
							nodeBase.Add(tn, b);
						}
					}
					TreeNode choi = new TreeNode(ic.ToString(), subNodes.ToArray());
					nodes[count++] = choi;
					nodeBase.Add(choi, ic);
				}
				return nodes;
			}

			public static void UpdateNodeData(object sender, TreeNodeMouseClickEventArgs e) {
				if (e.Button != MouseButtons.Left)
					return;
				if (currentObject != null)
					SaveInteractionData();
				currentObject = nodeBase[e.Node];
				displayTree.SelectedNode = e.Node;
				SetupInteractionData();
			}

			public static void DeleteNodeData() {
				if (displayTree.SelectedNode == null)
					return;
				if (displayTree.SelectedNode.Parent != null) {
					if (nodeBase[displayTree.SelectedNode.Parent].GetType() == typeof(InteractionChoice)) {
						((InteractionChoice)nodeBase[displayTree.SelectedNode.Parent]).paths.Remove((InteractionPath)nodeBase[displayTree.SelectedNode]);
					} else if (nodeBase[displayTree.SelectedNode.Parent].GetType() == typeof(InteractionPath)) {
						((InteractionPath)nodeBase[displayTree.SelectedNode.Parent]).items.Remove(nodeBase[displayTree.SelectedNode]);
					}
				} else {
					interactedObject.InterItems.Remove(nodeBase[displayTree.SelectedNode]);
				}
				displayTree.Nodes.Remove(displayTree.SelectedNode);
				nodeBase.Remove(displayTree.SelectedNode);
			}

			public static TextBox[] pars;
			public static Type tempType;
			//Creates a new object from type given and adds it to the tree
			public static void SetupNewData(Type nt) {
				InteractionBase news = new InteractionDialogue("");
				if (nt == typeof(InteractionQuests)) {
					news = new InteractionQuests(new List<InteractionPath>() { });
				} else if (nt.Equals(typeof(InteractionDialogue))) {
					news = new InteractionDialogue("");
				} else if (nt.Equals(typeof(InteractionChoice))) {
					news = new InteractionChoice(new List<InteractionPath>() { });
				} else if (nt.Equals(typeof(InteractionPath))) {
					news = new InteractionPath();
				}
				TreeNode nod = new TreeNode(news.ToString());
				if (displayTree.SelectedNode != null) {
					if (nodeBase[displayTree.SelectedNode].GetType() == typeof(InteractionPath)) {
						//If the node is a path, then add the item to that path
						((InteractionChoice)nodeBase[displayTree.SelectedNode.Parent]).AddBase(displayTree.SelectedNode.Text, news);
						displayTree.SelectedNode.Nodes.Add(nod);
						nodeBase.Add(nod, news);
					} else if (nodeBase[displayTree.SelectedNode].GetType() == typeof(InteractionChoice)){
						((InteractionChoice)nodeBase[displayTree.SelectedNode]).AddChoice(pars[0].Text);
						nod.Text = pars[0].Text;
						displayTree.SelectedNode.Nodes.Add(nod);
						nodeBase.Add(nod, news);
						pars = null;
					}
				} else {
					//if nothing selected, then the primary level
					interactedObject.InterItems.Add(news);
					nodeBase.Add(nod, news);
					displayTree.Nodes.Add(nod);
				}
			}

			public static void ShiftNode(int dir) {
				if (displayTree.SelectedNode == null)
					return;

				if (displayTree.SelectedNode.Parent == null) {
					//If on root
					int ind = interactedObject.InterItems.IndexOf(nodeBase[displayTree.SelectedNode]);
					if (ind - dir < 0 || ind - dir >= interactedObject.InterItems.Count) {
						displayTree.Focus();
						return;
					}
					interactedObject.InterItems.RemoveAt(ind);
					interactedObject.InterItems.Insert(ind - dir, nodeBase[displayTree.SelectedNode]);
					TreeNode n = displayTree.SelectedNode;
					int ind2 = displayTree.Nodes.IndexOf(displayTree.SelectedNode);
					displayTree.Nodes.Remove(n);
					displayTree.Nodes.Insert(ind2 - dir, n);
					displayTree.SelectedNode = n;
					displayTree.Focus();
				} else
				//If inside a path
				if (nodeBase[displayTree.SelectedNode.Parent].GetType() == typeof(InteractionPath)) {
					InteractionPath pathPar = ((InteractionPath)nodeBase[displayTree.SelectedNode.Parent]);
					int ind = pathPar.items.IndexOf(nodeBase[displayTree.SelectedNode]);
					if (ind - dir < 0 || ind - dir >= pathPar.items.Count) {
						displayTree.Focus();
						return;
					}
					pathPar.items.RemoveAt(ind);
					pathPar.items.Insert(ind - dir, nodeBase[displayTree.SelectedNode]);
					TreeNode n = displayTree.SelectedNode;
					int ind2 = displayTree.SelectedNode.Parent.Nodes.IndexOf(displayTree.SelectedNode);
					displayTree.SelectedNode.Parent.Nodes.Remove(n);
					displayTree.SelectedNode.Parent.Nodes.Insert(ind2 - dir, n);
					displayTree.SelectedNode = n;
					displayTree.Focus();
				} else 
				//If the node is a path
				if (nodeBase[displayTree.SelectedNode].GetType() == typeof(InteractionPath)) {
					InteractionChoice c = (InteractionChoice)nodeBase[displayTree.SelectedNode.Parent];
					int ind = c.paths.IndexOf((InteractionPath)nodeBase[displayTree.SelectedNode]);
					if (ind - dir < 0 || ind - dir >= c.paths.Count) {
						displayTree.Focus();
						return;
					}
					c.paths.RemoveAt(ind);
					c.paths.Insert(ind - dir, (InteractionPath)nodeBase[displayTree.SelectedNode]);
					TreeNode n = displayTree.SelectedNode;
					int ind2 = displayTree.SelectedNode.Parent.Nodes.IndexOf(displayTree.SelectedNode);
					displayTree.SelectedNode.Parent.Nodes.Remove(n);
					displayTree.SelectedNode.Parent.Nodes.Insert(ind2 - dir, n);
					displayTree.SelectedNode = n;
					displayTree.Focus();
				}
				SetupInteractionData();
			}

			private static void SetupInteractionData() {
				text.Visible = false;
				save.Visible = false;
				group.Visible = false;
				group.Controls.Clear();
				toolButton.DropDown.Items[0].Visible = false;
				toolButton.DropDown.Items[2].Visible = false;
				toolButton.DropDown.Items[3].Visible = false;
				if (currentObject.GetType() == typeof(InteractionChoice)) {
					toolButton.DropDown.Items[3].Visible = true;

				} else if (currentObject.GetType() == typeof(InteractionDialogue)) {
					text.Visible = true;
					text.Text = ((InteractionDialogue)currentObject).displayText;
					save.Visible = true;
				} else if (currentObject.GetType() == typeof(InteractionPath)) {
					toolButton.DropDown.Items[0].Visible = true;
					toolButton.DropDown.Items[2].Visible = true;
					group.Visible = true;
					save.Visible = true;
					//If under a quest, set quest flags and stuff
					if (nodeBase[displayTree.SelectedNode.Parent].GetType() == typeof(InteractionQuests)) {
						pars = new TextBox[2];
						TextBox val = new TextBox() {
							Location = new Point(0, 35),
							Size = new Size(200, 20),
							Text = displayTree.SelectedNode.Text
						};
						group.Controls.Add(new Label() { Text = "Choice Name: ", Location = new Point(0, 15), Size = new Size(200, 20) });
						group.Controls.Add(val);
						TextBox vall = new TextBox() {
							Location = new Point(0, 75),
							Size = new Size(200, 20),
							Text = "null"
						};
						group.Controls.Add(new Label() { Text = "Quest Flag: ", Location = new Point(0, 55), Size = new Size(200, 20) });
						group.Controls.Add(vall);
						pars[0] = val;
						pars[1] = vall;
					} else {
						pars = new TextBox[1];
						TextBox val = new TextBox() {
							Location = new Point(0, 35),
							Size = new Size(200, 20),
							Text = displayTree.SelectedNode.Text
						};
						group.Controls.Add(new Label() { Text = "Choice Name: ", Location = new Point(0, 15), Size = new Size(200, 20) });
						group.Controls.Add(val);
						pars[0] = val;
					}
				}
			}

			public static void SaveInteractionData() {
				if (currentObject.GetType().Equals(typeof(InteractionQuests))) {

				} else if (currentObject.GetType().Equals(typeof(InteractionChoice))) {
					bool valsSet = true;
					foreach (Control c in group.Controls) {
						if (c is TextBox t)
							if (t.TextLength == 0)
								valsSet = false;
					}
					if (valsSet) {

					}
				} else if (currentObject.GetType().Equals(typeof(InteractionDialogue))) {
					text.Visible = false;
					save.Visible = false;
					((InteractionDialogue)currentObject).displayText = text.Text;
					System.Diagnostics.Debug.WriteLine(text.Text);
				}
				toolButton.DropDown.Items[0].Visible = false;
				toolButton.DropDown.Items[2].Visible = false;
				toolButton.DropDown.Items[3].Visible = false;
				currentObject.ModificationAction();
			}
		}

		private void SaveButton_Click(object sender, EventArgs e) {
			Interaction.SaveInteractionData();
			SaveData.SaveGame();
		}

		private void Save_Click(object sender, EventArgs e) {
			Interaction.SaveInteractionData();
		}

		private void ChoiceToolStripMenuItem_Click(object sender, EventArgs e) {
			Interaction.SetupNewData(typeof(InteractionChoice));
			/*
			
			*/
		}

		private void DialogueToolStripMenuItem_Click(object sender, EventArgs e) {
			Interaction.SetupNewData(typeof(InteractionDialogue));
		}

		private void QuestToolStripMenuItem_Click(object sender, EventArgs e) {
			Interaction.SetupNewData(typeof(InteractionQuests));
		}
		private void PathToolStripMenuItem_Click(object sender, EventArgs e) {
			Interaction.tempType = typeof(InteractionPath);
			applyButt.Visible = true;
			groupBox.Visible = true;
			Interaction.pars = new TextBox[1];
			TextBox val = new TextBox() {
				Location = new Point(0, 35),
				Size = new Size(200, 20)
			};
			Interaction.group.Controls.Add(new Label() { Text = "Path Name: ", Location = new Point(0, 15), Size = new Size(200, 20) });
			Interaction.group.Controls.Add(val);
			Interaction.pars[0] = val;
		}

		private void ApplyButt_Click(object sender, EventArgs e) {
			bool valsSet = true;
			foreach (Control c in groupBox.Controls) {
				if (c is TextBox t)
					if (t.TextLength == 0)
						valsSet = false;
			}
			if (Interaction.tempType != null && valsSet) {
				Interaction.SetupNewData(Interaction.tempType);
				applyButt.Visible = false;
				groupBox.Visible = false;
				Interaction.group.Controls.Clear();
				Interaction.pars = null;
			}
		}

		private void DelButt_Click(object sender, EventArgs e) {
			Interaction.DeleteNodeData();
		}

		private void UpdateButton_Click(object sender, EventArgs e) {
			worldXLabel.Text = "World X: " + WorldData.CurrentX;
			worldYLabel.Text = "World Y: " + WorldData.CurrentY;
		}

		private void UpButton_Click(object sender, EventArgs e) {
			Interaction.ShiftNode(1);
		}

		private void DownButton_Click(object sender, EventArgs e) {
			Interaction.ShiftNode(-1);
		}
	}
}
