using Blocks.Net.Nbt;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.MapData;

[SubPacket]
public partial struct MapIcon
{
    public enum MapIconType
    {
        WhiteArrow = 0,
        GreenArrow = 1,
        RedArrow = 2,
        BlueArrow = 3,
        RedPointer = 5,
        WhiteCircle = 6,
        SmallWhiteCircle = 7,
        Mansion = 8,
        Monument = 9,
        WhiteBanner = 10,
        OrangeBanner = 11,
        MagentaBanner = 12,
        LightBlueBanner = 13,
        YellowBanner = 14,
        LimeBanner = 15,
        PinkBanner = 16,
        GrayBanner = 17,
        LightGrayBanner = 18,
        CyanBanner = 19,
        PurpleBanner = 20,
        BlueBanner = 21,
        BrownBanner = 22,
        GreenBanner = 23,
        RedBanner = 24,
        BlackBanner = 25,
        TreasureMarker = 26
    }

    [PacketEnum(typeof(VarInt))] public MapIconType Type;
    [PacketField] public sbyte X;
    [PacketField] public sbyte Z;
    [PacketField] public byte Direction;
    [PacketField] public bool HasDisplayName;

    [PacketOptionalField("HasDisplayName")]
    public NbtTag DisplayName;
}