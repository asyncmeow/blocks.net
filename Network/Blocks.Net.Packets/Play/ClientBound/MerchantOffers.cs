using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x2B,true,"Play")]
public partial class MerchantOffers : IPacket
{
    [PacketField] public VarInt WindowId;
    [PacketField] public VarInt Size;
    [PacketArrayField("Size")] public MerchantTrade[] Trades;
    [PacketField] public VarInt VillagerLevel;
    [PacketField] public VarInt Experience;
    [PacketField] public bool IsRegularVillager;
    [PacketField] public bool CanRestock;
}