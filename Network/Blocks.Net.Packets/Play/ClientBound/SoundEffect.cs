using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("sound",true,"Play")]
public partial class SoundEffect : IPacket
{
    [PacketField] public IdOrSoundEvent SoundEvent;
    [PacketField] public VarInt SoundCategory;
    [PacketField] public int X;
    [PacketField] public int Y;
    [PacketField] public int Z;
    [PacketField] public float Volume;
    [PacketField] public float Pitch;
    [PacketField] public long Seed;
}