using Blocks.Net.DataTypes;
using Blocks.Net.Nbt;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x06,true,"Play")]
public partial class BlockEntityData : IPacket
{
    [PacketField] public Position Location;
    [PacketField] public RegistryReference Type;
    [PacketField] public NbtTag Nbt;
}