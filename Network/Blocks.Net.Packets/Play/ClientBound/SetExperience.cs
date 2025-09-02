using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("set_experience",true,"Play")]
public partial class SetExperience : IPacket
{
    [PacketField] public float ExperienceBar;
    [PacketField] public VarInt Level;
    [PacketField] public VarInt TotalExperience;
}