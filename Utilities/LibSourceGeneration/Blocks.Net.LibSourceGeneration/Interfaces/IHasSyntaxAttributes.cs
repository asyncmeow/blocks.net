using Blocks.Net.LibSourceGeneration.Query;

namespace Blocks.Net.LibSourceGeneration.Interfaces;

public interface IHasSyntaxAttributes
{
    /// <summary>
    /// Does this syntax node have at least one attribute of type T
    /// </summary>
    /// <typeparam name="T">The attribute to check for</typeparam>
    /// <returns>True if there is any attribute of that type</returns>
    public bool HasAttribute<T>() where T : Attribute;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>A list of attributes</returns>
    public IEnumerable<T> GetAttributes<T>() where T : Attribute;

    // /// <summary>
    // /// Does this syntax node have at least one attribute with of type name
    // /// </summary>
    // /// <param name="name">The name of the attribute type</param>
    // /// <returns>True if there is any attribute of that type</returns>
    // public bool HasAttribute(string name);





    // /// <summary>
    // /// Get all attributes with a type of said name
    // /// </summary>
    // /// <param name="name">The name of the attribute type</param>
    // /// <returns>A list of syntax attributes of that type</returns>
    // public SyntaxAttributeList GetAttributes(string name);
    //
    // /// <summary>
    // /// Get a list of all attribute declarations on this node (used for easier LINQ style querying)
    // /// </summary>
    // public SyntaxAttributeList Attributes { get; }
}