namespace Blocks.Net.LibSourceGeneration.Interfaces;

/// <summary>
/// An expression is another name for a buildable, but with one crucial difference
/// Buildable is expected to start on a new line and end on a newline
/// Expression starts in the middle of a line and ends in the middle of one
/// </summary>
public interface IExpression : IBuildable;