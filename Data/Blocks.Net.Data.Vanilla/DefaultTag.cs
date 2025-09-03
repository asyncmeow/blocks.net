using Blocks.Net.DataTypes;

namespace Blocks.Net.Data.Vanilla;

public record DefaultTag(NamespacedIdentifier Tag, params int[] Entries);