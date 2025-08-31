using Blocks.Net.Data.Vanilla;
using Blocks.Net.DataTypes;
using JetBrains.Annotations;

namespace Blocks.Net.Framework;

[PublicAPI]
public class RegistrySet
{
    public readonly Dictionary<NamespacedIdentifier, Registry> Registries = [];

    public Registry this[NamespacedIdentifier entryId] => Registries[entryId];


    public RegistrySet()
    {
        // Now we need to get all the vanilla registry data into the registries
        const string core = "minecraft:core";

        foreach (var registry in CoreRegistryData.Registries)
        {
            var reg = GetOrCreateRegistry(registry.Registry);
            foreach (var entry in registry.Entries)
            {
                reg.Register(entry, core, null);
            }
        }
    }
    
    public Registry GetOrCreateRegistry(NamespacedIdentifier entryId)
    {
        if (Registries.TryGetValue(entryId, out var registry))
        {
            return registry;
        }

        return Registries[entryId] = new Registry(entryId);
    }
    
    
    
    // public Registry TrimMaterial => GetOrCreateRegistry("trim_material");
    // public Registry TrimPattern => GetOrCreateRegistry("trim_pattern");
    // public Registry BannerPattern => GetOrCreateRegistry("banner_pattern");
    // public Registry Biome => GetOrCreateRegistry("worldgen/biome");
    // public Registry ChatType => GetOrCreateRegistry("chat_type");
    // public Registry DamageType => GetOrCreateRegistry("damage_type");
    // public Registry DimensionType => GetOrCreateRegistry("dimension_type");
    // public Registry WolfVariant => GetOrCreateRegistry("wolf_variant");
    // public Registry PaintingVariant => GetOrCreateRegistry("painting_variant");
    // public Registry Dialog = new Registry("dialog");
}