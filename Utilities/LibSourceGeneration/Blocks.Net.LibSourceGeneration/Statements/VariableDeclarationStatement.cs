using System.Text;
using Blocks.Net.LibSourceGeneration.Extensions;
using Blocks.Net.LibSourceGeneration.Interfaces;
using Blocks.Net.LibSourceGeneration.References;

namespace Blocks.Net.LibSourceGeneration.Statements;

public class VariableDeclarationStatement(TypeReference? varType, string name, IExpression? init=null) : IStatement
{

    public VariableDeclarationStatement(string name, IExpression init) : this(null, name, init)
    {
        
    }
    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel)
    {
        builder.AppendRepeating(indentation,indentationLevel).Append(varType?.Generate() ?? "var").Append(' ').Append(name);
        if (init != null)
        {
            builder.Append(" = ");
            init.Build(builder, indentation, indentationLevel);
        }
        return builder.Append(";\n");
    }
}