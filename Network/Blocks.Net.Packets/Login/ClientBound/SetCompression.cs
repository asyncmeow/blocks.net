using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Login.ClientBound;

[PublicAPI]
[Packet("login_compression",true,"Login")]
public partial class SetCompression : IPacket
{
    [PacketField] public VarInt Threshold;
}