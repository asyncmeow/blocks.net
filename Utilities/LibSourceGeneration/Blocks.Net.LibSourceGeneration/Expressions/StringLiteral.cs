using System.Text;
using Blocks.Net.LibSourceGeneration.Interfaces;

namespace Blocks.Net.LibSourceGeneration.Expressions;

public class StringLiteral(string s) : IExpression
{
    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel) =>
        builder.Append(System.Web.HttpUtility.JavaScriptStringEncode(s, true));
}