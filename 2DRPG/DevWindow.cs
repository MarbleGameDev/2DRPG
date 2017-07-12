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
			TextBox.Visible = false;
			save.Visible = false;
			applyButt.Visible = false;
			groupBox.Visible = false;
		}


		public static class Interaction {
			public static World.Objects.WorldObjectInteractable interactedObject;
			public static TreeView displayTree;
			public static RichTextBox text;
			public static GroupBox group;
			public static Button save;
			static InteractionBase currentObject;

			static Dictionary<TreeNode, InteractionBase> nodeBase = new Dictionary<TreeNode, InteractionBase>();
			static HashSet<TreeNode> choices = new HashSet<TreeNode>();

			public static void SetupTree() {
				if (displayTree == null)
					return;
				displayTree.Nodes.Clear();
				nodeBase.Clear();
				choices.Clear();
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
				TreeNode[] nodes = new TreeNode[c.choices.Count];
				int count = 0;
				foreach (KeyValuePair<string, List<InteractionBase>> keys in c.choices) {
					List<TreeNode> subNodes = new List<TreeNode>();
					foreach (InteractionBase b in keys.Value) {
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
					TreeNode choi = new TreeNode(keys.Key, subNodes.ToArray());
					nodes[count++] = choi;
					choices.Add(choi);
				}
				return nodes.ToArray();
			}

			public static void UpdateNodeData(object sender, TreeNodeMouseClickEventArgs e) {
				if (e.Button != MouseButtons.Left)
					return;
				if (currentObject != null)
					SaveInteractionData();
				if (!nodeBase.ContainsKey(e.Node))
					return;
				currentObject = nodeBase[e.Node];
				SetupInteractionData();
			}

			public static void DeleteNodeData() {
				if (displayTree.SelectedNode == null)
					return;
				if (displayTree.SelectedNode.Parent != null) {
					//If the node is a choice path, remove it from the choice
					if (choices.Contains(displayTree.SelectedNode)) {
						((InteractionChoice)nodeBase[displayTree.SelectedNode.Parent]).choices.Remove(displayTree.SelectedNode.Text);
					} else if (choices.Contains(displayTree.SelectedNode.Parent)){
						//If the node is inside a choice path, remove it from the path in the choice
						((InteractionChoice)nodeBase[displayTree.SelectedNode.Parent.Parent]).choices[displayTree.SelectedNode.Parent.Text].Remove(nodeBase[displayTree.SelectedNode]);
					}
				} else {
					interactedObject.InterItems.Remove(nodeBase[displayTree.SelectedNode]);
				}
				displayTree.Nodes.Remove(displayTree.SelectedNode);
			}

			public static TextBox[] pars;
			public static Type tempType;
			public static void SetupNewData(Type nt) {
				InteractionBase news = new InteractionDialogue("");
				if (nt.Equals(typeof(InteractionChoice))) {
					news = new InteractionChoice(new Dictionary<string, List<InteractionBase>>() { });
				} else if (nt.Equals(typeof(InteractionDialogue))) {
					news = new InteractionDialogue("");
				} else if (nt.Equals(typeof(InteractionQuests))) {
					//news = new InteractionQuests();
				}
				TreeNode nod = new TreeNode(news.ToString());
				if (displayTree.SelectedNode != null) {
					if (choices.Contains(displayTree.SelectedNode)) {
						//If the node is a choice option, then add the item to that option path
						((InteractionChoice)nodeBase[displayTree.SelectedNode.Parent]).AddBase(displayTree.SelectedNode.Text, news);
						displayTree.SelectedNode.Nodes.Add(nod);
						nodeBase.Add(nod, news);
					} else {
						//Choice added to a choice makes a new option, not a new node/Interaction
						if (nodeBase[displayTree.SelectedNode].GetType().Equals(typeof(InteractionChoice)) && nt.Equals(typeof(InteractionChoice))) {
							((InteractionChoice)nodeBase[displayTree.SelectedNode]).AddChoice(pars[0].Text);
							nod = new TreeNode(pars[0].Text);
							displayTree.SelectedNode.Nodes.Add(nod);
							choices.Add(nod);
						} else {
							//Default to the primary level
							interactedObject.InterItems.Add(news);
							displayTree.Nodes.Add(nod);
							nodeBase.Add(nod, news);
						}
					}
				} else {
					//if nothing selected, then the primary level
					interactedObject.InterItems.Add(news);
					nodeBase.Add(nod, news);
					displayTree.Nodes.Add(nod);
				}
			}

			private static void SetupInteractionData() {
				text.Visible = false;
				save.Visible = false;
				if (currentObject.GetType().Equals(typeof(InteractionChoice))) {
					
				} else if (currentObject.GetType().Equals(typeof(InteractionDialogue))) {
					text.Visible = true;
					text.Text = ((InteractionDialogue)currentObject).displayText;
					save.Visible = true;
				} else if (currentObject.GetType().Equals(typeof(InteractionQuests))) {

				}
			}

			public static void SaveInteractionData() {
				if (currentObject.GetType().Equals(typeof(InteractionChoice))) {

				} else if (currentObject.GetType().Equals(typeof(InteractionDialogue))) {
					text.Visible = false;
					((InteractionDialogue)currentObject).displayText = text.Text;
				} else if (currentObject.GetType().Equals(typeof(InteractionQuests))) {

				}
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
			Interaction.tempType = typeof(InteractionChoice);
			applyButt.Visible = true;
			groupBox.Visible = true;
			Interaction.pars = new TextBox[1];
			TextBox val = new TextBox() {
				Location = new Point(0, 35),
				Size = new Size(200, 20)
			};
			Interaction.group.Controls.Add(new Label() { Text = "Choice Name: ", Location = new Point(0, 15), Size = new Size(200, 20) });
			Interaction.group.Controls.Add(val);
			Interaction.pars[0] = val;
		}

		private void DialogueToolStripMenuItem_Click(object sender, EventArgs e) {
			Interaction.SetupNewData(typeof(InteractionDialogue));
		}

		private void QuestToolStripMenuItem_Click(object sender, EventArgs e) {
			Interaction.SetupNewData(typeof(InteractionQuests));
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
	}
}
