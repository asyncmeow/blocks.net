using System.Runtime.CompilerServices;
using Blocks.Net.Nbt;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Enums;

[Flags]
[FieldedEnum(typeof(byte))]
public enum PlayerAction
{
    AddPlayer = 0x01,
    InitializeChat = 0x02,
    UpdateGameMode = 0x04,
    UpdateListed = 0x08,
    UpdateLatency = 0x10,
    UpdateDisplayName = 0x20,
    UpdateListPriority = 0x40,
    UpdateHat = 0x80
}

[EnumField(typeof(PlayerAction))]
public partial class PlayerActionAddPlayer
{
    [PacketField] public string Name;
    [PacketField] public VarInt NumberOfProperties;

    [PacketArrayField("NumberOfProperties")]
    public PlayerProperty[] Properties;
}

[EnumField(typeof(PlayerAction))]
public partial class PlayerActionInitializeChat
{
    [PacketField] public bool HasSignatureData;

    [PacketOptionalField("HasSignatureData")]
    public Guid ChatSessionId;

    [PacketOptionalField("HasSignatureData")]
    public long PublicKeyExpiryTime;

    [PacketOptionalField("HasSignatureData")]
    public VarInt EncodedPublicKeySize;

    [PacketOptionalArrayField("HasSignatureData", "EncodedPublicKeySize")]
    public byte[] EncodedPublicKey;

    [PacketOptionalField("HasSignatureData")]
    public VarInt PublicKeySignatureSize;

    [PacketOptionalArrayField("HasSignatureData", "PublicKeySignatureSize")]
    public byte[] PublicKeySignature;
}

[EnumField(typeof(PlayerAction))]
public partial class PlayerActionUpdateGameMode
{
    [PacketField] public VarInt GameMode;
}

[EnumField(typeof(PlayerAction))]
public partial class PlayerActionUpdateListed
{
    [PacketField] public bool Listed;
}

[EnumField(typeof(PlayerAction))]
public partial class PlayerActionUpdateLatency
{
    [PacketField] public VarInt Ping;
}

[EnumField(typeof(PlayerAction))]
public partial class PlayerActionUpdateDisplayName
{
    [PacketField] public NbtTag? DisplayName;
}

[EnumField(typeof(PlayerAction))]
public partial class PlayerActionUpdateListPriority
{
    [PacketField] public VarInt Priority;
}

[EnumField(typeof(PlayerAction))]
public partial class PlayerActionUpdateHat
{
    [PacketField] public bool Visible;
}