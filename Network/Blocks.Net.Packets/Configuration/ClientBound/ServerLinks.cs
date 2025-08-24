using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets.Configuration;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Configuration.ClientBound;

[PublicAPI]
[Packet(0x0F,true, "Configuration")]
public partial class ServerLinks : IPacket
{
    [PacketField] public VarInt NumLinks;
    [PacketArrayField("NumLinks")] public ServerLink[] Links;
}