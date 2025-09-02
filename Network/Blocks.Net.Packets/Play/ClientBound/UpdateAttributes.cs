using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("update_attributes",true,"Play")]
public partial class UpdateAttributes : IPacket
{
    [PacketField] public VarInt EntityId;
    [PacketField] public EntityProperty[] Properties;
}