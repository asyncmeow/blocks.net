using System.Collections;
using System.Text;
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
            var nextTag = Read(stream);
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


    public IEnumerable<string> Keys => ActualChildren.Take(ActualChildren.Count - 1).Select(x => x.Name!);

    public override void WriteData(Stream stream)
    {
        foreach (var child in ActualChildren)
        {
            child.Write(stream);
        }
    }

    protected override bool IsSameImpl(NbtTag other)
    {
        var otherCompound = (CompoundTag)other;
        var keys = Keys.ToArray();
        var otherKeys = otherCompound.Keys.ToArray();
        if (keys.Length != otherKeys.Length) return false;
        foreach (var key in keys)
        {
            if (!otherKeys.Contains(key)) return false;
            if (!this[key].IsSameAs(otherCompound[key]))
            {
                return false;
            }
        }
        return true;
    }

    public override void DumpImpl(StringBuilder sb, string indentation, int level, bool dumpName)
    {
        if (dumpName && Name != null)
        {
            sb.Append($"Compound({System.Web.HttpUtility.JavaScriptStringEncode(Name,true)}):\n").AppendRepeating(indentation, level).Append("{\n");
        }
        else
        {
            sb.Append("{\n");
        }

        for (var i = 0; i < ActualChildren.Count - 1; i++)
        {
            sb.AppendRepeating(indentation, level + 1);
            ActualChildren[i].DumpImpl(sb, indentation, level + 1, true);
            if (i != ActualChildren.Count - 2) sb.Append(',');
            sb.Append('\n');
        }
        sb.AppendRepeating(indentation, level).Append('}');
    }
}