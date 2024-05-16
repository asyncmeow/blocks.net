using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.Utilities;

namespace Blocks.Net.Packets;

public static partial class PacketParser
{
    // Will add a ParsePlay, ParseConfiguration, ParseHandshake, and ParseLogin for 

    public static IPacket ParseHandshaking(MemoryStream stream)
    {
        _ = VarInt.ReadFrom(stream);
        var id = VarInt.ReadFrom(stream);
        // if (HandshakingServerBoundPackets.TryGetValue(id, out var cons)) return cons(stream);
        throw new Exception($"Unsupported Handshaking Packet ID: {id}");
    }    
    
    public static IPacket ParseStatus(MemoryStream stream)
    {
        _ = VarInt.ReadFrom(stream);
        var id = VarInt.ReadFrom(stream);
        // if (StatusServerBoundPackets.TryGetValue(id, out var cons)) return cons(stream);
        throw new Exception($"Unsupported Status Packet ID: {id}");
    }
    
    public static IPacket ParseLogin(MemoryStream stream)
    {
        _ = VarInt.ReadFrom(stream);
        var id = VarInt.ReadFrom(stream);
        // if (LoginServerBoundPackets.TryGetValue(id, out var cons)) return cons(stream);
        throw new Exception($"Unsupported Login Packet ID: {id}");
    }
    
    public static IPacket ParseConfiguration(MemoryStream stream)
    {
        _ = VarInt.ReadFrom(stream);
        var id = VarInt.ReadFrom(stream);
        // if (ConfigurationServerBoundPackets.TryGetValue(id, out var cons)) return cons(stream);
        throw new Exception($"Unsupported Configuration Packet ID: {id}");
    }
}