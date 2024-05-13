using Blocks.Net.Packets.Primitives;

namespace Blocks.Net.Packets;

public partial interface IPacket
{
    // Each full class/struct implementing this class also needs a static method source genned
    // public static T ReadFrom(MemoryStream stream), where the stream passed in has already had the length and packet ID read
    
    // Needs to be source genned
    // protected void Write(MemoryStream stream)
   
    // Also needs to be source genned
    // public byte PacketId { get; }
    
    // Once both of those are source genned uncomment the calls in this
    public void WriteToStream(MemoryStream stream)
    {
        using var subStream = new MemoryStream();
        // subStream.WriteByte(PacketId);
        // Write(subStream)
        VarInt length = (int)subStream.Length;
        length.WriteTo(stream);
        stream.Write(subStream.ToArray());
    }
}