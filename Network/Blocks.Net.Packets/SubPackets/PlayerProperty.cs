using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.SubPackets;

[SubPacket]
[PublicAPI]
public partial struct PlayerProperty
{
    [PacketField] public string Name;
    [PacketField] public string Value;
    [PacketField] public bool Signed;
    [PacketOptionalField("Signed")] public string Signature;
}