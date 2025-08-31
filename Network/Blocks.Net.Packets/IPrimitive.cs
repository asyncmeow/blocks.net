using System.Runtime.CompilerServices;

namespace Blocks.Net.Packets;

public interface IPrimitive
{
    public void WriteTo(Stream stream, PacketState state);
}