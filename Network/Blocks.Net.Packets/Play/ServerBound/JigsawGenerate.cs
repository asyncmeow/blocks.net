using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("jigsaw_generate",false,"Play")]
public partial class JigsawGenerate : IPacket
{
    [PacketField] public Position Location;
    [PacketField] public VarInt Levels;
    [PacketField] public bool KeepJigsaws;
}