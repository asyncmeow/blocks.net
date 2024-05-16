using System.Text;

namespace Blocks.Net.LibSourceGeneration.Interfaces;

public interface IBuildable
{
    
    /// <summary>
    /// Write the current node into <paramref name="builder"/>
    /// </summary>
    /// <param name="builder">The <see cref="StringBuilder"/> being built to</param>
    /// <param name="indentation">The indentation string currently being used</param>
    /// <param name="indentationLevel">The </param>
    /// <returns><paramref name="builder"/></returns>
    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel);
}