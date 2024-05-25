using Blocks.Net.Nbt;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x31,true,"Play")]
public partial class OpenScreen : IPacket
{
    [PacketField] public VarInt WindowId;
    [PacketField] public VarInt WindowType;
    [PacketField] public NbtTag WindowTitle;
}