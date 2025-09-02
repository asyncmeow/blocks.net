using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("player_input",false,"Play")]
public partial class PlayerInput : IPacket
{
    [Flags]
    public enum Input : byte
    {
        Forward = 1, 
        Backward = 2,
        Left = 4,
        Right = 8,
        Jump = 16,
        Sneak = 32,
        Sprint = 64
    }

    [PacketEnum(typeof(byte))] public Input Inputs;
}