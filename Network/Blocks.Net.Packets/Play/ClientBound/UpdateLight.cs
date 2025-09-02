using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets.Chunks;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("light_update",true,"Play")]
public partial class UpdateLight : IPacket
{
    [PacketField] public VarInt X;
    [PacketField] public VarInt Z;
    [PacketField] public LightData Data;
}