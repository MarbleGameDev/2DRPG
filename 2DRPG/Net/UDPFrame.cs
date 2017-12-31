using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace _2DRPG.Net
{
	[ProtoContract]
	public class UDPFrame {
		[ProtoMember(1)]
		public FrameType subject;
		[ProtoMember(2)]
		public int[] intData;
		[ProtoMember(3)]
		public float[] floatData;
		[ProtoMember(4)]
		public string[] stringData;
		[ProtoMember(5)]
		public Guid uid;
		[ProtoMember(6)]
		public Guid packetID;


		public enum FrameType{Heartbeat, Movement, Inventory, Interaction, Quest, Connection, WorldObject, Player, Confirmation};
    }
}
