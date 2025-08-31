using Blocks.Net.DataTypes;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets.Slots.Components;

[SubPacket]
public partial struct AttributeModifier
{
    public enum OperationEnum
    {
        Add,
        MultiplyBase,
        MultiplyTotal
    }

    public enum SlotEnum
    {
        Any,
        MainHand,
        OffHand,
        Hand,
        Feet,
        Legs,
        Chest,
        Head,
        Armor,
        Body
    }
    
    [PacketField] public RegistryReference AttributeId;
    [PacketField] public Identifier ModifierId;
    [PacketField] public double Value;
    [PacketEnum(typeof(VarInt))] public OperationEnum Operation;
    [PacketEnum(typeof(VarInt))] public SlotEnum Slot;
}