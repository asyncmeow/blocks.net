using Blocks.Net.Nbt.Utilities;

namespace Blocks.Net.Nbt;

public class CompoundTag : INbtTag
{
    public NbtTagType TagType => NbtTagType.Compound;
    public string? Name { get; }

    public readonly List<INbtTag> ActualChildren = [];
    public INbtTag[] Children => ActualChildren.ToArray();

    public CompoundTag(MemoryStream stream, bool readName=true)
    {
        Name = readName ? stream.ReadLengthPrefixedString() : null;
        var lastTagType = NbtTagType.Compound;
        while (lastTagType != NbtTagType.End)
        {
            var nextTag = INbtTag.Read(stream);
            ActualChildren.Add(nextTag);
            lastTagType = nextTag.TagType;
        }
    }

    public CompoundTag(string? name=null, IEnumerable<INbtTag>? compound = null)
    {
        Name = name;
        ActualChildren = compound?.Append(new EndTag()).ToList() ?? [];
    }

    public INbtTag this[string key]
    {
        get
        {
            foreach (var child in ActualChildren.Where(child => child.Name == key))
            {
                return child;
            }

            throw new KeyNotFoundException(key);
        }
        set
        {
            // Now we must do some list modification
            // Order will *not* be preserved
            int i;
            for (i = 0; i < ActualChildren.Count; i++)
            {
                if (ActualChildren[i].Name == key) break;
            }

            if (i == ActualChildren.Count)
            {
                ActualChildren[i - 1] = value;
                ActualChildren.Add(new EndTag());
            }
            else
            {
                ActualChildren[i] = value;
            }
        }
    }

    public bool TryAdd(string key, INbtTag value)
    {
        
    }
    
    public bool TryGet(string key, out INbtTag value)
    {
        foreach (var child in ActualChildren.Where(child => child.Name == key))
        {
            value = child;
            return true;
        }
        value = null!;
        return false;
    }
    
    public void Remove(string key)
    {
        
        int i;
        for (i = 0; i < ActualChildren.Count; i++)
        {
            if (ActualChildren[i].Name == key) break;
        }
        if (i != ActualChildren.Count) ActualChildren.RemoveAt(i);
    }
    
    public void WriteNetwork(MemoryStream stream)
    {
        stream.WriteByte((byte)TagType);
        foreach (var child in ActualChildren)
        {
            child.Write(stream);
        }
    }

    public void Write(MemoryStream stream)
    {
        stream.WriteByte((byte)TagType);
        stream.WriteLengthPrefixedString(Name!);
        foreach (var child in ActualChildren)
        {
            child.Write(stream);
        }
    }
}