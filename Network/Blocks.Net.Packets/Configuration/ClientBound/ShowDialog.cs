using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Configuration.ClientBound;

[PublicAPI]
[Packet(0x12, true, "Configuration")]
public partial class ShowDialog : IPacket
{
    [PacketField] public Primitives.Nbt Dialog;
}