using Blocks.Net.DataTypes;
using Blocks.Net.Nbt;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Slots.Components;

[SubPacket]
[GenerateIdOrXFor]
public partial struct TrimPattern
{
    [PacketField] public string AssetName;
    [PacketField] public ItemId TemplateItem;
    [PacketField] public NbtTag Description;
    [PacketField] public bool Decal;
}