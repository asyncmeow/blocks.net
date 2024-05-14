using Blocks.Net.Nbt;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Configuration.ClientBound;


[PublicAPI]
[Packet(0x05,true,"Configuration")]
public partial class RegistryData : IPacket
{
    [PacketField] public NbtTag RegistryCodec;
}