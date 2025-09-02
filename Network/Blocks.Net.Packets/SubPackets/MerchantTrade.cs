using Blocks.Net.Packets.SubPackets.Slots;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets;

[SubPacket]
public partial struct MerchantTrade
{
    [PacketField] public TradeItem InputItem1;
    [PacketField] public Slot OutputItem;
    [PacketField] public TradeItem? InputItem2;
    [PacketField] public bool TradeDisabled;
    [PacketField] public int NumberOfTradeUses;
    [PacketField] public int MaximumNumberOfTradeUses;
    [PacketField] public int Xp;
    [PacketField] public int SpecialPrice;
    [PacketField] public float PriceMultiplier;
    [PacketField] public int Demand;
}