
using Blocks.Net.DataTypes;
namespace Blocks.Net.Data.Vanilla;

public static partial class Blocks
{
    static partial void InitializeLookup();
    static Blocks()
    {
        InitializeLookup();
    }

    private static Block[] _allBlocks;
    private static int[] _stateToBlockIndexLookup;
    

    public static Block GetBlock(NamespacedIdentifier blockId)
    {
        return _allBlocks.First(x => x.Id == blockId);
    }

    public static Block GetBlock(this BlockState blockState)
    {
        if (blockState.StateId < 0 || blockState.StateId >= _stateToBlockIndexLookup.Length) throw new InvalidBlockStateException();
        return _allBlocks[_stateToBlockIndexLookup[blockState.StateId]];
    }
    
    
    // Here we want to generate a list of all the default block states for each ID
    
}