using Blocks.Net.Nbt;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Configuration.ClientBound;

[PublicAPI]
[Packet(0x07,true,"Configuration")]
public partial class AddResourcePack : IPacket
{
    [PacketField] public Uuid Uuid;
    [PacketField] public string Url;
    [PacketField] public string Hash;
    [PacketField] public bool Forced;
    [PacketField] public bool HasPromptMessage;
    [PacketOptionalField("HasPromptMessage")] public NbtTag PromptMessage;
}