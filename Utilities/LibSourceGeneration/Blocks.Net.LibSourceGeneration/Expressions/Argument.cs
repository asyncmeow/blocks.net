﻿using System.Text;
using Blocks.Net.LibSourceGeneration.Interfaces;

namespace Blocks.Net.LibSourceGeneration.Expressions;

public class Argument(string name, IExpression value) : IExpression
{
    public StringBuilder Build(StringBuilder builder, string indentation, int indentationLevel)
    {
        builder.Append(name);
        return value.Build(builder, indentation, indentationLevel + 1);
    }
}