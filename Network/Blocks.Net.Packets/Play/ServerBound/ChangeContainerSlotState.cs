using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("container_slot_state_changed",false,"Play")]
public partial class ChangeContainerSlotState : IPacket
{
    [PacketField] public VarInt SlotId;
    [PacketField] public VarInt WindowId;
    [PacketField] public bool State;
}