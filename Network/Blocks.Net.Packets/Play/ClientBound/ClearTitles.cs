using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("clear_titles",true,"Play")]
public partial class ClearTitles : IPacket
{
    [PacketField] public bool Reset;
}