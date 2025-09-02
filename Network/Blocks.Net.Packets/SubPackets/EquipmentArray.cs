using JetBrains.Annotations;

namespace Blocks.Net.Packets.SubPackets;

[PublicAPI]
public struct EquipmentArray(EquipmentData[] equipmentData) : IPrimitive
{
    public EquipmentData[] EquipmentData = equipmentData;
    public ref EquipmentData this[int index] => ref EquipmentData[index];
    public void WriteTo(Stream stream, PacketState state)
    {

        for (var i = 0; i < EquipmentData.Length; i++)
        {
            if (i == EquipmentData.Length - 1)
            {
                var copy = EquipmentData[i];
                copy.EquipmentSlot = (SubPackets.EquipmentData.Slots)((byte)copy.EquipmentSlot | 0x80);
                copy.WriteTo(stream,state);
            }
            else
            {
                EquipmentData[i].WriteTo(stream, state);
            }
        }
    }

    public static EquipmentArray ReadFrom(Stream stream, PacketState state)
    {
        List<EquipmentData> equipmentData = [];
        var running = true;
        while (running)
        {
            var result = SubPackets.EquipmentData.ReadFrom(stream, state);
            if (((byte)result.EquipmentSlot & 0x80) != 0)
            {
                result.EquipmentSlot = (SubPackets.EquipmentData.Slots)((byte)result.EquipmentSlot & 0x7F);
                running = false;
            }
            equipmentData.Add(result);
        }
        return new EquipmentArray(equipmentData.ToArray());
    }
}