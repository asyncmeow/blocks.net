using Blocks.Net.Nbt;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x07,true,"Play")]
public partial class BlockEntityData : IPacket
{
    [PacketField] public Position Location;
    [PacketField] public VarInt Type;
    [PacketField] public NbtTag Data;
}