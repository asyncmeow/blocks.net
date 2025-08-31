using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Slots.Components;

[SubPacket]
public partial struct TrimOverride
{
    [PacketField] public Identifier ArmorMaterialType;
    [PacketField] public string OverridenAssetName;
}