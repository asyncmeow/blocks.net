using Blocks.Net.DataTypes;
using Blocks.Net.Nbt;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("open_screen",true,"Play")]
public partial class OpenScreen : IPacket
{
    [PacketField] public VarInt WindowId;
    [PacketField] public RegistryReference WindowType;
    [PacketField] public NbtTag? WindowTitle;
}