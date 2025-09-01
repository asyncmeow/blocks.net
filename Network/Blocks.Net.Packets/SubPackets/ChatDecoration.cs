using Blocks.Net.Nbt;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets;

[SubPacket]
public partial struct ChatDecoration
{
    public const int ParametersSender = 0;
    public const int ParametersTarget = 1;
    public const int ParametersContent = 2;
    [PacketField] public string TranslationKey;
    [PacketField] public VarInt[] Parameters;
    [PacketField] public NbtTag Style;
}