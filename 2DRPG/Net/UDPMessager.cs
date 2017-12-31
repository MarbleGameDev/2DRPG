using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using ProtoBuf;
using System.Windows.Forms;
using _2DRPG.Save;

namespace _2DRPG.Net {
	static class UDPMessager {

		public static int port = 23640;
		private static UdpClient server;

		static UDPMessager() {
			server = new UdpClient(port);
		}

		public static void SendFrame(UDPFrame frame, IPEndPoint p) {
			byte[] buff = UDPtoBytes(frame);
			server.Send(buff, buff.Length, p);
		}
		private static List<Guid> receieved = new List<Guid>();
		public static void RetreivalLoop() {
			IPEndPoint EP = new IPEndPoint(IPAddress.Any, port);
			while (SaveData.GameSettings.coOp) {
				byte[] data = server.Receive(ref EP);
				UDPFrame frame = BytestoUDP(data);
				if (receieved.Contains(frame.packetID)) {
					continue;
				} else {
					receieved.Add(frame.packetID);
					SessionManager.CommandSwitching(frame, EP);
				}
				if (receieved.Count > 200)	//Keep track of the last 200 packets to allow rejection of duplicates
					receieved.Clear();
			}
		}

		private static byte[] UDPtoBytes(UDPFrame frame) {
			using (MemoryStream ms = new MemoryStream()) {
				Serializer.Serialize(ms, frame);
				return ms.ToArray();
			}
		}

		private static UDPFrame BytestoUDP(byte[] data) {
			using (MemoryStream ms = new MemoryStream(data)) {
				return Serializer.Deserialize<UDPFrame>(ms);
			}
		}

		public static IPEndPoint GetLocal() {
			using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0)) {
				try {
					socket.Connect("8.8.8.8", port);
					return socket.LocalEndPoint as IPEndPoint;
				} catch (SocketException) {
					return new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
				}
			}
		}
	}
}
