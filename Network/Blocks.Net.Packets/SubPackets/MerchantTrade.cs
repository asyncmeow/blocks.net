using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets;

[SubPacket]
public partial struct MerchantTrade
{
    [PacketField] public Slot InputItem1;
    [PacketField] public Slot OuputItem;
    [PacketField] public Slot InputItem2;
    [PacketField] public bool TradeDisabled;
    [PacketField] public int NumberOfTradeUses;
    [PacketField] public int MaximumNumberOfTradeUses;
    [PacketField] public int Xp;
    [PacketField] public int SpecialPrice;
    [PacketField] public float PriceMultiplier;
    [PacketField] public int Demand;
}