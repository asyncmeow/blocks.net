using Attribute = Blocks.Net.LibSourceGeneration.References.Attribute;

namespace Blocks.Net.LibSourceGeneration.Interfaces;

public interface IAttributable<out T>
{
    public T WithAttributes(params Attribute[] attributes);
}