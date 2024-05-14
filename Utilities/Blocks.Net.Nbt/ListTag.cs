using Blocks.Net.Nbt.Utilities;

namespace Blocks.Net.Nbt;

public sealed class ListTag : NbtTag
{
    public override NbtTagType TagType => NbtTagType.List;
    public override string? Name { get; set; }
    public readonly List<NbtTag> Data = [];
    public NbtTagType ChildType;
    public override NbtTag[] Children => Data.ToArray();


    public NbtTag this[int index]
    {
        get => Data[index];
        set
        {
            if (value.TagType != ChildType)
            {
                if (ChildType == NbtTagType.End) ChildType = value.TagType;
                else
                    throw new Exception(
                        $"Cannot insert a value of type {value.TagType} into a list of type {ChildType}");
            }
            Data[index] = value;
        }
    }

    public void Clear()
    {
        Data.Clear();
        ChildType = NbtTagType.End;
    }

    public void RemoveAt(int index)
    {
        Data.RemoveAt(index);
        if (Count == 0) ChildType = NbtTagType.End;
    }

    public int RemoveAll(Predicate<NbtTag> match)
    {
        var cnt = Data.RemoveAll(match);
        if (Count == 0) ChildType = NbtTagType.End;
        return cnt;
    }

    public void Add(NbtTag value)
    {
        if (value.TagType != ChildType)
        {
            if (ChildType == NbtTagType.End) ChildType = value.TagType;
            else
                throw new Exception(
                    $"Cannot insert a value of type {value.TagType} into a list of type {ChildType}");
        }

        Data.Add(value);
    }
    public int Count => Data.Count;
    
    
    public ListTag(Stream stream, bool readName)
    {
        Name = readName ? stream.ReadLengthPrefixedString() : null;
        ChildType = (NbtTagType)stream.CheckedReadByte();
        var bytes = new byte[4];
        stream.ReadExactly(bytes);
        if (BitConverter.IsLittleEndian) bytes = [bytes[3], bytes[2], bytes[1], bytes[0]];
        var length = BitConverter.ToInt32(bytes);
        for (var i = 0; i < length; i++)
        {
            var tag = Read(stream, false);
            if (tag.TagType != ChildType)
            {
                if (ChildType == NbtTagType.End) ChildType = tag.TagType;
                else throw new Exception($"Invalid child tag in list({ChildType}) - {tag.TagType}");
            }
            else
            {
                Data.Add(tag);
            }
        }
    }

    public ListTag(string? name=null, NbtTagType childType = NbtTagType.End, IEnumerable<NbtTag>? tags = null)
    {
        Name = name;
        if (tags == null)
        {
            ChildType = NbtTagType.End;
            return;
        }
        ChildType = childType;
        foreach (var tag in tags)
        {
            Add(tag);
        }
        if (Data.Count == 0) ChildType = NbtTagType.End;
    }
    
    public override void WriteData(Stream stream)
    {
        stream.WriteByte((byte)ChildType);
        var bytes = BitConverter.GetBytes(Data.Count);
        if (BitConverter.IsLittleEndian) bytes = [bytes[3], bytes[2], bytes[1], bytes[0]];
        stream.Write(bytes);
        foreach (var child in Data)
        {
            child.Write(stream, false);
        }
    }
}