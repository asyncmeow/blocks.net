using JetBrains.Annotations;

namespace Blocks.Net.PacketSourceGenerator.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class Packet(string id, bool clientBound, string state) : Attribute
{
    public string Id => id;
    public bool ClientBound => clientBound;
    public string State => state;
}