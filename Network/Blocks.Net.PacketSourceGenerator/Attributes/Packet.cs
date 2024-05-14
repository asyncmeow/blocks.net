using JetBrains.Annotations;

namespace Blocks.Net.PacketSourceGenerator.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class Packet(byte id, bool clientBound = true, string state = "Play") : Attribute;