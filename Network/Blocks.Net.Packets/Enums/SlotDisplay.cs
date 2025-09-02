using Blocks.Net.DataTypes;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets.Slots;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Enums;

[FieldedEnum(typeof(VarInt))]
public enum SlotDisplay
{
    Empty,
    AnyFuel,
    Item,
    ItemStack,
    Tag,
    SmithingTrim,
    WithRemainder,
    Composite
}

[EnumField(typeof(SlotDisplay))]
public partial class SlotDisplayItem
{
    [PacketField] public ItemId Item;
}


[EnumField(typeof(SlotDisplay))]
public partial class SlotDisplayItemStack
{
    [PacketField] public Slot ItemStack;
}

[EnumField(typeof(SlotDisplay))]
public partial class SlotDisplayTag
{
    [PacketField] public Identifier Tag;
}

[EnumField(typeof(SlotDisplay))]
public partial class SlotDisplaySmithingTrim
{
    [PacketField] public SlotDisplayImpl Base;
    [PacketField] public SlotDisplayImpl Material;
    [PacketField] public RegistryReference Pattern;
}

[EnumField(typeof(SlotDisplay))]
public partial class SlotDisplayWithRemainder
{
    [PacketField] public SlotDisplayImpl Ingredient;
    [PacketField] public SlotDisplayImpl Remainder;
}

[EnumField(typeof(SlotDisplay))]
public partial class SlotDisplayComposite
{
    [PacketField] public SlotDisplayImpl[] Options;
}