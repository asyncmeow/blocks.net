using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("set_camera",true,"Play")]
public partial class SetCamera : IPacket
{
    [PacketField] public VarInt CameraId;
}