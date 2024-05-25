namespace Blocks.Net.PacketSourceGenerator.Attributes;

/// <summary>
/// Generate .ReadFrom and .WriteTo methods for this class like it were some form of sub packet
/// (Mostly used for generating new primitive types based on structured data)
///
/// When using the extra args, the args are named _0, _1, _2, ...
/// </summary>
[AttributeUsage(AttributeTargets.Struct)]
public class SubPacket(params Type[] extraArgs) : Attribute
{
    public IEnumerable<Type> ExtraArgs => extraArgs;
}