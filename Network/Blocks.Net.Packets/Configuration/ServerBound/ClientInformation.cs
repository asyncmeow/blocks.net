using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Configuration.ServerBound;

[PublicAPI]
[Packet(0x00,false,"Configuration")]
public partial class ClientInformation : IPacket
{
    public enum ClientChatMode
    {
        Enabled = 0,
        CommandsOnly = 1,
        Hidden = 2
    }

    public enum ClientMainHand
    {
        Left = 0,
        Right = 1
    }

    [Flags]
    public enum ClientDisplayedSkinParts : byte
    {
        Cape = 0x00,
        Jacket = 0x01,
        LeftSleeve = 0x04,
        RightSleeve = 0x08,
        LeftPants = 0x10,
        RightPants = 0x20,
        Hat = 0x40
    }
    
    [PacketField] public string Locale;
    [PacketField] public byte ViewDistance;
    [PacketEnum(typeof(VarInt))] public ClientChatMode ChatMode;
    [PacketField] public bool ChatColors;
    [PacketEnum(typeof(UnsignedByte))] public ClientDisplayedSkinParts SkinParts;
    [PacketEnum(typeof(VarInt))] public ClientMainHand MainHand;
    [PacketField] public bool EnableTextFiltering;
    [PacketField] public bool AllowServerListings;
}