using Blocks.Net.LibSourceGeneration.References;

namespace Blocks.Net.LibSourceGeneration.Interfaces;




public interface ITopLevelProvider<out T>
{
    public T WithNamespace(string name, out NamespaceReference ns);
    public T WithNamespace(string name, Action<NamespaceReference> construct);
}