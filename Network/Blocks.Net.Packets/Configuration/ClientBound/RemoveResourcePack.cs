using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Configuration.ClientBound;

[PublicAPI]
[Packet(0x06,true,"Configuration")]
public partial class RemoveResourcePack : IPacket
{
    [PacketField] public bool HasUuid;
    [PacketOptionalField("HasUuid")] public Uuid Uuid;
}