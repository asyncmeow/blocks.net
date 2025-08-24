using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets.Configuration;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Configuration.ServerBound;

[PublicAPI]
[Packet(0x07, false, "Configuration")]
public partial class ServerBoundKnownPacks : IPacket
{
    
    [PacketField] public VarInt NumPacks;
    [PacketArrayField(nameof(NumPacks))] public KnownPacks[] Packs;
}