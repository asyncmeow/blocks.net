using Blocks.Net.Nbt;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets.Configuration;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Configuration.ClientBound;

[PublicAPI]
[Packet(0x07, true, "Configuration")]
public partial class RegistryData : IPacket
{
    [PacketField] public Identifier RegistryId;
    [PacketField] public RegistryEntry[] Entries;
}