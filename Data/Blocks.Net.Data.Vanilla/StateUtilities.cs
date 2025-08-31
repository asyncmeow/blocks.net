using System.Runtime.CompilerServices;

namespace Blocks.Net.Data.Vanilla;

public static class StateUtilities
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetStateValue(int currentState, int stateBase, int numPriorStates, int numberInVariant)
    {
        currentState -= stateBase;
        currentState /= numPriorStates;
        var result = currentState % numberInVariant;
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int SetStateValue(int currentState, int stateBase, int numPriorStates, int numberInVariant,
        int stateValue)
    {
        currentState -= stateBase;
        var oldPriorVariants = currentState % numPriorStates;
        currentState /= numPriorStates;
        currentState -= currentState % numberInVariant;
        currentState += stateValue;
        currentState *= numPriorStates;
        currentState += oldPriorVariants;
        currentState += stateBase;
        return currentState;
    }
}