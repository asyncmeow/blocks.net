using Blocks.Net.Packets.Primitives;

namespace Blocks.Net.Packets;

public partial interface IPacket
{
    // Each full class/struct implementing this class also needs a static method source genned
    // public static T ReadFrom(MemoryStream stream), where the stream passed in has already had the length and packet ID read
    
    
    /// <summary>
    /// Writes the packet to a memory stream (without ID/length)
    /// </summary>
    /// <param name="stream">The stream to write the packet to</param>
    /// <param name="state">The current state of the packet stream for the connection</param>
    public void Write(Stream stream, PacketState state);
   
    /// <summary>
    /// Returns the ID of this packet
    /// </summary>
    public int PacketId { get; }


    // TODO: Maybe have a thread local buffer for all packets for writing to a stream
    
    /// <summary>
    /// Writes the packet to a memory stream (with it's ID and length)
    /// </summary>
    /// <param name="stream">The stream to write the packet to</param>
    /// <param name="state">The current state of the packet stream for the connection</param>
    public void WriteToStream(Stream stream, PacketState state)
    {
        using var subStream = new MemoryStream();
        ((VarInt)PacketId).WriteTo(subStream, state);
        Write(subStream, state);
        VarInt length = (int)subStream.Length;
        length.WriteTo(stream, state);
        subStream.Seek(0, SeekOrigin.Begin);
        subStream.CopyTo(stream);
    }
}