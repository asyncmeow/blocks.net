using System.ComponentModel.DataAnnotations;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Enums;

[FieldedEnum(typeof(VarInt))]
public enum RecipeDisplay
{
    CraftingShapeless,
    CraftingShaped,
    Furnace,
    Stonecutter,
    Smithing
}

[EnumField(typeof(RecipeDisplay))]
public partial class RecipeDisplayCraftingShapeless
{
    [PacketField] public SlotDisplayImpl[] Ingredients;
    [PacketField] public SlotDisplayImpl Result;
    [PacketField] public SlotDisplayImpl CraftingStation;
}


[EnumField(typeof(RecipeDisplay))]
public partial class RecipeDisplayCraftingShaped
{
    [PacketField] public VarInt Width;
    [PacketField] public VarInt Height;
    [PacketField] public SlotDisplayImpl[] Ingredients;
    [PacketField] public SlotDisplayImpl Result;
    [PacketField] public SlotDisplayImpl CraftingStation;
}

[EnumField(typeof(RecipeDisplay))]
public partial class RecipeDisplayFurnace
{
    [PacketField] public SlotDisplayImpl Ingredient;
    [PacketField] public SlotDisplayImpl Fuel;
    [PacketField] public SlotDisplayImpl Result;
    [PacketField] public SlotDisplayImpl CraftingStation;
    [PacketField] public VarInt CookingTime;
    [PacketField] public float Experience;
}

[EnumField(typeof(RecipeDisplay))]
public partial class RecipeDisplayStonecutter
{
    [PacketField] public SlotDisplayImpl Ingredient;
    [PacketField] public SlotDisplayImpl Result;
    [PacketField] public SlotDisplayImpl CraftingStation;
}

[EnumField(typeof(RecipeDisplay))]
public partial class RecipeDisplaySmithing
{
    [PacketField] public SlotDisplayImpl Template;
    [PacketField] public SlotDisplayImpl Base;
    [PacketField] public SlotDisplayImpl Addition;
    [PacketField] public SlotDisplayImpl Result;
    [PacketField] public SlotDisplayImpl CraftingStation;
}