using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x00,true,"Play")]
public partial class BundleDelimiter : IPacket;