using Blocks.Net.LibSourceGeneration.Constraints;
using Blocks.Net.LibSourceGeneration.Expressions;
using Blocks.Net.LibSourceGeneration.References;

namespace Blocks.Net.LibSourceGeneration.Interfaces;

public interface IGeneric<out T>
{
    public T WithGenericParameters(params TypeParameterReference[] names);
    public T WithConstraint(string genericName, BaseGenericConstraint constraint);
}