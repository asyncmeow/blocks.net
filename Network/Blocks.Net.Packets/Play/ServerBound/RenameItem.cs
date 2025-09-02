using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("rename_item",false,"Play")]
public partial class RenameItem : IPacket
{
    [PacketField] public string ItemName;
}