using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("merchant_offers",true,"Play")]
public partial class MerchantOffers : IPacket
{
    [PacketField] public VarInt WindowId;
    [PacketField] public MerchantTrade[] Trades;
}