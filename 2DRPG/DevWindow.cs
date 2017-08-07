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
using _2DRPG.Quests;

namespace _2DRPG {
	public partial class DevWindow : Form {
		public DevWindow() {
			InitializeComponent();

			//Interaction Tab
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
			treeView.HideSelection = false;

			//Quest Tab
			Quest.list = listBox1;
			Quest.items = panel2;
			Quest.grp = groupBox1;
			Quest.active = checkBox1;
			Quest.completed = checkBox2;
			Quest.addButt = button4;
			Quest.active.Visible = false;
			Quest.completed.Visible = false;
			Quest.grp.Visible = false;
			Quest.items.Visible = false;
			Quest.addButt.Visible = false;
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
				currentObject = null;
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
				if (e.Button != MouseButtons.Left || !nodeBase.ContainsKey(e.Node))
					return;
				if (currentObject != null && nodeBase.ContainsValue(currentObject))
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
					} else if (nodeBase[displayTree.SelectedNode.Parent].GetType() == typeof(InteractionQuests)) {
						((InteractionQuests)nodeBase[displayTree.SelectedNode.Parent]).paths.Remove((InteractionPath)nodeBase[displayTree.SelectedNode]);
					}
				} else {
					interactedObject.InterItems.Remove(nodeBase[displayTree.SelectedNode]);
				}
				nodeBase.Remove(displayTree.SelectedNode);
				displayTree.Nodes.Remove(displayTree.SelectedNode);
				SetupInteractionData();
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
						displayTree.SelectedNode.Expand();
						nodeBase.Add(nod, news);
					} else if (nodeBase[displayTree.SelectedNode].GetType() == typeof(InteractionChoice)) {
						((InteractionChoice)nodeBase[displayTree.SelectedNode]).AddChoice(pars[0].Text);
						nod.Text = pars[0].Text;
						displayTree.SelectedNode.Nodes.Add(nod);
						displayTree.SelectedNode.Expand();
						nodeBase.Add(nod, news);
						pars = null;
					} else if (nodeBase[displayTree.SelectedNode].GetType() == typeof(InteractionQuests)) {
						((InteractionQuests)nodeBase[displayTree.SelectedNode]).AddChoice(pars[0].Text);
						((InteractionQuests)nodeBase[displayTree.SelectedNode]).questTags.Add("");
						nod.Text = pars[0].Text;
						displayTree.SelectedNode.Nodes.Add(nod);
						displayTree.SelectedNode.Expand();
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

			private static int tmpInt;
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
				} else if (currentObject.GetType() == typeof(InteractionQuests)) {
					toolButton.DropDown.Items[3].Visible = true;
					group.Visible = true;
					CheckBox bux = new CheckBox() {
						Location = new Point(5, 15),
						Text = "Immediate Mode",
						Checked = ((InteractionQuests)currentObject).immediateMode
					};
					bux.CheckedChanged += new EventHandler((object sender, EventArgs e) => {
						((InteractionQuests)currentObject).immediateMode = bux.Checked;
					});
					group.Controls.Add(bux);
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
						InteractionQuests que = (InteractionQuests)nodeBase[displayTree.SelectedNode.Parent];
						tmpInt = que.paths.IndexOf((InteractionPath)currentObject);
						pars = new TextBox[3];
						TextBox val = new TextBox() {
							Location = new Point(0, 35),
							Size = new Size(200, 20),
							Text = displayTree.SelectedNode.Text
						};
						group.Controls.Add(new Label() { Text = "Path Name: ", Location = new Point(0, 15), Size = new Size(200, 20) });
						group.Controls.Add(val);
						TextBox vall = new TextBox() {
							Location = new Point(0, 75),
							Size = new Size(200, 20),
							Text = (que.questTags.Count > tmpInt && tmpInt != -1) ? que.questTags[tmpInt] : ""
						};
						group.Controls.Add(new Label() { Text = "Quest Name: ", Location = new Point(0, 55), Size = new Size(200, 20) });
						group.Controls.Add(vall);
						TextBox valll = new TextBox() {
							Location = new Point(0, 115),
							Size = new Size(200, 20),
							Text = (que.questInts.Count > tmpInt && tmpInt != -1) ? que.questInts[tmpInt].ToString() : "0"
						};
						group.Controls.Add(new Label() { Text = "Checked Int: ", Location = new Point(0, 95), Size = new Size(200, 20) });
						group.Controls.Add(valll);
						pars[0] = val;
						pars[1] = vall;
						pars[2] = valll;
					} else {
						pars = new TextBox[1];
						TextBox val = new TextBox() {
							Location = new Point(0, 35),
							Size = new Size(200, 20),
							Text = displayTree.SelectedNode.Text
						};
						group.Controls.Add(new Label() { Text = "Path Name: ", Location = new Point(0, 15), Size = new Size(200, 20) });
						group.Controls.Add(val);
						pars[0] = val;
					}
				}
			}

			public static void SaveInteractionData() {
				if (currentObject.GetType() == typeof(InteractionQuests)) {
					
				} else if (currentObject.GetType() == typeof(InteractionDialogue)) {
					text.Visible = false;
					save.Visible = false;
					((InteractionDialogue)currentObject).displayText = text.Text;
				} else if (currentObject.GetType() == typeof(InteractionPath)) {
					if (pars[0].Text.Length != 0) {
						((InteractionPath)currentObject).pathName = pars[0].Text;
						displayTree.SelectedNode.Text = pars[0].Text;
					}
					if (nodeBase[displayTree.SelectedNode.Parent].GetType() == typeof(InteractionQuests)) {
						InteractionQuests que = (InteractionQuests)nodeBase[displayTree.SelectedNode.Parent];
						if (tmpInt != -1) {
							if (que.questTags.Count > tmpInt)
								que.questTags[tmpInt] = pars[1].Text;
							if (que.questInts.Count > tmpInt) {
								if (!int.TryParse(pars[2].Text, out int vbs))
									vbs = 0;
								que.questInts[tmpInt] = vbs;
							}
						}
						
					}
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
			Interaction.group.Controls.Clear();
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
			treeView.Focus();
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

		public static class Quest {
			public static ListBox list;
			public static GroupBox grp;
			public static Panel items;
			public static CheckBox active;
			public static CheckBox completed;
			public static Button addButt;

			public static IQuest selectedQuest;

			public static void UpdateQuests() {
				list.Items.Clear();
				foreach (KeyValuePair<string, IQuest> q in QuestData.QuestDatabase) {
					list.Items.Add((QuestData.ActiveQuests.Contains(q.Key) ? "*" : "") + q.Key);
				}
			}

			public static TextBox[] pars;
			public static List<TextBox> itemPars;

			public static void UpdateSelected() {
				grp.Visible = false;
				active.Visible = false;
				completed.Visible = false;
				items.Visible = false;
				addButt.Visible = false;
				if (list.SelectedItem == null || !QuestData.QuestDatabase.ContainsKey(((string)list.SelectedItem).Replace("*", "")))
					return;
				selectedQuest = QuestData.QuestDatabase[((string)list.SelectedItem).Replace("*", "")];
				grp.Visible = true;
				active.Visible = true;
				completed.Visible = true;
				completed.Checked = selectedQuest.Completed;
				if (QuestData.ActiveQuests.Contains(selectedQuest.ToString()))
					active.Checked = true;
				if (selectedQuest.GetType() == typeof(TaskBase)) {
					TaskBase bas = (TaskBase)selectedQuest;
					grp.Controls.Clear();
					items.Visible = true;
					addButt.Visible = true;
					pars = new TextBox[1];
					TextBox val = new TextBox() {
						Location = new Point(0, 35),
						Size = new Size(200, 20)
					};
					val.Text = bas.taskName;
					grp.Controls.Add(new Label() { Text = "Task Name (Requires Restart): ", Location = new Point(0, 15), Size = new Size(200, 20) });
					grp.Controls.Add(val);
					pars[0] = val;

					itemPars = new List<TextBox>();
					items.Controls.Clear();
					int counter = 0;
					foreach (ItemPickup p in bas.taskItems) {
						TextBox vab = new TextBox() {
							Location = new Point(0, 35 + 10 * counter),
							Size = new Size(100, 20)
						};
						vab.Text = p.itemName;
						TextBox vad = new TextBox() {
							Location = new Point(100, 35 + 10 * counter),
							Size = new Size(100, 20)
						};
						vad.Text = p.itemQuantity.ToString();
						items.Controls.Add(vab);
						items.Controls.Add(vad);
						counter += 2;
						itemPars.Add(vab);
						itemPars.Add(vad);
					}
					items.Controls.Add(new Label() { Text = "Item Name: ", Location = new Point(10, 15), Size = new Size(90, 20) });
					items.Controls.Add(new Label() { Text = "Item Quantity: ", Location = new Point(110, 15), Size = new Size(90, 20) });
				}


			}

			public static void SaveSelected() {
				if (selectedQuest == null || pars.Length == 0)
					return;
				if (selectedQuest.GetType() == typeof(TaskBase)) {
					TaskBase tmp = (TaskBase)selectedQuest;
					tmp.taskName = pars[0].Text;
					tmp.taskItems.Clear();
					for (int i = 0; i < itemPars.Count; i += 2) {
						if (itemPars[i].Text.Length > 0)
							tmp.taskItems.Add(new ItemPickup { itemName = itemPars[i].Text, itemQuantity = int.Parse(itemPars[i + 1].Text) });
					}
				}
			}
		}

		private void IndexChanged(object sender, EventArgs e) {
			Quest.UpdateSelected();
		}

		private void UpdateQuestClick(object sender, EventArgs e) {
			Quest.UpdateQuests();
		}

		private void ActiveCheckChanged(object sender, EventArgs e) {
			QuestData.SetQuestActive(Quest.selectedQuest.ToString(), Quest.active.Checked);
			Quest.UpdateQuests();
		}

		private void SaveClick(object sender, EventArgs e) {
			Quest.SaveSelected();
		}

		private void CompletedCheckChanged(object sender, EventArgs e) {
			Quest.selectedQuest.Completed = Quest.completed.Checked;
		}

		private void SaveGameClick(object sender, EventArgs e) {
			Quest.SaveSelected();
			SaveData.SaveGame();
		}

		private void AddItemClick(object sender, EventArgs e) {
			TextBox vab = new TextBox() {
				Location = new Point(0, 35 + 10 * Quest.itemPars.Count),
				Size = new Size(100, 20)
			};
			TextBox vad = new TextBox() {
				Location = new Point(100, 35 + 10 * Quest.itemPars.Count),
				Size = new Size(100, 20)
			};
			Quest.items.Controls.Add(vab);
			Quest.items.Controls.Add(vad);
			Quest.itemPars.Add(vab);
			Quest.itemPars.Add(vad);
		}
	}
}
