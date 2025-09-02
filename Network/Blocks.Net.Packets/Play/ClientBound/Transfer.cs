using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("transfer",true,"Play")]
public partial class Transfer : IPacket
{
    [PacketField] public string Host;
    [PacketField] public VarInt Port;
}