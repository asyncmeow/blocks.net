using Blocks.Net.Packets.Primitives;

namespace Blocks.Net.Packets;

public static partial class ClientboundPacketParser
{
    

    public static IPacket ParseStatus(Stream stream, PacketState state)
    {
        _ = VarInt.ReadFrom(stream,state);
        var id = VarInt.ReadFrom(stream,state);
        return StatusClientBoundPackets.TryGetValue(id, out var cons)
            ? cons(stream, state)
            : throw new Exception($"Unsupported Status Packet ID: {id.Value}");
    }

    public static IPacket ParseLogin(Stream stream, PacketState state)
    {
        _ = VarInt.ReadFrom(stream,state);
        var id = VarInt.ReadFrom(stream,state);
        return LoginClientBoundPackets.TryGetValue(id, out var cons)
            ? cons(stream, state)
            : throw new Exception($"Unsupported Login Packet ID: {id.Value}");
    }

    public static IPacket ParseConfiguration(Stream stream, PacketState state)
    {
        
        _ = VarInt.ReadFrom(stream,state);
        var id = VarInt.ReadFrom(stream,state);
        return ConfigurationClientBoundPackets.TryGetValue(id, out var cons)
            ? cons(stream, state)
            : throw new Exception($"Unsupported Configuration Packet ID: {id.Value}");
    }
    
    
    public static IPacket ParsePlay(Stream stream, PacketState state)
    {
        
        _ = VarInt.ReadFrom(stream,state);
        var id = VarInt.ReadFrom(stream,state);
        return PlayClientBoundPackets.TryGetValue(id, out var cons)
            ? cons(stream, state)
            : throw new Exception($"Unsupported Play Packet ID: {id.Value}");
    }
}