using Blocks.Net.DataTypes;

namespace Blocks.Net.Data.Vanilla;

public record DefaultTagsRegistry(NamespacedIdentifier Name, params DefaultTag[] Tags);