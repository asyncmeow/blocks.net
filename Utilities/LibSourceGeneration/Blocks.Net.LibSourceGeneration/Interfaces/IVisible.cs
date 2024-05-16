using Blocks.Net.LibSourceGeneration.Definitions;

namespace Blocks.Net.LibSourceGeneration.Interfaces;

public interface IVisible<out T>
{
    public T SetVisibility(VisibilityLevel visibilityLevel);
    public T Public();
    public T Internal();
    public T Protected();
    public T Private();
    public T ImplicitVisibility();
}