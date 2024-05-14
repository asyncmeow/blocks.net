using JetBrains.Annotations;

namespace Blocks.Net.PacketSourceGenerator.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class Packet(int id, bool clientBound = true, string state = "Play") : Attribute;