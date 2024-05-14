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
}