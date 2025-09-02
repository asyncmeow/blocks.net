using System.Security;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("ticking_state",true,"Play")]
public partial class SetTickingState : IPacket
{
    [PacketField] public float TickRate;
    [PacketField] public bool IsFrozen;
}