using Blocks.Net.Nbt;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x1A,true,"Play")]
public partial class Disconnect : IPacket
{
    [PacketField] public NbtTag Reason;
}