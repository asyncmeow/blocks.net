using Blocks.Net.Data.Vanilla;
using Blocks.Net.DataTypes;
using Blocks.Net.Packets.Configuration.ClientBound;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;

namespace Blocks.Net.Framework;

public class TagRegistrySet
{
    public Dictionary<NamespacedIdentifier, TagRegistry> TagRegistries = [];

    public TagRegistry this[NamespacedIdentifier key]
    {
        get
        {
            if (TagRegistries.TryGetValue(key, out var value))
            {
                return value;
            }

            return TagRegistries[key] = new TagRegistry();
        }
        set => TagRegistries[key] = value;
    }

    public TagRegistrySet()
    {
        foreach (var registry in DefaultTags.Registries)
        {
            var reg = this[registry.Name];
            foreach (var tag in registry.Tags)
            {
                reg[tag.Tag] = tag.Entries.Select(x => new RegistryReference(x)).ToList();
            }
        }
    }

    public UpdateTags GenerateTagUpdatePacket()
    {
        List<Packets.SubPackets.Configuration.TagsRegistry> registries = [];
        foreach (var kvp in TagRegistries)
        {
            registries.Add(new()
            {
                Registry = kvp.Key,
                Tags = kvp.Value.Tags.Select(x => new Tag()
                {
                    TagName = x.Key,
                    Entries = x.Value.Select(x => new VarInt(x.RegistryId)).ToArray()
                }).ToArray()
            });
        }

        return new UpdateTags { Registries = registries.ToArray() };
    }
}