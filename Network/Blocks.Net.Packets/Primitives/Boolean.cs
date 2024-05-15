using System.Runtime.CompilerServices;
using Blocks.Net.Packets.Utilities;
using JetBrains.Annotations;

namespace Blocks.Net.Packets.Primitives;

[PublicAPI]
public struct Boolean(bool v)
{
    public bool Value => v;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator bool(Boolean v) => v.Value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Boolean(bool v) => new(v);


    public void WriteTo(Stream stream) => stream.WriteByte((byte)(v ? 1 : 0));

    public static Boolean ReadFrom(MemoryStream stream) => stream.CheckedReadByte() == 1;
}