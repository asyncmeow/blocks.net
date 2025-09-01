using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Handshake;

[Packet("intention",false,"Handshake")]
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