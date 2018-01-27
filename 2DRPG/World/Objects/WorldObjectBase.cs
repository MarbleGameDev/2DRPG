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
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="layer"></param>
		/// <param name="textureName"></param>
		/// <param name="width1"></param>
		/// <param name="height1"></param>
		/// <param name="width2"></param>
		/// <param name="height2"></param>
		/// <param name="width3"></param>
		/// <param name="height3"></param>
		/// <param name="width4"></param>
		/// <param name="height4"></param>
		public WorldObjectBase(float x, float y, int layer, Texture textureName, float width1 = 8, float height1 = 8, float width2 = 8, float height2 = 8, float width3 = 8, float height3 = 8, float width4 = 8, float height4 = 8) : this(x, y, textureName, width1, height1, width2, height2, width3, height3, width4, height4) {
			SetLayer(layer);
		}

		public WorldObjectBase() : base() {
			UpdateQuadPosition();
		}
		public WorldObjectBase(Texture textureName) : base(textureName) {

        }
		public WorldObjectBase(float x, float y, Texture textureName, float width1 = 8, float height1 = 8, float width2 = 8, float height2 = 8, float width3 = 8, float height3 = 8, float width4 = 8, float height4 = 8) : base(textureName) {
			this.width1 = width1;
			this.height1 = height1;
			this.width2 = width2;
			this.height2 = height2;
			this.width3 = width3;
			this.height3 = height3;
			this.width4 = width4;
			this.height4 = height4;
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
			UpdateQuadPosition();
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
		/// Call when the WorldX, WorldY, heights, or widths are updated outside of SetWorldPosition()
		/// </summary>
		protected override void UpdateQuad() {
			quadPosition[0] = worldX - width3;
			quadPosition[1] = worldY - height3;
			quadPosition[3] = worldX - width2;
			quadPosition[4] = worldY + height2;
			quadPosition[6] = worldX + width1;
			quadPosition[7] = worldY + height1;
			quadPosition[9] = worldX + width4;
			quadPosition[10] = worldY - height4;
			if (automaticTextureMapping)
				ResetTexturePosition();
			SendPacket();
		}

		protected override void Rotate(float angle) {
			//UpdateQuad();
			float anglRad = angle * (float)(Math.PI / 180);
			//Working counterclockwise through quadrants
			float currentang = (float)Math.Atan(height1 / width1) + anglRad;    //Fundamental angle of the vertex plus the current rotation
			float hyp = (float)Math.Sqrt(height1 * height1 + width1 * width1);
			quadPosition[6] = worldX + hyp * (float)Math.Cos(currentang);
			quadPosition[7] = worldY + hyp * (float)Math.Sin(currentang);

			currentang = (float)Math.Atan(height2 / -width2) + (float)Math.PI + anglRad;
			hyp = (float)Math.Sqrt(height2 * height2 + width2 * width2);
			quadPosition[3] = worldX + hyp * (float)Math.Cos(currentang);
			quadPosition[4] = worldY + hyp * (float)Math.Sin(currentang);

			currentang = (float)Math.Atan(height3 / width3) + (float)Math.PI + anglRad;
			hyp = (float)Math.Sqrt(height3 * height3 + width3 * width3);
			quadPosition[0] = worldX + hyp * (float)Math.Cos(currentang);
			quadPosition[1] = worldY + hyp * (float)Math.Sin(currentang);

			currentang = (float)Math.Atan(-height4 / width4) + anglRad;
			hyp = (float)Math.Sqrt(height4 * height4 + width4 * width4);
			quadPosition[9] = worldX + hyp * (float)Math.Cos(currentang);
			quadPosition[10] = worldY + hyp * (float)Math.Sin(currentang);
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
			UpdateQuadPosition();
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
					floatData = new float[] { worldX, worldY, width1, height1, width2, height2, width3, height3, width4, height4},
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
