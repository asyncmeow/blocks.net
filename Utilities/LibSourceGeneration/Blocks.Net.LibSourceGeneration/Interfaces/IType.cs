using System.Linq.Expressions;
using Blocks.Net.LibSourceGeneration.Definitions;
using Blocks.Net.LibSourceGeneration.References;

namespace Blocks.Net.LibSourceGeneration.Interfaces;

/// <summary>
/// This excludes enums, it is records, structs, classes, and interfaces
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IType<out T> : IVisible<T>, IDocumentable<T>, IAttributable<T>, IGeneric<T>, IAddable<T>
{
    public T Partial();
    public T Sealed();
    public T Abstract();
    public T Readonly();
    public T Record();
    public T Static();

    public T AddField(TypeReference type, string name, out FieldReference field);
    public T AddField(TypeReference type, string name, Action<FieldReference> construct);

    public T AddProperty(TypeReference type, string name, out PropertyReference property);
    public T AddProperty(TypeReference type, string name, Action<PropertyReference> construct);

    public T AddConstructor(out MethodReference method);
    public T AddConstructor(Action<MethodReference> construct);

    public T AddDestructor(out MethodReference method);
    public T AddDestructor(Action<MethodReference> construct);
    
    public T AddConversionOperator(TypeReference to, out MethodReference method);
    public T AddConversionOperator(TypeReference to, Action<MethodReference> construct);

    public T AddMethod(TypeReference returnType, string name, out MethodReference method);

    public T AddMethod(TypeReference returnType, string name, Action<MethodReference> construct);


    /// <summary>
    /// This will always add a static method to this class that is the expression passed in
    /// </summary>
    /// <param name="name">The name of the static method</param>
    /// <param name="method">The method to add to the class</param>
    /// <param name="visibilityLevel">The visibility level of the static method</param>
    /// <returns>The type for method chaining</returns>
    public T AddStaticMethod<T0>(string name, Expression<T0> method, VisibilityLevel visibilityLevel=VisibilityLevel.Public);
    
    
}
