using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Handshake;

[Packet(0x00,false,"Handshaking")]
public partial class Handshake : IPacket
{
    public enum NextStateEnum
    {
        Status = 1,
        Login = 2
    }
    
    
    
    [PacketField] public VarInt ProtocolVersion;
    [PacketField] public string ServerAddress;
    [PacketField] public ushort ServerPort;
    [PacketEnum(typeof(VarInt))] public NextStateEnum NextState;

}