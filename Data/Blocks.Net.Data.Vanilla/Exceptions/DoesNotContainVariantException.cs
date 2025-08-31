namespace Blocks.Net.Data.Vanilla;

public class DoesNotContainVariantException(string variant) : Exception($"Block state does not contain variant '{variant}'")
{
    
}