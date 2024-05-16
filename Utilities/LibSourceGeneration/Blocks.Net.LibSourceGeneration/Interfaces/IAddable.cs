namespace Blocks.Net.LibSourceGeneration.Interfaces;

public interface IAddable<out T>
{
    public T Add(IBuildable buildable);
}