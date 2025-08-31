using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Configuration.ClientBound;

[PublicAPI]
[Packet(0x0C,true,"Configuration")]
public partial class FeatureFlags : IPacket
{
    [PacketField] public string[] Flags;
}