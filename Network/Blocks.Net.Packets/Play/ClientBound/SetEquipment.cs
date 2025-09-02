using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("set_equipment",true,"Play")]
public partial class SetEquipment : IPacket
{
    [PacketField] public VarInt EntityId;
    [PacketField] public EquipmentArray Equipment;
}