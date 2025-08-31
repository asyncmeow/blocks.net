using Blocks.Net.Packets.Utilities;

namespace Blocks.Net.Packets.Primitives;

public struct Position(int x, int y, int z) : IPrimitive
{
    public int X => x;
    public int Y => y;
    public int Z => z;

    public void Deconstruct(out int posX, out int posY, out int posZ)
    {
        posX = X;
        posY = Y;
        posZ = Z;
    }

    public static Position ReadFrom(Stream stream, PacketState state)
    {
        var l = Long.ReadFrom(stream, state).Value;
        var x = l >> 38;
        var y = l << 52 >> 52;
        var z = l << 26 >> 38;
        return new Position((int)x, (int)y, (int)z);
    }

    public void WriteTo(Stream stream, PacketState state)
    {
        Long l = ((x & 0x3FFFFFFL) << 38) | ((z & 0x3FFFFFFL) << 12) | (y & 0xFFFL);
        l.WriteTo(stream,state);
    }
}