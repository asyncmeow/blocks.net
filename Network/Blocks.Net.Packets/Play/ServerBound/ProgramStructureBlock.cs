using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("set_structure_block",false,"Play")]
public partial class ProgramStructureBlock : IPacket
{
    public enum Actions
    {
        Update,
        Save,
        Load,
        Detect
    }

    public enum Modes
    {
        Save,
        Load,
        Corner,
        Data
    }

    public enum Mirrors
    {
        None,
        LeftRight,
        FrontBack
    }

    public enum Rotations
    {
        None,
        Clockwise90,
        Clockwise180,
        Counterclockwise90,
    }
    
    [PacketField] public Position Location;
    [PacketEnum(typeof(VarInt))] public Actions Action;
    [PacketEnum(typeof(VarInt))] public Modes Mode;
    [PacketField] public string Name;
    [PacketField] public Byte3 Offset;
    [PacketField] public Byte3 Size;
    [PacketEnum(typeof(VarInt))] public Mirrors Mirror;
    [PacketEnum(typeof(VarInt))] public Rotations Rotation;
    [PacketField] public string Metadata;
    [PacketField] public float Integrity;
    [PacketField] public VarLong Seed;
    [PacketField] public byte Flags;
}