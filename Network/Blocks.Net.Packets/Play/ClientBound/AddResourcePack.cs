using Blocks.Net.Nbt;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;
using Blocks.Net.Text;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("resource_pack_push",true,"Play")]
public partial class AddResourcePack : IPacket
{
    [PacketField] public Uuid Uuid;
    [PacketField] public string Url;
    [PacketField] public string Hash;
    [PacketField] public bool Forced;
    [PacketField] public TextComponent? PromptMessage;
}