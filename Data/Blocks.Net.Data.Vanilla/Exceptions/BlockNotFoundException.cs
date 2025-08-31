namespace Blocks.Net.Data.Vanilla;

public class BlockNotFoundException(string id) : Exception($"No block with the id {id} was found in the registry.");