using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.Utilities;
using JetBrains.Annotations;

namespace Blocks.Net.Packets;

public static partial class PacketParser
{
    // Will add a ParsePlay, ParseConfiguration, ParseHandshake, and ParseLogin for 

    public static IPacket ParseHandshaking(Stream stream, PacketState state)
    {
        _ = VarInt.ReadFrom(stream,state);
        var id = VarInt.ReadFrom(stream,state);
        return HandshakingServerBoundPackets.TryGetValue(id, out var cons)
            ? cons(stream, state)
            : throw new Exception($"Unsupported Handshaking Packet ID: {id.Value}");
    }

    public static IPacket ParseStatus(Stream stream, PacketState state)
    {
        _ = VarInt.ReadFrom(stream,state);
        var id = VarInt.ReadFrom(stream,state);
        return StatusServerBoundPackets.TryGetValue(id, out var cons)
            ? cons(stream, state)
            : throw new Exception($"Unsupported Status Packet ID: {id.Value}");
    }

    public static IPacket ParseLogin(Stream stream, PacketState state)
    {
        _ = VarInt.ReadFrom(stream,state);
        var id = VarInt.ReadFrom(stream,state);
        return LoginServerBoundPackets.TryGetValue(id, out var cons)
            ? cons(stream, state)
            : throw new Exception($"Unsupported Login Packet ID: {id.Value}");
    }

    public static IPacket ParseConfiguration(Stream stream, PacketState state)
    {
        _ = VarInt.ReadFrom(stream,state);
        var id = VarInt.ReadFrom(stream,state);
        return ConfigurationServerBoundPackets.TryGetValue(id, out var cons)
            ? cons(stream, state)
            : throw new Exception($"Unsupported Configuration Packet ID: {id.Value}");
    }
}