using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("sign_update",false,"Play")]
public partial class UpdateSign : IPacket
{
    [PacketField] public Position Location;
    [PacketField] public bool IsFrontText;
    [PacketField] public string Line1;
    [PacketField] public string Line2;
    [PacketField] public string Line3;
    [PacketField] public string Line4;
}