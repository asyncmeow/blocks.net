using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x13,true,"Play")]
public partial class SetContainerContent : IPacket
{
    [PacketField] public byte WindowId;
    [PacketField] public VarInt StateId;
    [PacketField] public VarInt Count;
    [PacketArrayField("Count")] public Slot[] SlotData;
    public Slot CarriedItem;
}