using Blocks.Net.LibSourceGeneration.References;

namespace Blocks.Net.LibSourceGeneration.Interfaces;

/// <summary>
/// The current builder can provide class/struct/record/enum references
/// </summary>
public interface ITypeProvider<out T>
{
    public T AddClass(string name, out StructuredTypeReference @class);
    public T AddClass(string name, Action<StructuredTypeReference> construct);
    public T AddStruct(string name, out StructuredTypeReference @struct);
    public T AddStruct(string name, Action<StructuredTypeReference> construct);
    public T AddRecord(string name, out StructuredTypeReference record);
    public T AddRecord(string name, Action<StructuredTypeReference> construct);
    public T AddInterface(string name, out StructuredTypeReference @interface);
    public T AddInterface(string name, Action<StructuredTypeReference> construct);
}