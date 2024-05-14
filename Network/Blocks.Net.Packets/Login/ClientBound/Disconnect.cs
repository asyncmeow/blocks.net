using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Login.ClientBound;

[PublicAPI]
[Packet(0x00,true,"Login")]
public partial class Disconnect : IPacket
{
    /// <summary>
    /// The JSON Text Component Reason For Disconnecting
    /// </summary>
    [PacketField] public string Reason;
}