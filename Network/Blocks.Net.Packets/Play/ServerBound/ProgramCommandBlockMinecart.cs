using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("set_command_minecart",false,"Play")]
public partial class ProgramCommandBlockMinecart : IPacket
{
    [PacketField] public VarInt EntityId;
    [PacketField] public string Command;
    [PacketField] public bool TrackOutput;
}