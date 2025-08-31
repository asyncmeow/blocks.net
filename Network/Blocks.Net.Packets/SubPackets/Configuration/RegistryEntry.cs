using Blocks.Net.Nbt;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Configuration;

[SubPacket]
public partial struct RegistryEntry
{
    [PacketField] public Identifier EntryId;
    [PacketField] public NbtTag? Data;
}