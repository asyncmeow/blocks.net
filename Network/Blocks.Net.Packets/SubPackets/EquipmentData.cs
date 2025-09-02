using Blocks.Net.Packets.SubPackets.Slots;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.SubPackets;

[SubPacket]
public partial struct EquipmentData
{
    public enum Slots : byte
    {
        MainHand,
        OffHand,
        Boots,
        Leggings,
        ChestPlate,
        Helmet,
        Body,
        Saddle
    }
    
    [PacketEnum(typeof(byte))] public Slots EquipmentSlot;
    [PacketField] public Slot Item;
}