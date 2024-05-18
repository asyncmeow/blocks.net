﻿using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet(0x06,true,"Play")]
public partial class SetBlockDestroyStage : IPacket
{
    [PacketField] public VarInt EntityId;
    [PacketField] public Position Location;
    [PacketField] public byte DestroyStage;
}