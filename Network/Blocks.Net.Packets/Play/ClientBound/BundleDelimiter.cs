using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("bundle_delimiter",true,"Play")]
public partial class BundleDelimiter : IPacket;