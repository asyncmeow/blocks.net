using Blocks.Net.Packets.Utilities;

namespace Blocks.Net.Packets.Primitives;

public readonly struct Angle(byte fractionalTurns) : IPrimitive
{
    public double Degrees => fractionalTurns * 360d / 256d;
    public byte FractionalTurns => fractionalTurns;

    public static implicit operator byte(Angle angle) => angle.FractionalTurns;
    public static implicit operator Angle(byte b) => new(b);
    public static explicit operator double(Angle angle) => angle.Degrees;
    public static explicit operator Angle(double d) => new((byte)(d * 256d / 360d));

    public static Angle ReadFrom(Stream stream, PacketState state) => (byte)UnsignedByte.ReadFrom(stream, state);

    public void WriteTo(Stream stream, PacketState state) => new UnsignedByte(fractionalTurns).WriteTo(stream, state);
}