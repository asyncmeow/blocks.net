using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;
using KnownPacks = Blocks.Net.Packets.SubPackets.Configuration.KnownPacks;

namespace Blocks.Net.Packets.Configuration.ClientBound;

[PublicAPI]
[Packet(0x0E, true, "Configuration")]
public partial class ClientBoundKnownPacks : IPacket
{
    [PacketField] public VarInt NumPacks;
    [PacketArrayField(nameof(NumPacks))] public KnownPacks[] Packs;
}