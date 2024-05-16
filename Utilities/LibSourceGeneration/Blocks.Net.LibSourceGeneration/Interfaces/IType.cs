using Blocks.Net.LibSourceGeneration.Definitions;

namespace Blocks.Net.LibSourceGeneration.Interfaces;

/// <summary>
/// This excludes enums, it is records, structs, classes, and interfaces
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IType<out T> : IVisible<T>, IDocumentable<T>, IAttributable<T>, IGeneric<T>
{
    public T Partial();
    public T Sealed();
    public T Abstract();
    public T Readonly();
    public T Record();
}
