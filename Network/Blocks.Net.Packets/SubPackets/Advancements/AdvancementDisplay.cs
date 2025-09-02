using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets.Slots;
using Blocks.Net.PacketSourceGenerator.Attributes;
using Blocks.Net.Text;

namespace Blocks.Net.Packets.SubPackets.Advancements;

[SubPacket]
public partial struct AdvancementDisplay
{
    public enum FrameTypes
    {
        Task,
        Challenge,
        Goal
    }

    [Flags]
    public enum FlagsEnum
    {
        Background = 0x1,
        ShowToast = 0x2,
        Hidden = 0x4
    }
    
    [PacketField] public TextComponent Title;
    [PacketField] public TextComponent Description;
    [PacketField] public Slot Icon;
    [PacketEnum(typeof(VarInt))] public FrameTypes Type;
    [PacketEnum(typeof(int))] public FlagsEnum Flags;
    [PacketField] public float X;
    [PacketField] public float Y;
}