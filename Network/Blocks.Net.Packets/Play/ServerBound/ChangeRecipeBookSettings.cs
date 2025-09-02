using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("recipe_book_change_settings",false,"Play")]
public partial class ChangeRecipeBookSettings : IPacket
{
    public enum Books
    {
        Crafting,
        Furnace,
        BlastFurnace,
        Smoker
    }

    [PacketEnum(typeof(VarInt))] public Books Book;
    [PacketField] public bool BookOpen;
    [PacketField] public bool FilterActive;
}