using Blocks.Net.DataTypes;

namespace Blocks.Net.Data.Vanilla;

public record CoreRegistry(NamespacedIdentifier Registry, params NamespacedIdentifier[] Entries);