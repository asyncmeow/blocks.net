using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Slots.Components;


[SubPacket]
[GenerateIdOrXFor]
public partial struct BannerPattern
{
    [PacketField] public Identifier AssetId;
    [PacketField] public string TranslationKey;
}