using Blocks.Net.LibSourceGeneration.Builders;

namespace Blocks.Net.LibSourceGeneration.Interfaces;

public interface IDocumentable<out T>
{
    public T WithDocumentation(DocCommentBuilder docCommentBuilder);
}