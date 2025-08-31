using System.Collections.Specialized;
using Blocks.Net.DataTypes;
using Blocks.Net.Nbt;
using Blocks.Net.Packets.Configuration.ClientBound;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets.Configuration;
using JetBrains.Annotations;

namespace Blocks.Net.Framework;

[PublicAPI]
public class Registry(NamespacedIdentifier id)
{
    public NamespacedIdentifier Id => id;

    public readonly OrderedDictionary<NamespacedIdentifier, (string pack, NbtTag? value)> Entries = [];


    public RegistryReference Register(NamespacedIdentifier entryId, string pack, NbtTag? value)
    {
        var index = Entries.Count;
        Entries[entryId] = (pack, value);
        return new RegistryReference(index);
    }


    public RegistryReference this[NamespacedIdentifier entryId] => Entries.ContainsKey(entryId)
        ? new RegistryReference(Entries.IndexOf(entryId))
        : throw new KeyNotFoundException();

    public bool HasExtraData(params string[] extraMutualPacks) =>
        Entries.Any(x => x.Key != "minecraft:core" && !extraMutualPacks.Contains(x.Value.pack));

    public RegistryData GenerateRegistryDataPacket(params string[] extraMutualPacks)
    {
        var entries = (from value in Entries
            let id = value.Key
            let pack = value.Value.pack
            let val = value.Value.value
            where pack != "minecraft:core" && !extraMutualPacks.Contains(pack)
            select new RegistryEntry { EntryId = id, Data = val }).ToList();

        return new RegistryData
        {
            RegistryId = Id,
            Entries = entries.ToArray(),
        };
    }
}