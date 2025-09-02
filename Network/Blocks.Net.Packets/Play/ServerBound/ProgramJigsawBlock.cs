using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("set_jigsaw_block",false,"Play")]
public partial class ProgramJigsawBlock : IPacket
{
    [PacketField] public Position Location;
    [PacketField] public Identifier Name;
    [PacketField] public Identifier Target;
    [PacketField] public Identifier Pool;
    [PacketField] public string FinalState;
    [PacketField] public string JointType;
    [PacketField] public VarInt SelectionPriority;
    [PacketField] public VarInt PlacementPriority;
}