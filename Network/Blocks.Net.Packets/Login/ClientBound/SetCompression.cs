using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Login.ClientBound;

[PublicAPI]
[Packet(0x03,true,"Login")]
public partial class SetCompression : IPacket
{
    [PacketField] public VarInt Threshold;
}