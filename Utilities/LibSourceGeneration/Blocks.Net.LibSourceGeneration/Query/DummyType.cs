using System.Globalization;
using System.Reflection;
using Blocks.Net.LibSourceGeneration.References;

namespace Blocks.Net.LibSourceGeneration.Query;

public class DummyType(TypeReference t) : Type
{
    
    public override object[] GetCustomAttributes(bool inherit) =>
        throw new InvalidOperationException("Dummy types do not support getting attributes!");

    public override object[] GetCustomAttributes(Type attributeType, bool inherit) =>
        throw new InvalidOperationException("Dummy types do not support getting attributes!");

    public override bool IsDefined(Type attributeType, bool inherit) =>
        throw new InvalidOperationException("Dummy types do not support getting attributes!");

    public override Module Module => throw new InvalidOperationException("Dummy types are not part of a module!");
    public override string Namespace => t.Namespace ?? "";
    public override string Name => t.Name;
    public TypeReference Reference => t;
    protected override TypeAttributes GetAttributeFlagsImpl() =>
        throw new InvalidOperationException("Dummy types do not support getting attributes!");

    protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention,
        Type[] types, ParameterModifier[] modifiers) =>
        throw new InvalidOperationException("Dummy types do not support getting constructors!");

    public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr) =>
        throw new InvalidOperationException("Dummy types do not support getting constructors!");

    public override Type GetElementType() => (Type)Reference.GetElementType();

    public override EventInfo GetEvent(string name, BindingFlags bindingAttr) =>
        throw new InvalidOperationException("Dummy types do not have events!");

    public override EventInfo[] GetEvents(BindingFlags bindingAttr) =>
        throw new InvalidOperationException("Dummy types do not have events!");
    public override FieldInfo GetField(string name, BindingFlags bindingAttr) =>
        throw new InvalidOperationException("Dummy types do not have fields!");

    public override FieldInfo[] GetFields(BindingFlags bindingAttr) =>
        throw new InvalidOperationException("Dummy types do not have fields!");

    public override MemberInfo[] GetMembers(BindingFlags bindingAttr) =>
        throw new InvalidOperationException("Dummy types do not have members!");

    protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention,
        Type[] types, ParameterModifier[] modifiers) =>
        throw new InvalidOperationException("Dummy types do not have methods!");

    public override MethodInfo[] GetMethods(BindingFlags bindingAttr) =>
        throw new InvalidOperationException("Dummy types do not have methods!");
    public override PropertyInfo[] GetProperties(BindingFlags bindingAttr) =>
        throw new InvalidOperationException("Dummy types do not have properties!");
    public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args,
        ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters) =>
        throw new InvalidOperationException("Dummy types do not have methods!");

    public override Type UnderlyingSystemType =>
        throw new Exception("Dummy types do not have underlying system types!");

    protected override bool IsArrayImpl() => Reference.IsArray;
    protected override bool IsByRefImpl()
    {
        throw new NotImplementedException();
    }

    protected override bool IsCOMObjectImpl()
    {
        throw new NotImplementedException();
    }

    protected override bool IsPointerImpl()
    {
        throw new NotImplementedException();
    }

    protected override bool IsPrimitiveImpl()
    {
        throw new InvalidOperationException("It is impossible to determine if a dummy type is a primitive");
    }

    public override Assembly Assembly => throw new InvalidOperationException("Dummy types do not have assemblies!");

    public override string AssemblyQualifiedName =>
        throw new InvalidOperationException("Dummy types do not have an assembly qualified name");

    public override Type BaseType => throw new InvalidOperationException("Dummy types do not have a base type");
    public override string FullName => Reference;
    public override Guid GUID => new(FullName);

    protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types,
        ParameterModifier[] modifiers) =>
        throw new InvalidOperationException("Dummy types do not have properties!");

    protected override bool HasElementTypeImpl() => Reference.IsArray;

    public override Type GetNestedType(string name, BindingFlags bindingAttr) =>
        throw new InvalidOperationException("Dummy types do not have nested types!");

    public override Type[] GetNestedTypes(BindingFlags bindingAttr) =>
        throw new InvalidOperationException("Dummy types do not have nested types!");

    public override Type GetInterface(string name, bool ignoreCase) =>
        throw new InvalidOperationException("Dummy types do not have interfaces!");

    public override Type[] GetInterfaces() =>
        throw new InvalidOperationException("Dummy types do not have interfaces!");
}