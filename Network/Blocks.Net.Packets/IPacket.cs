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
    public void Write(MemoryStream stream);
   
    /// <summary>
    /// Returns the ID of this packet
    /// </summary>
    public int PacketId { get; }
    
    // Once both of those are source genned uncomment the calls in this
    
    /// <summary>
    /// Writes the packet to a memory stream (with it's ID and length)
    /// </summary>
    /// <param name="stream">The stream to write the packet to</param>
    public void WriteToStream(Stream stream)
    {
        using var subStream = new MemoryStream();
        ((VarInt)PacketId).WriteTo(subStream);
        Write(subStream);
        VarInt length = (int)subStream.Length;
        length.WriteTo(stream);
        subStream.Seek(0, SeekOrigin.Begin);
        subStream.CopyTo(stream);
    }
}