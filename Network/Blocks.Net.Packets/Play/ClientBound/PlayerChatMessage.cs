using System.Runtime.CompilerServices;
using Blocks.Net.Nbt;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x37,true,"Play")]
public partial class PlayerChatMessage : IPacket
{
    #region Header

    [PacketField] public Guid Sender;
    [PacketField] public VarInt Index;
    [PacketField] public bool MessageSignaturePresent;
    [PacketOptionalArrayField("MessageSignaturePresent", "256")]
    public byte[] MessageSignature;
    #endregion

    #region Body
    [PacketField] public string Message;
    [PacketField] public long Timestamp;
    [PacketField] public long Salt;
    #endregion

    #region Previous Messages
    [PacketField] public VarInt TotalPreviousMessages;

    [PacketArrayField("TotalPreviousMessages")]
    public PreviousMessage[] PreviousMessages;

    #endregion

    #region Other

    public enum FilterTypeEnum
    {
        PassThrough = 0x00,
        FullyFiltered = 0x01,
        PartiallyFiltered = 0x02
    }
    
    [PacketField] public bool UnsignedContentPresent;

    [PacketOptionalField("UnsignedContentPresent")]
    public NbtTag UnsignedContent;

    [PacketEnum(typeof(VarInt))] public FilterTypeEnum FilterType;

    [PacketOptionalField("FilterType == FilterTypeEnum.PartiallyFiltered")]
    public BitSet FilterTypeBits;

    #endregion

    #region Chat
    [PacketField] public VarInt ChatType;
    [PacketField] public NbtTag SenderName;
    [PacketField] public bool HasTargetName;
    [PacketOptionalField("HasTargetName")] public NbtTag TargetName;
    #endregion
}