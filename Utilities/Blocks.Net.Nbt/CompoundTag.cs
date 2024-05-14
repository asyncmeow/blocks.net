using Blocks.Net.Nbt.Utilities;

namespace Blocks.Net.Nbt;

public sealed class CompoundTag : NbtTag
{
    public override NbtTagType TagType => NbtTagType.Compound;
    public override string? Name { get; set; }

    public readonly List<NbtTag> ActualChildren = [];
    public override NbtTag[] Children => ActualChildren.ToArray();

    public CompoundTag(Stream stream, bool readName=true)
    {
        Name = readName ? stream.ReadLengthPrefixedString() : null;
        var lastTagType = NbtTagType.Compound;
        while (lastTagType != NbtTagType.End)
        {
            var nextTag = NbtTag.Read(stream);
            ActualChildren.Add(nextTag);
            lastTagType = nextTag.TagType;
        }
    }

    public CompoundTag(string? name=null, IEnumerable<NbtTag>? compound = null)
    {
        Name = name;
        ActualChildren = compound?.Append(new EndTag()).ToList() ?? [new EndTag()];
    }

    /// <summary>
    /// Index the compound tag
    /// </summary>
    /// <param name="key">The NBT tag name</param>
    /// <exception cref="KeyNotFoundException">If the corresponding NBT tag is not found when reading</exception>
    public NbtTag this[string key]
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
            value.Name = key;
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

    
    public bool TryAdd(string key, NbtTag value)
    {
        value.Name = key;
        int i;
        for (i = 0; i < ActualChildren.Count; i++)
        {
            if (ActualChildren[i].Name == key) break;
        }
        if (i != ActualChildren.Count) return false;
        ActualChildren[i - 1] = value;
        ActualChildren.Add(new EndTag());
        return true;
    }

    public void Add(string key, NbtTag value)
    {
        value.Name = key;
        int i;
        for (i = 0; i < ActualChildren.Count; i++)
        {
            if (ActualChildren[i].Name == key) break;
        }
        if (i != ActualChildren.Count) throw new ArgumentException(key);
        ActualChildren[i - 1] = value;
        ActualChildren.Add(new EndTag());
    }

    public bool TryAdd(NbtTag value) => TryAdd(value.Name!, value);
    public void Add(NbtTag value) => Add(value.Name!, value);
    
    public bool TryGet(string key, out NbtTag value)
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
    

    public override void WriteData(Stream stream)
    {
        foreach (var child in ActualChildren)
        {
            child.Write(stream);
        }
    }
}