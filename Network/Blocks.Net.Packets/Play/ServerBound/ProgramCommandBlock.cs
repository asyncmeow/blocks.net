using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("set_command_block",false,"Play")]
public partial class ProgramCommandBlock : IPacket
{
    [PacketField] public Position Location;
    [PacketField] public string Command;
    [PacketField] public VarInt Mode;
    [PacketField] public byte Flags;
}