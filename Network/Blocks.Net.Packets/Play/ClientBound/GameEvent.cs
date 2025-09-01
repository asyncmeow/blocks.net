using Blocks.Net.PacketSourceGenerator.Attributes;

namespace Blocks.Net.Packets.Play.ClientBound;

[Packet("game_event", true, "Play")]
public partial class GameEvent : IPacket
{
    public enum Events : byte
    {
        NoRespawnBlockAvailable,
        BeginRaining,
        EndRaining,
        ChangeGameMode,
        WinGame,
        DemoEvent,
        ArrowHitPlayer,
        RainLevelChange,
        ThunderLevelChange,
        PlayPufferFishStingSound,
        PlayElderGuardianMobAppearance,
        EnableRespawnScreen,
        LimitedCrafting,
        StartWaitingForLevelChunks
    }

    [PacketEnum(typeof(byte))] public Events Event;
    [PacketField] public float Value;
}