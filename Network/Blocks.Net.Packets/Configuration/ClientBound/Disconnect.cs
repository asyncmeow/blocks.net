using Blocks.Net.Nbt;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Configuration.ClientBound;

[PublicAPI]
[Packet(0x02,true,"Configuration")]
public partial class Disconnect : IPacket
{
    [PacketField] public NbtTag Reason;
}