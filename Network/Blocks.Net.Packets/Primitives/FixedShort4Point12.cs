using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Primitives;

[SubPacket]
public partial struct FixedShort4Point12
{
    [PacketField] private short _internalValue;

    [PacketField]
    public double Value
    {
        get => (double)_internalValue / 4096;
        set => _internalValue = (short)Math.Round(value * 4096);
    }
}