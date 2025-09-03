using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Configuration.ClientBound;

[PublicAPI]
[Packet("select_known_packs", true, "Configuration")]
public partial class KnownPacks : IPacket
{
    [PacketField] public SubPackets.Configuration.KnownPack[] Packs;
}