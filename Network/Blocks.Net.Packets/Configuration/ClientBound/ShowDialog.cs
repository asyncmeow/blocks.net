using Blocks.Net.Nbt;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Configuration.ClientBound;

[PublicAPI]
[Packet("show_dialog", true, "Configuration")]
public partial class ShowDialog : IPacket
{
    [PacketField] public NbtTag Dialog;
}