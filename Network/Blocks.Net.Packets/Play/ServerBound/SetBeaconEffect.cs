using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("set_beacon",false,"Play")]
public partial class SetBeaconEffect : IPacket
{
    [PacketField] public VarInt? PrimaryEffect;
    [PacketField] public VarInt? SecondaryEffect;
}