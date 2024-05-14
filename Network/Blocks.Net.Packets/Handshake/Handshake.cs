using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Handshake;

[Packet(0x00,false,"Handshaking")]
public partial struct Handshake : IPacket
{
    public enum NextStateEnum
    {
        Status = 1,
        Login = 2
    }
    
    [PacketField] public VarInt ProtocolVersion;
    [PacketField] public string ServerAddress;
    [PacketField] public ushort ServerPort;
    [PacketField] public NextStateEnum NextState;
}