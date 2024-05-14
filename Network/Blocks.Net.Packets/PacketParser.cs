using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.Utilities;

namespace Blocks.Net.Packets;

public static partial class PacketParser
{
    // Will add a ParsePlay, ParseConfiguration, ParseHandshake, and ParseLogin for 

    public static IPacket ParseHandshaking(MemoryStream stream)
    {
        _ = VarInt.ReadFrom(stream);
        var id = stream.CheckedReadByte();
        if (HandshakingServerBoundPackets.TryGetValue(id, out var cons)) return cons(stream);
        throw new Exception($"Unsupported Handshaking Packet ID: {id}");
    }
}