using System.Text;

namespace Blocks.Net.LibSourceGeneration.Builders;

public class DocCommentBuilder
{
    private string? _inherit;
    
    public static DocCommentBuilder InheritDoc(string inheritingFrom = "")
    {
        return new DocCommentBuilder
        {
            _inherit = inheritingFrom
        };
    }

    public DocCommentBuilder Inherit(string inheritingFrom = "")
    {
        _inherit = inheritingFrom;
        return this;
    }

    private string? _summary;

    private List<string> _seeAlso = [];

    private List<(string name, string desc)> _typeParams = [];
    private List<(string name, string desc)> _params = [];
    private string? _returns;
    public DocCommentBuilder WithSummary(string summary)
    {
        _summary = summary;
        return this;
    }

    public DocCommentBuilder SeeAlso(params string[] cref)
    {
        _seeAlso.AddRange(cref);
        return this;
    }

    public DocCommentBuilder Returns(string returns)
    {
        _returns = returns;
        return this;
    }

    public DocCommentBuilder WithParameter(string name, string desc)
    {
        _params.Add((name, desc));
        return this;
    }

    public DocCommentBuilder WithTypeParameter(string name, string desc)
    {
        _typeParams.Add((name, desc));
        return this;
    }
    public string[] Build()
    {
        if (_inherit != null)
        {
            if (_inherit == "") return ["<inheritdoc/>"];
            return [$"<inheritdoc cref=\"{_inherit}\"/>"];
        }

        List<string> lines = [];
        if (!string.IsNullOrEmpty(_summary))
        {
            lines.Add("<summary>");
            lines.AddRange(_summary!.Split('\n','\r'));
            lines.Add("</summary>");
        }

        foreach (var (name, desc) in _typeParams)
        {
            var full = $"<typeparam name=\"{name}\">{desc}</param>";
            lines.AddRange(full.Split('\n', '\r'));
        }
        
        foreach (var (name, desc) in _params)
        {
            var full = $"<param name=\"{name}\">{desc}</param>";
            lines.AddRange(full.Split('\n', '\r'));
        }

        if (!string.IsNullOrEmpty(_returns))
        {
            var full = $"<returns>{_returns}</returns>";
            lines.AddRange(full.Split('\n', '\r'));
        }

        foreach (var also in _seeAlso)
        {
            lines.Add($"<seealso cref=\"{also}\"/>");
        }

        return lines.ToArray();
    }
}