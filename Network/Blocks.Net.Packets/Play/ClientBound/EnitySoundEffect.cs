using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("sound_entity",true,"Play")]
public partial class EnitySoundEffect : IPacket
{
    
    [PacketField] public IdOrSoundEvent SoundEvent;
    [PacketField] public VarInt SoundCategory;
    [PacketField] public VarInt EntityId;
    [PacketField] public float Volume;
    [PacketField] public float Pitch;
    [PacketField] public long Seed;
}