using Blocks.Net.Nbt;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.PacketSourceGenerator.Attributes;
using Blocks.Net.Text;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("player_chat",true,"Play")]
public partial class PlayerChatMessage : IPacket
{
    #region Header
    [PacketField] public VarInt GlobalIndex;
    [PacketField] public Uuid Sender;
    [PacketField] public VarInt Index;
    [PacketField] public ChatSignature? MessageSignatureBytes;
    #endregion
    
    #region Body
    [PacketField] public string Message;
    [PacketField] public long Timestamp;
    [PacketField] public long Salt;
    #endregion
    
    #region Other
    
    public enum FilterTypes
    {
        PassThrough,
        FullyFiltered,
        PartiallyFiltered
    }
    
    [PacketField] public PreviousMessage[] PreviousMessages;
    [PacketField] public NbtTag? UnsignedContent;
    [PacketEnum(typeof(VarInt))] public FilterTypes FilterType;

    [PacketOptionalField("FilterType == FilterTypes.PartiallyFiltered")]
    public BitSet FilterTypeBits;
    #endregion
    
    #region Chat Formatting
    [PacketField] public IdOrChatType ChatType;
    [PacketField] public TextComponent SenderName;
    [PacketField] public TextComponent? TargetName;
    #endregion
}