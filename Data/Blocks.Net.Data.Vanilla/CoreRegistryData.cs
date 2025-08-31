using JetBrains.Annotations;

namespace Blocks.Net.Data.Vanilla;

[PublicAPI]
public static partial class CoreRegistryData
{
    
    // A field of List<NamespacedIdentifier> will be made for each of these
    public static List<CoreRegistry> Registries = [];

    static partial void AddAllRegistries();
    
    static CoreRegistryData()
    {
        AddAllRegistries();
    }
}