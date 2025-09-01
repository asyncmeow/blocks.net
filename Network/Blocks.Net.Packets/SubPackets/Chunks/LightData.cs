using Blocks.Net.Packets.Primitives;
using Blocks.Net.PacketSourceGenerator.Attributes;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.SubPackets.Chunks;

[SubPacket]
[PublicAPI]
public partial struct LightData
{
    [PacketField] public BitSet SkyLightMask;
    [PacketField] public BitSet BlockLightMask;
    [PacketField] public BitSet EmptySkyLightMask;
    [PacketField] public BitSet EmptyBlockLightMask;
    [PacketField] public LightArray[] SkyLightArrays;
    [PacketField] public LightArray[] BlockLightArrays;

    public int SkyLightCount
    {
        get
        {
            var result = 0;
            for (var i = 0; i < SkyLightMask.Length; i++)
            {
                if (SkyLightMask[i]) result++;
            }

            return result;
        }
    }
    
    

    public int BlockLightCount
    {
        get
        {
            var result = 0;
            for (var i = 0; i < SkyLightMask.Length; i++)
            {
                if (SkyLightMask[i]) result++;
            }

            return result;
        }
    }
}