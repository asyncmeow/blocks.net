using Blocks.Net.LibSourceGeneration.References;

namespace Blocks.Net.LibSourceGeneration.Interfaces;




public interface ITopLevelProvider<out T> : IAddable<T>
{
    /// <summary>
    /// Add a namespace to this node
    /// </summary>
    /// <param name="name">The name of the namespace</param>
    /// <param name="ns">A reference to the constructed namespace</param>
    /// <returns>The instance for method chaining</returns>
    public T WithNamespace(string name, out NamespaceReference ns);
    
    /// <summary>
    /// Add a namespace to this node
    /// </summary>
    /// <param name="name">The name of the namespace</param>
    /// <param name="construct">A delegate that takes the constructed namespace and finishes constructing it</param>
    /// <returns>The instance for method chaining</returns>
    public T WithNamespace(string name, Action<NamespaceReference> construct);

    /// <summary>
    /// Import a set of namespaces into the scope
    /// </summary>
    /// <param name="namespaces">The namespaces to import into the scope</param>
    /// <returns>The instance for method chaining</returns>
    public T Using(params string[] namespaces);
    
    /// <summary>
    /// Import a set of namespaces into the scope
    /// </summary>
    /// <param name="namespaces">The namespaces to import into the scope</param>
    /// <returns>The instance for method chaining</returns>
    public T Using(IEnumerable<string> namespaces);

    /// <summary>
    /// Import a set of static classes into the scope
    /// </summary>
    /// <param name="classes">The static classes to import into the scope</param>
    /// <returns>The instance for method chaining</returns>
    public T UsingStatic(params string[] classes);

    /// <summary>
    /// Import a set of static classes into the scope
    /// </summary>
    /// <param name="classes">The static classes to import into the scope</param>
    /// <returns>The instance for method chaining</returns>
    public T UsingStatic(IEnumerable<string> classes);

    /// <summary>
    /// Alias a type in the scope
    /// </summary>
    /// <param name="newTypeName">The new name you want to refer to the type as</param>
    /// <param name="oldTypeName">The old name of the type</param>
    /// <returns>The instance for method chaining</returns>
    public T Alias(string newTypeName, string oldTypeName);
}