using Blocks.Net.Nbt;
using Blocks.Net.PacketSourceGenerator.Attributes;
using Blocks.Net.Text;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Configuration.ClientBound;

[PublicAPI]
[Packet("disconnect",true,"Configuration")]
public partial class Disconnect : IPacket
{
    [PacketField] public TextComponent Reason;
}