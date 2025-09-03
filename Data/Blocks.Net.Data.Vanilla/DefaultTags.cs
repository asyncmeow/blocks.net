namespace Blocks.Net.Data.Vanilla;

public static partial class DefaultTags
{
    
    // A field of List<NamespacedIdentifier> will be made for each of these
    public static List<DefaultTagsRegistry> Registries;
    static partial void AddAllTags();

    static DefaultTags()
    {
        Registries = [];
        AddAllTags();
    }
}