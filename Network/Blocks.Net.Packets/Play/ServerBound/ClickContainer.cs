using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets.Slots;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("container_click",false,"Play")]
public partial class ClickContainer : IPacket
{
    [PacketField] public VarInt WindowId;
    [PacketField] public VarInt StateId;
    [PacketField] public short ContainerSlot;
    [PacketField] public byte Button;
    [PacketField] public VarInt Mode;
    [PacketField] public ChangedSlot[] ChangedSlots;
    [PacketField] public HashedSlot CarriedItem;
}