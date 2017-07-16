﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2DRPG.GUI.Interaction;
using Newtonsoft.Json.Linq;

namespace _2DRPG.World.Objects {
	public class WorldObjectInteractable : WorldObjectBase, IInteractable {

		public Action interAction;

		public string InteractionTag = "";

		public List<InteractionBase> InterItems = new List<InteractionBase>();

		/// <summary>
		/// Complete Declaration for WorldObjectInteractable
		/// </summary>
		/// <param name="x">X position in the world</param>
		/// <param name="y">Y position in the world</param>
		/// <param name="layer">Render Layer</param>
		/// <param name="textureName">Name of the texture</param>
		public WorldObjectInteractable(float x, float y, int layer, string textureName = "default", float width = 16, float height = 16) : base(x, y, layer, textureName, width, height) {
			interAction = OpenDialogue;
		}
		public WorldObjectInteractable(GameSave.WorldObjectStorage store) : this(store.worldX, store.worldY, store.layer, store.textureName, store.width, store.height) {
			interAction = OpenDialogue;
			InteractionTag = (string)store.extraData[0];
			List<GameSave.InteractionObjectStorage> st = ((JArray)store.extraData[1]).ToObject<List<GameSave.InteractionObjectStorage>>();
			foreach (GameSave.InteractionObjectStorage s in st)
				InterItems.Add(GameSave.ConstructInteractionObject(s));
		}

		void OpenDialogue() {
			if (Screen.currentWindows.ContainsKey("interaction")) {
				Screen.CloseWindow("interaction");
				return;
			}
			/*
			InterItems = new List<InteractionBase>(){
			new InteractionDialogue("Plenty 'O Peanuts, Eh?"),
			new InteractionDialogue("So, what can I help you with?"),
			new InteractionChoice(new Dictionary<string, List<InteractionBase>>{
				{ "choice A", new List<InteractionBase> {
					new InteractionDialogue("Blah Blah Blah"),
					new InteractionDialogue("Ho Hum, fiddly dee")} }, 
				{ "choice B", new List<InteractionBase>{
					new InteractionDialogue("Interesting, but I'm afraid I can't help you"),
					new InteractionDialogue("What you want is a fresh start on life")
				} }
			})
			};
			*/
			GUI.Windows.InteractionWindow.SetInteractionElements(InterItems);
			if (SaveData.GameSettings.interactionEditor) {
				DevWindow.Interaction.interactedObject = this;
				DevWindow.Interaction.SetupTree();
			}
			Screen.OpenWindow("interaction");
		}

		public void Interact() {
			if (interAction != null)
				interAction.Invoke();
		}

		public override GameSave.WorldObjectStorage StoreObject() {
			List<GameSave.InteractionObjectStorage> st = new List<GameSave.InteractionObjectStorage>();
			foreach (InteractionBase b in InterItems) {
				st.Add(b.StoreObject());
			}
			GameSave.WorldObjectStorage store = new GameSave.WorldObjectStorage() {
				worldX = worldX, worldY = worldY, width = width, height = height, textureName = texName, layer = layer, objectType = GameSave.WorldObjectType.Interactable, extraData = new object[] { InteractionTag, st}
			};
			return store;
		}
	}
}
