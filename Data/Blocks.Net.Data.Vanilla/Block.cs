using System.Text.Json.Serialization;
using Blocks.Net.DataTypes;
using JetBrains.Annotations;

namespace Blocks.Net.Data.Vanilla;

[PublicAPI]
public class Block
{
    public Block(NamespacedIdentifier id)
    {
        Id = id;
    }

    public NamespacedIdentifier Id { get; }


    public virtual int GetVariantInt(BlockState state, string variantName) =>
        throw new DoesNotContainVariantException(variantName);

    public virtual bool GetVariantBool(BlockState state, string variantName) =>
        throw new DoesNotContainVariantException(variantName);

    public virtual string GetVariantString(BlockState state, string variantName) =>
        throw new DoesNotContainVariantException(variantName);

    public virtual BlockState SetVariantInt(BlockState state, string variantName, int value) =>
        throw new DoesNotContainVariantException(variantName);

    public virtual BlockState SetVariantBool(BlockState state, string variantName, bool value) =>
        throw new DoesNotContainVariantException(variantName);

    public virtual BlockState SetVariantString(BlockState state, string variantName, string value) =>
        throw new DoesNotContainVariantException(variantName);
}