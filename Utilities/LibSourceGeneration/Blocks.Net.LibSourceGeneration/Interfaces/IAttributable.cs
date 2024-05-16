using Attribute = Blocks.Net.LibSourceGeneration.Expressions.Attribute;

namespace Blocks.Net.LibSourceGeneration.Interfaces;

public interface IAttributable<out T>
{
    public T WithAttributes(params Attribute[] attributes);

    public T WithAttributes(IEnumerable<Attribute> attributes);
}