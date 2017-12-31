using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using _2DRPG.Save;

namespace _2DRPG.Net {
	static class SessionManager {
		public static bool isHost = false;

		private static IPEndPoint partner = null;
		private static Timer hbTread;
		private static Thread rtThread;

		public static string ConnectionInfo() {
			if (SaveData.GameSettings.coOp) {
				return ((isHost) ? ("Host - ") : ("Guest - ")) + ((partner == null) ? ("no partner") : ("partner: " + partner.ToString())); 
			}
			return "CoOp is not enabled";
		}

		public static void StartRetreival() {
			if (rtThread != null && rtThread.IsAlive)
				return;
			rtThread = new Thread(UDPMessager.RetreivalLoop) {
				IsBackground = true
			};
			rtThread.Start();
		}

		public static void NewPartner(IPEndPoint p) {
			//As Host, treat the IPEP as the guest
			SetupPartner(p);
			SendFrame(new UDPFrame() {
				subject = UDPFrame.FrameType.Connection, intData = new int[] { 3 }
			});
		}

		private static void SetupPartner(IPEndPoint p) {
			partner = p;
			hbTread = new Timer(HeartbeatLoop, null, 0, 500);		//500ms loop
		}

		public static void JoinPartner(IPEndPoint p) {
			//Attempt to join host at p
			SaveData.GameSettings.coOp = true;
			SetupPartner(p);
			StartRetreival();
			SendFrame(new UDPFrame() {
				subject = UDPFrame.FrameType.Connection,
				intData = new int[] { 1 }
			});
		}

		public static void EndPartner() {
			if (isHost) {
				hbTread.Dispose();
				SendFrame(new UDPFrame() { subject = UDPFrame.FrameType.Connection, intData = new int[] { 0 } });
				isHost = false;
			}
			GUI.Windows.NotificationWindow.NewNotification("Connection Severed", 60);
			SaveData.GameSettings.coOp = false;
			partner = null;
		}

		/// <summary>
		/// Sends a UDPFrame packet to the connected partner
		/// </summary>
		/// <param name="frame"></param>
		/// <returns></returns>
		public static int SendFrame(UDPFrame frame) {
			if (SaveData.GameSettings.coOp && partner != null) {
				System.Diagnostics.Debug.WriteLine("Sending frame: " + frame.subject.ToString());
				frame.packetID = Guid.NewGuid();
				UDPMessager.SendFrame(frame, partner);
				if (frame.subject != UDPFrame.FrameType.Confirmation)
					lock(confirmationQueue)
						confirmationQueue.Add(frame);
				return 1;
			}
			return 0;
		}

		private static List<Guid> seenConfirmations = new List<Guid>();
		public static void HeartbeatLoop(object state) {
			if (partner == null)
				return;
			int hearts = 0;
			//Checks the list of queued confirmed items, if it's been more than one cycle of the heartbeat loop,
			//then it resends it. TODO: Perhaps change the time delay between resending (some might have a ping over 500)
			lock (confirmationQueue) {
				for (int i = confirmationQueue.Count - 1; i >= 0; i--) {
					if (seenConfirmations.Contains(confirmationQueue[i].packetID)) {
						SendFrame(confirmationQueue[i]);
						confirmationQueue.RemoveAt(i);
					}
				}
				seenConfirmations.Clear();
				for (int i = 0; i < confirmationQueue.Count; i++) {
					if (confirmationQueue[i].subject == UDPFrame.FrameType.Heartbeat)
						hearts++;
					seenConfirmations.Add(confirmationQueue[i].packetID);
				}
			}
			if (hearts > 10) {       //Arbitrary packet timeout  limit of 5 seconds
				System.Diagnostics.Debug.WriteLine("Packet Timeout, 5 seconds without response");
				EndPartner();
			}
			if (isHost)
				SendFrame(new UDPFrame() { subject = UDPFrame.FrameType.Heartbeat});
		}

		private static List<UDPFrame> confirmationQueue = new List<UDPFrame>();

		public static void CommandSwitching(UDPFrame frame, IPEndPoint ep) {
			switch (frame.subject) {
				case UDPFrame.FrameType.Confirmation:
					int index = -1;
					lock (confirmationQueue) {
						for (int i = 0; i < confirmationQueue.Count; i++)
							if (confirmationQueue[i].packetID.Equals(frame.uid))
								index = i;
						if (index >= 0) {
							confirmationQueue.RemoveAt(index);
						}
					}
					return;
				case UDPFrame.FrameType.Heartbeat:
					System.Diagnostics.Debug.WriteLine("Heartbeat");
					break;
				case UDPFrame.FrameType.Movement:   //Packet received by guest to update position of an object that moved on the host's end
					System.Diagnostics.Debug.WriteLine("Movement");
					if (frame.floatData.Length > 1 && frame.stringData.Length > 0 && WorldData.currentRegions.ContainsKey(frame.stringData[0])) {
						lock(WorldData.currentRegions)
							foreach (World.Objects.WorldObjectBase b in WorldData.currentRegions[frame.stringData[0]].GetWorldObjects()) {
								if (b.uid.Equals(frame.uid)) {
									b.SetWorldPosition(frame.floatData[0], frame.floatData[1]);
									break;
								}
							}
					}
					break;
				case UDPFrame.FrameType.Quest:
					System.Diagnostics.Debug.WriteLine("Quest");
					break;
				case UDPFrame.FrameType.Player:
					switch (frame.intData[0]) {
						case 0:
							System.Diagnostics.Debug.WriteLine("Loaded new guest player");
							WorldData.AddPlayer(frame.uid);
							if (isHost) {       //If host received guest, return the host character to the guest
								SendFrame(new UDPFrame() {
									subject = UDPFrame.FrameType.Player,
									intData = new int[] { 0 },
									uid = WorldData.player.uid
								});
								SendFrame(new UDPFrame() {
									subject = UDPFrame.FrameType.Player,
									intData = new int[] { 1 },
									floatData = new float[] { WorldData.player.worldX, WorldData.player.worldY }
								});
							}
							break;
						case 1:
							if (WorldData.partner != null) {
								WorldData.partner.SetWorldPosition(frame.floatData[0], frame.floatData[1]);
							}
							break;
					}
					
					break;
				case UDPFrame.FrameType.Connection:
					System.Diagnostics.Debug.WriteLine("Connection");
					if (frame.intData.Length == 0)
						break;
					switch (frame.intData[0]) {
						case 0:     //Sever connection
							EndPartner();
							break;
						case 1:     //New Partner is Guest
							NewPartner(ep);
							break;
						case 2:     //Text message
							GUI.Windows.NotificationWindow.NewNotification(frame.stringData[0], 120);
							System.Diagnostics.Debug.WriteLine("message: " + frame.stringData[0]);
							break;
						case 3:     //Confirmation from host
							SendFrame(new UDPFrame() {
								subject = UDPFrame.FrameType.Player, uid = SaveData.GameSettings.coOpUID, intData = new int[] { 0 }
							});
							break;
						default:
							System.Diagnostics.Debug.WriteLine("Connection Value: " + frame.intData[0]);
							break;
					}
					break;
				case UDPFrame.FrameType.WorldObject:
					WorldData.PacketToWorldObject(frame);
					break;
			}
			SendFrame(new UDPFrame() {
				subject = UDPFrame.FrameType.Confirmation, uid = frame.packetID
			});
		}
	}
}
