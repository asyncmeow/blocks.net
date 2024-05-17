using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Play.ClientBound;

[PublicAPI]
[Packet(0x00,true,"Play")]
public partial class BundleDelimiter : IPacket;