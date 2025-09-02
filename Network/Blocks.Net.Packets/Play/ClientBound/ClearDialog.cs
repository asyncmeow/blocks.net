using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("clear_dialog",true,"Play")]
public partial class ClearDialog : IPacket;