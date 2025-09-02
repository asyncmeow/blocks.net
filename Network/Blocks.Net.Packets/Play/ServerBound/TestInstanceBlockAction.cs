using Blocks.Net.DataTypes;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;
using Blocks.Net.Text;

namespace Blocks.Net.Packets.Play.ServerBound;

[Packet("test_instance_block_action",false,"Play")]
public partial class TestInstanceBlockAction : IPacket
{
    public enum Actions
    {
        Initialize,
        Query,
        Set,
        Reset,
        Save,
        Export,
        Run
    }

    public enum Rotations
    {
        None,
        Clockwise90,
        Clockwise180,
        Counterclockwise90,
    }

    public enum Statuses
    {
        Cleared,
        Running,
        Finished
    }
    
    [PacketField] public Position Location;
    [PacketEnum(typeof(VarInt))] public Actions Action;
    [PacketField] public RegistryReference? Test;
    [PacketField] public VarInt3 Size;
    [PacketEnum(typeof(VarInt))] public Rotations Rotation;
    [PacketField] public bool IgnoreEntities;
    [PacketEnum(typeof(VarInt))] public Statuses Status;
    [PacketField] public TextComponent? ErrorMessage;
}