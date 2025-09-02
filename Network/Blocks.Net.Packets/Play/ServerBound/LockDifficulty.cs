using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("lock_difficulty",false,"Play")]
public partial class LockDifficulty : IPacket
{
    [PacketField] public bool Locked;
}