using System.IO.Compression;

namespace Blocks.Net.Nbt.Tests;

[TestClass]
public class NbtTests
{
    public readonly byte[] HelloWorld;
    public readonly byte[] BigTest;

    public NbtTests()
    {
        HelloWorld = File.ReadAllBytes("hello_world.nbt");
        using var compressedFileStream = File.Open("bigtest.nbt", FileMode.Open);
        using var uncompressedFileStream = new MemoryStream();
        using var decompressor = new GZipStream(compressedFileStream, CompressionMode.Decompress);
        decompressor.CopyTo(uncompressedFileStream);
        BigTest = uncompressedFileStream.ToArray();
    }
    
    [TestMethod]
    public void HelloWorldGenerating()
    {
        // If we generate the hello world file, it should exactly match the hello world.nbt file
        var rootTag = new CompoundTag("hello world")
        {
            ["name"] = "Bananrama"
        };
        using var stream = new MemoryStream();
        rootTag.Write(stream);
        var generated = stream.ToArray();
        if (generated.SequenceEqual(HelloWorld)) return;
        var string1 = $"{HelloWorld.Length} : [{string.Join(", ", HelloWorld.Select(x => $"{x:X2}"))}]";
        var string2 = $"{generated.Length} : [{string.Join(", ", generated.Select(x => $"{x:X2}"))}]";
        throw new Exception(
            $"Failed to generate the same structure for the hello world test\nExpected:\n\t{string1}\nGot:\n\t{string2}");
    }

    [TestMethod]
    public void HelloWorldParsing()
    {
        using var stream = new MemoryStream(HelloWorld, false);
        var rootTag = NbtTag.Read(stream);
        var testTag = new CompoundTag("hello world")
        {
            ["name"] = "Bananrama"
        };
        if (rootTag.IsSameAs(testTag, true)) return;
        throw new Exception($"Failed to parse an NBT tree\nExpected:\n{testTag.Dump()}\nGot:\n{rootTag.Dump()}");
    }

    [TestMethod]
    public void BigTestGenerating()
    {
        var bytes = new sbyte[1000];
        for (var n = 0; n < 1000; n++)
        {
            bytes[n] = (sbyte)((n * n * 255 + n * 7) % 100);
        }
        var byteArray = new ByteArrayTag(null,bytes);
        
        // The way this is ordered is meant to exactly match the desired output
        var rootTag = new CompoundTag("Level")
        {
            ["longTest"] = 9223372036854775807L,
            ["shortTest"] = (short)32767,
            ["stringTest"] = "HELLO WORLD THIS IS A TEST STRING \xc5\xc4\xd6!",
            ["floatTest"] = 0.49823147058486938f,
            ["intTest"] = 2147483647,
            ["nested compound test"] = new CompoundTag
            {
                ["ham"] = new CompoundTag
                {
                    ["name"] ="Hampus",
                    ["value"] = 0.75f
                },
                ["egg"] = new CompoundTag
                {
                    ["name"] = "Eggbert",
                    ["value"] = 0.5f
                },
            },
            ["listTest (long)"] = new ListTag
            {
                11L,
                12L,
                13L,
                14L,
                15L
            },
            ["listTest (compound)"] = new ListTag
            {
                new CompoundTag
                {
                    ["name"] = "Compound tag #0",
                    ["created-on"] = 1264099775885L,
                },
                new CompoundTag
                {
                    ["name"] = "Compound tag #1",
                    ["created-on"] = 1264099775885L,
                }
            },
            ["byteTest"] = (sbyte)127,
            ["byteArrayTest (the first 1000 values of (n*n*255+n*7)%100, starting with n=0 (0, 62, 34, 16, 8, ...))"] = byteArray,
            ["doubleTest"] = 0.49312871321823148d,
        };
        using var stream = new MemoryStream();
        rootTag.Write(stream);
        var generated = stream.ToArray();
        if (generated.SequenceEqual(BigTest)) return;
        var string1 = $"{BigTest.Length} : [{string.Join(", ", BigTest.Select(x => $"{x:X2}"))}]";
        var string2 = $"{generated.Length} : [{string.Join(", ", generated.Select(x => $"{x:X2}"))}]";
        throw new Exception(
            $"Failed to generate the same structure for the big test\nExpected:\n\t{string1}\nGot:\n\t{string2}");
    }

    [TestMethod]
    public void BigTestParsing()
    {
        var bytes = new sbyte[1000];
        for (var n = 0; n < 1000; n++)
        {
            bytes[n] = (sbyte)((n * n * 255 + n * 7) % 100);
        }
        var byteArray = new ByteArrayTag(null,bytes);
        using var stream = new MemoryStream(BigTest, false);
        var rootTag = NbtTag.Read(stream);
        var testTag = new CompoundTag("Level")
        {
            ["longTest"] = 9223372036854775807L,
            ["shortTest"] = (short)32767,
            ["stringTest"] = "HELLO WORLD THIS IS A TEST STRING \xc5\xc4\xd6!",
            ["floatTest"] = 0.49823147058486938f,
            ["intTest"] = 2147483647,
            ["nested compound test"] = new CompoundTag
            {
                ["ham"] = new CompoundTag
                {
                    ["name"] ="Hampus",
                    ["value"] = 0.75f
                },
                ["egg"] = new CompoundTag
                {
                    ["name"] = "Eggbert",
                    ["value"] = 0.5f
                },
            },
            ["listTest (long)"] = new ListTag
            {
                11L,
                12L,
                13L,
                14L,
                15L
            },
            ["listTest (compound)"] = new ListTag
            {
                new CompoundTag
                {
                    ["name"] = "Compound tag #0",
                    ["created-on"] = 1264099775885L,
                },
                new CompoundTag
                {
                    ["name"] = "Compound tag #1",
                    ["created-on"] = 1264099775885L,
                }
            },
            ["byteTest"] = (sbyte)127,
            ["byteArrayTest (the first 1000 values of (n*n*255+n*7)%100, starting with n=0 (0, 62, 34, 16, 8, ...))"] = byteArray,
            ["doubleTest"] = 0.49312871321823148d,
        };
        if (rootTag.IsSameAs(testTag, true)) return;
        throw new Exception($"Failed to parse an NBT tree\nExpected:\n{testTag.Dump()}\nGot:\n{rootTag.Dump()}");
    }
}