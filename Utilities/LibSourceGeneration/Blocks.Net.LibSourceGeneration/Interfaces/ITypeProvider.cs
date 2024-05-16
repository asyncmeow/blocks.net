using Blocks.Net.LibSourceGeneration.References;

namespace Blocks.Net.LibSourceGeneration.Interfaces;

/// <summary>
/// A node that can have type definitions as children
/// </summary>
public interface ITypeProvider<out T>
{
    /// <summary>
    /// Adds a class to the type provider
    /// </summary>
    /// <param name="name">The name of the class</param>
    /// <param name="class">A reference to the class</param>
    /// <returns>The type provider for method chaining</returns>
    public T AddClass(string name, out StructuredTypeReference @class);
    
    /// <summary>
    /// Adds a class to the type provider
    /// </summary>
    /// <param name="name">The name of the class</param>
    /// <param name="construct">A delegate for finishing constructing the class</param>
    /// <returns>The type provider for method chaining</returns>
    public T AddClass(string name, Action<StructuredTypeReference> construct);
    
    /// <summary>
    /// Adds a struct to the type provider
    /// </summary>
    /// <param name="name">The name of the struct</param>
    /// <param name="struct">A reference to the struct</param>
    /// <returns>The type provider for method chaining</returns>
    public T AddStruct(string name, out StructuredTypeReference @struct);
    
    
    /// <summary>
    /// Adds a struct to the type provider
    /// </summary>
    /// <param name="name">The name of the struct</param>
    /// <param name="construct">A delegate for finishing constructing the struct</param>
    /// <returns>The type provider for method chaining</returns>
    public T AddStruct(string name, Action<StructuredTypeReference> construct);
    
    /// <summary>
    /// Adds a record to the type provider
    /// </summary>
    /// <param name="name">The name of the record</param>
    /// <param name="record">A reference to the record</param>
    /// <returns>The type provider for method chaining</returns>
    public T AddRecord(string name, out StructuredTypeReference record);
    
    /// <summary>
    /// Adds a record to the type provider
    /// </summary>
    /// <param name="name">The name of the record</param>
    /// <param name="construct">A delegate for finishing constructing the record</param>
    /// <returns>The type provider for method chaining</returns>
    public T AddRecord(string name, Action<StructuredTypeReference> construct);
    
    
    /// <summary>
    /// Adds a interface to the type provider
    /// </summary>
    /// <param name="name">The name of the interface</param>
    /// <param name="interface">A reference to the interface</param>
    /// <returns>The type provider for method chaining</returns>
    public T AddInterface(string name, out StructuredTypeReference @interface);
    
    /// <summary>
    /// Adds a interface to the type provider
    /// </summary>
    /// <param name="name">The name of the interface</param>
    /// <param name="construct">A delegate for finishing constructing the interface</param>
    /// <returns>The type provider for method chaining</returns>
    public T AddInterface(string name, Action<StructuredTypeReference> construct);
}