using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using _2DRPG.Save;

namespace _2DRPG.World.Objects {
	[Serializable]
	public class WorldObjectBase : TexturedObject {
		[Editable]
		public float worldX;
		[Editable]
		public float worldY;

		/// <summary>
		/// Complete Declaration for WorldObjectBase
		/// </summary>
		/// <param name="x">X position in the world</param>
		/// <param name="y">Y position in the world</param>
		/// <param name="layer">Render Layer</param>
		/// <param name="textureName">Name of the texture</param>
		public WorldObjectBase(float x, float y, int layer, Texture textureName, float width = 16, float height = 16) : this(x, y, textureName, width, height) {
			SetLayer(layer);
		}

		public WorldObjectBase() : base() {
			UpdateWorldPosition();
		}
		public WorldObjectBase(Texture textureName) : base(textureName) {

        }
		public WorldObjectBase(float x, float y, Texture textureName, float width = 16, float height = 16) : base(textureName) {
			this.width = width;
			this.height = height;
			SetWorldPosition(x, y);
		}

		/// <summary>
		/// Sets the Guid of the object, generating if passed a null Guid
		/// </summary>
		/// <param name="ud"></param>
		public void SetUID(Guid ud) {
			if (ud == null || ud == Guid.Empty)
				uid = Guid.NewGuid();
			else
				uid = ud;
		}
	
		/// <summary>
		/// Sets the position of the object in the world
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void SetWorldPosition(float x, float y) {
			worldX = x;
			worldY = y;
			UpdateWorldPosition();
		}
		/// <summary>
		/// Adjusts the dimensions of the world object
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public override void SetScreenDimensions(float width, float height) {
			this.width = width;
			this.height = height;
			UpdateWorldPosition();
		}
		/// <summary>
		/// Returns the location of the object as a Point
		/// </summary>
		/// <returns></returns>
		public System.Drawing.Point GetPointLocation() {
			return new System.Drawing.Point((int)worldX, (int)worldY);
		}

		/// <summary>
		/// Redraws the vertices of the object using the world coordinates and dimensions
		/// Call when the WorldX, WorldY, height, or width are updated outside of SetWorldPosition()
		/// </summary>
		public void UpdateWorldPosition() {
			quadPosition[0] = worldX - width / 2;
			quadPosition[3] = worldX - width / 2;
			quadPosition[6] = worldX + width / 2;
			quadPosition[9] = worldX + width / 2;
			quadPosition[1] = worldY - height / 2;
			quadPosition[10] = worldY - height / 2;
			quadPosition[4] = worldY + height / 2;
			quadPosition[7] = worldY + height / 2;
			SendPacket();
		}

		public static void ShiftPosition(float[] quad, float x, float y) {
			quad[0] += x;
			quad[3] += x;
			quad[6] += x;
			quad[9] += x;
			quad[1] += y;
			quad[4] += y;
			quad[7] += y;
			quad[10] += y;
		}

		protected void SendPacket() {
			if (SaveData.GameSettings.coOp && Net.SessionManager.isHost)     //Send update packets
				Net.SessionManager.SendFrame(new Net.UDPFrame() {
					subject = Net.UDPFrame.FrameType.Movement,
					uid = uid,
					//stringData = new string[] { WorldData.GetRegionString(worldX, worldY) },
					floatData = new float[] { worldX, worldY }
				});
		}

		/// <summary>
		/// Checks if the coordinates given are within the world object, sets up the worldBuilder window
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public bool CheckCoords(float x, float y){
			bool b = LogicUtils.Logic.CheckIntersection(quadPosition, x, y);
			if (b)
				GUI.Windows.BuilderWindow.SetCurrentObject(this);
			return b;
		}

		public override string ToString() {
			return GetType().Name + "- Coords: " + worldX + "," + worldY;
		}

		/// <summary>
		/// To be called whenever values are directly modified outside of standard functions
		/// </summary>
		public virtual void ModificationAction() {
			UpdateWorldPosition();
			SetLayer(layer);
			if (this is WorldObjectControllable)
				return;
			//Moves the world object to the correct region if it's misplaced
			Regions.RegionBase bs = WorldData.GetRegion(worldX, worldY);
			if (!bs.GetWorldObjects().Contains(this)) {
				lock (WorldData.currentRegions) {
					foreach (Regions.RegionBase hsh in WorldData.currentRegions.Values)
						if (hsh.GetWorldObjects().Contains(this))
							hsh.GetWorldObjects().Remove(this);
					bs.GetWorldObjects().Add(this);
				}
			}
		}
		/// <summary>
		/// Adds all necessary data in order to transmit the worldobject over UDP Packets
		/// See table in WorldObjectBase for the integers for each WorldObject
		/// </summary>
		/// <returns></returns>
		public virtual Net.UDPFrame[] ToPacket() {
			return new Net.UDPFrame[]{
				new Net.UDPFrame() {
					subject = Net.UDPFrame.FrameType.WorldObject,
					floatData = new float[] { worldX, worldY, width, height},
					intData = new int[] { 0, 0, layer}, //[Object Type, multipacket system (0/1), extra data]
					//stringData = new string[]{ WorldData.GetRegionString(worldX, worldY) },
					uid = uid
				}
			};
		}


		/*		Table of World Objects by integer for use in UDP Packets
		 *	0: WorldObjectBase
		 *	1: WorldObjectAnimated
		 *	2: WorldObjectCollidable
		 *  3: WorldObjectControllable
		 *  4: WorldObjectMovable
		 *  5: WorldObjectMovableAnimated
		 *  6: WorldObjectSimpleItem
		 *  7: WorldObjectDialogue
		 *  8: WorldObjectInventory
		*/
	}
}
