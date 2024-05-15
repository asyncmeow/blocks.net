namespace Blocks.Net.InjectorSourceGenerator;


/// <summary>
/// Tell a source generator to embed a source file into a static field in a static constructor
/// </summary>
/// <param name="sourceName">The short name of the file to embed (with the .cs)</param>
[AttributeUsage(AttributeTargets.Field)]
public class EmbedMe(string sourceName) : Attribute;