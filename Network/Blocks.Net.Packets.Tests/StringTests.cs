namespace Blocks.Net.Packets.Tests;

[TestClass]
public class StringTests
{
    [TestMethod]
    [DataRow("Hello, World!")]
    [DataRow("\ud83d\ude00\ud83d\ude00\ud83d\ude00")]
    [DataRow("これはテストです")]
    public void StringRoundTripTesting(string value)
    {
        using var stream = new MemoryStream();
        Primitives.String x = value;
        x.WriteTo(stream);
        stream.Seek(0, SeekOrigin.Begin);
        var result = Primitives.String.ReadFrom(stream);
        if (result == value) return;
        throw new Exception($"Failed round trip for {value}, got {result.Value}");
    }

    [TestMethod]
    [DataRow(0x0b,0x31,0x39,0x32,0x2E,0x31,0x36,0x38,0x2E,0x32,0x2E,0x37, "192.168.2.7")]
    public void StringReadTesting(object[] value)
    {
        var data = value.Take(value.Length - 1).Select(x => (byte)(int)x).ToArray();
        var expected = (string)value.Last();
        // Console.WriteLine(value.Last());
        using var stream = new MemoryStream(data,false);
        var x = Primitives.String.ReadFrom(stream);
        if (x.Value == expected) return;
        throw new Exception($"Failed to parse string {expected} got {x}");
    }
}