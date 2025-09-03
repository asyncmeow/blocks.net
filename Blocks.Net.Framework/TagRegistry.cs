using Blocks.Net.DataTypes;

namespace Blocks.Net.Framework;

public class TagRegistry
{
    public Dictionary<NamespacedIdentifier, List<RegistryReference>> Tags = [];

    public List<RegistryReference> this[NamespacedIdentifier key]
    {
        get
        {
            if (Tags.TryGetValue(key, out var value))
            {
                return value;
            }
            return Tags[key] = [];
        }
        set => Tags[key] = value;
    }
    
    
}