using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("edit_book",false,"Play")]
public partial class EditBook : IPacket
{
    [PacketField] public VarInt Slot;
    [PacketField] public string[] Entries;
    [PacketField] public string? Title;
}