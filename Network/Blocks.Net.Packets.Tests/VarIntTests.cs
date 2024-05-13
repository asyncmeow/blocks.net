using System.Runtime.CompilerServices;
using Blocks.Net.Packets.Primitives;

namespace Blocks.Net.Packets.Tests;

[TestClass]
public class VarIntTests
{
    /// <summary>
    /// Tests if the var int class round trips values to and from a memory stream correctly
    /// </summary>
    /// <param name="x">The value to test round trip</param>
    /// <exception cref="Exception">Thrown if the value read from the memory stream is different than the one written to it</exception>
    [TestMethod]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(0x80)]
    [DataRow(0x8000)]
    [DataRow(0x800000)]
    [DataRow(235236441)]
    [DataRow(-1)]
    [DataRow(-10000000)]
    public void VarIntRoundTrip(int x)
    {
        using var stream = new MemoryStream();
        VarInt y = x;
        y.WriteTo(stream);
        stream.Seek(0, SeekOrigin.Begin);
        var z = VarInt.ReadFrom(stream);
        if (z != x)
        {
            throw new Exception($"Round trip failed, got {z.Value} expected {x}");
        }
    }


    /// <summary>
    /// Tests if short values (values &lt; 128) get encoded as a single byte
    /// </summary>
    /// <param name="x">The value to test, must be less than 128</param>
    /// <exception cref="Exception">Thrown if it does not get encoded as such</exception>
    [TestMethod]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(0x7F)]
    public void ShortGetsEncodedCorrectly(int x)
    {
        using var stream = new MemoryStream();
        VarInt y = x;
        y.WriteTo(stream);
        var array = stream.ToArray();
        if (array.Length == 1 && array[0] == x) return;
        var current = string.Join(", ", array.Select(z => $"0x{z:X2}"));
        throw new Exception($"Short value {x} is encoded incorrectly, it is encoded as [{current}]");
    }

    
    
    // These methods are testing the samples from wiki.vg
    // Listed here: https://wiki.vg/Protocol#VarInt_and_VarLong
    #region Wiki Samples
    [TestMethod]
    public void WikiTest_128_Write()
    {
        using var stream = new MemoryStream();
        VarInt y = 128;
        y.WriteTo(stream);
        var array = stream.ToArray();
        if (array is [0x80, 0x01]) return;
        var current = string.Join(", ", array.Select(z => $"0x{z:X2}"));
        throw new Exception($"128 is encoded incorrectly, it is encoded as [{current}]");
    }
    
    [TestMethod]
    public void WikiTest_128_Read()
    {
        using var stream = new MemoryStream([0x80, 0x01],false);
        var y = VarInt.ReadFrom(stream);
        if (y == 128) return;
        throw new Exception($"128 was not decoded correctly, it was decoded as {y.Value}");
    }
    
    [TestMethod]
    public void WikiTest_255_Write()
    {
        using var stream = new MemoryStream();
        VarInt y = 255;
        y.WriteTo(stream);
        var array = stream.ToArray();
        if (array is [0xff, 0x01]) return;
        var current = string.Join(", ", array.Select(z => $"0x{z:X2}"));
        throw new Exception($"255 is encoded incorrectly, it is encoded as [{current}]");
    }
    
    [TestMethod]
    public void WikiTest_255_Read()
    {
        using var stream = new MemoryStream([0xff, 0x01],false);
        var y = VarInt.ReadFrom(stream);
        if (y == 255) return;
        throw new Exception($"255 was not decoded correctly, it was decoded as {y.Value}");
    }
    
    [TestMethod]
    public void WikiTest_25565_Write()
    {
        using var stream = new MemoryStream();
        VarInt y = 25565;
        y.WriteTo(stream);
        var array = stream.ToArray();
        if (array is [0xdd, 0xc7, 0x01]) return;
        var current = string.Join(", ", array.Select(z => $"0x{z:X2}"));
        throw new Exception($"25565 is encoded incorrectly, it is encoded as [{current}]");
    }
    
    [TestMethod]
    public void WikiTest_25565_Read()
    {
        using var stream = new MemoryStream([0xdd, 0xc7, 0x01],false);
        var y = VarInt.ReadFrom(stream);
        if (y == 25565) return;
        throw new Exception($"25565 was not decoded correctly, it was decoded as {y.Value}");
    }
    
    [TestMethod]
    public void WikiTest_2097151_Write()
    {
        using var stream = new MemoryStream();
        VarInt y = 2097151;
        y.WriteTo(stream);
        var array = stream.ToArray();
        if (array is [0xff, 0xff, 0x7f]) return;
        var current = string.Join(", ", array.Select(z => $"0x{z:X2}"));
        throw new Exception($"2097151 is encoded incorrectly, it is encoded as [{current}]");
    }
    
    [TestMethod]
    public void WikiTest_2097151_Read()
    {
        using var stream = new MemoryStream([0xff, 0xff, 0x7f],false);
        var y = VarInt.ReadFrom(stream);
        if (y == 2097151) return;
        throw new Exception($"2097151 was not decoded correctly, it was decoded as {y.Value}");
    }
    
    
    [TestMethod]
    public void WikiTest_2147483647_Write()
    {
        using var stream = new MemoryStream();
        VarInt y = 2147483647;
        y.WriteTo(stream);
        var array = stream.ToArray();
        if (array is [0xff, 0xff, 0xff, 0xff, 0x07]) return;
        var current = string.Join(", ", array.Select(z => $"0x{z:X2}"));
        throw new Exception($"2147483647 is encoded incorrectly, it is encoded as [{current}]");
    }
    
    [TestMethod]
    public void WikiTest_2147483647_Read()
    {
        using var stream = new MemoryStream([0xff, 0xff, 0xff, 0xff, 0x07],false);
        var y = VarInt.ReadFrom(stream);
        if (y == 2147483647) return;
        throw new Exception($"2147483647 was not decoded correctly, it was decoded as {y.Value}");
    }
    
    [TestMethod]
    public void WikiTest_Negative_1_Write()
    {
        using var stream = new MemoryStream();
        VarInt y = -1;
        y.WriteTo(stream);
        var array = stream.ToArray();
        if (array is [0xff, 0xff, 0xff, 0xff, 0x0F]) return;
        var current = string.Join(", ", array.Select(z => $"0x{z:X2}"));
        throw new Exception($"-1 is encoded incorrectly, it is encoded as [{current}]");
    }
    
    [TestMethod]
    public void WikiTest_Negative_1_Read()
    {
        using var stream = new MemoryStream([0xff, 0xff, 0xff, 0xff, 0x0F],false);
        var y = VarInt.ReadFrom(stream);
        if (y == -1) return;
        throw new Exception($"-1 was not decoded correctly, it was decoded as {y.Value}");
    }
    
    [TestMethod]
    public void WikiTest_Negative_2147483648_Write()
    {
        using var stream = new MemoryStream();
        VarInt y = -2147483648;
        y.WriteTo(stream);
        var array = stream.ToArray();
        if (array is [0x80, 0x80, 0x80, 0x80, 0x08]) return;
        var current = string.Join(", ", array.Select(z => $"0x{z:X2}"));
        throw new Exception($"-2147483648 is encoded incorrectly, it is encoded as [{current}]");
    }
    
    [TestMethod]
    public void WikiTest_Negative_2147483648_Read()
    {
        using var stream = new MemoryStream([0x80, 0x80, 0x80, 0x80, 0x08],false);
        var y = VarInt.ReadFrom(stream);
        if (y == -2147483648) return;
        throw new Exception($"-2147483648 was not decoded correctly, it was decoded as {y.Value}");
    }
    #endregion
}