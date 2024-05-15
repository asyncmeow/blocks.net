using System.Collections;
using System.Drawing;
using System.Net.Mime;
using System.Text;
using Blocks.Net.Nbt;
using JetBrains.Annotations;

namespace Blocks.Net.Text;

[PublicAPI]
public class TextComponent
{
    #region Implementation
    public enum ComponentType
    {
        Text,
        Translatable,
        KeyBind
    }
    public ComponentType Type = ComponentType.Text;
    public string? Text;
    public List<TextComponent> With = [];
    public string? Color;
    public StylingState Bold = StylingState.Inherit;
    public StylingState Italic = StylingState.Inherit;
    public StylingState Underlined = StylingState.Inherit;
    public StylingState Strikethrough = StylingState.Inherit;
    public StylingState Obfuscated = StylingState.Inherit;
    public string? Font;
    public string? Insertion;
    public ClickEvent? ClickEvent;
    public HoverEvent? HoverEvent;
    
    
    public List<TextComponent> Children = [];

    // ReSharper disable once ConvertToPrimaryConstructor


    public bool IsSimpleTextComponent => Type == ComponentType.Text && Color == null && Bold == StylingState.Inherit &&
                                         Italic == StylingState.Inherit && Underlined == StylingState.Inherit &&
                                         Strikethrough == StylingState.Inherit && Obfuscated == StylingState.Inherit &&
                                         Font == null && Insertion == null && ClickEvent == null &&
                                         HoverEvent == null && Children.Count == 0;
    
    public string ToJson()
    {
        if (IsSimpleTextComponent) return System.Web.HttpUtility.JavaScriptStringEncode(Text ?? "", true);
        var sb = new StringBuilder();
        sb.Append('{');
        switch (Type)
        {
            case ComponentType.Text:
                sb.Append($"\"text\":{System.Web.HttpUtility.JavaScriptStringEncode(Text ?? "", true)}");
                break;
            case ComponentType.Translatable:
                sb.Append($"\"translate\":{System.Web.HttpUtility.JavaScriptStringEncode(Text ?? "", true)}");
                if (With.Count > 0)
                {
                    sb.Append($",\"with\":[{string.Join(",", With.Select(x => x.ToJson()))}]");
                }
                break;
            case ComponentType.KeyBind:
                sb.Append($"\"keybind\":{System.Web.HttpUtility.JavaScriptStringEncode(Text ?? "", true)}");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (Color != null)
        {
            sb.Append($",\"color\":{System.Web.HttpUtility.JavaScriptStringEncode(Color, true)}");
        }

        if (Bold != StylingState.Inherit)
        {
            sb.Append($",\"bold\":{(Bold == StylingState.Set ? "true" : "false")}");
        }
        if (Italic != StylingState.Inherit)
        {
            sb.Append($",\"italic\":{(Italic == StylingState.Set ? "true" : "false")}");
        }
        if (Underlined != StylingState.Inherit)
        {
            sb.Append($",\"underlined\":{(Underlined == StylingState.Set ? "true" : "false")}");
        }
        if (Strikethrough != StylingState.Inherit)
        {
            sb.Append($",\"strikethrough\":{(Strikethrough == StylingState.Set ? "true" : "false")}");
        }
        if (Obfuscated != StylingState.Inherit)
        {
            sb.Append($",\"obfuscated\":{(Obfuscated == StylingState.Set ? "true" : "false")}");
        }

        if (Insertion != null)
        {
            sb.Append($",\"insertion\":{System.Web.HttpUtility.JavaScriptStringEncode(Insertion, true)}");
        }

        if (ClickEvent != null)
        {
            sb.Append($",\"clickEvent\":{ClickEvent.ToJson()}");
        }

        if (HoverEvent != null)
        {
            sb.Append($",\"hoverEvent\":{HoverEvent.ToJson()}");
        }

        if (Children.Count > 0)
        {
            sb.Append($",\"extra\":[{string.Join(",", Children.Select(x => x.ToJson()))}]");
        }
        sb.Append('}');
        return sb.ToString();
    }

    public NbtTag ToNbt(bool forceCompound=false)
    {
        if (IsSimpleTextComponent && !forceCompound) return Text ?? "";
        var compound = new CompoundTag();
        switch (Type)
        {
            case ComponentType.Text:
                compound["text"] = Text ?? "";
                break;
            case ComponentType.Translatable:
                compound["translate"] = Text ?? "";
                if (With.Count > 0)
                {
                    var force = With.Any(x => !x.IsSimpleTextComponent);
                    compound["with"] = new ListTag(null, NbtTagType.End, With.Select(x => x.ToNbt(force)));
                }
                break;
            case ComponentType.KeyBind:
                compound["keybind"] = Text ?? "";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (Color != null)
        {
            compound["color"] = Color;
        }

        if (Bold != StylingState.Inherit)
        {
            compound["bold"] = Bold == StylingState.Set;
        }
        
        if (Italic != StylingState.Inherit)
        {
            compound["italic"] = Italic == StylingState.Set;
        } 
        
        if (Underlined != StylingState.Inherit)
        {
            compound["underlined"] = Underlined == StylingState.Set;
        }

        if (Strikethrough != StylingState.Inherit)
        {
            compound["strikethrough"] = Strikethrough == StylingState.Set;
        }

        if (Obfuscated != StylingState.Inherit)
        {
            compound["obfuscated"] = Obfuscated == StylingState.Set;
        }

        if (Font != null)
        {
            compound["font"] = Font;
        }

        if (Insertion != null)
        {
            compound["insertion"] = Insertion;
        }

        if (ClickEvent != null)
        {
            compound["clickEvent"] = ClickEvent.ToNbtTag();
        }

        if (HoverEvent != null)
        {
            compound["hoverEvent"] = HoverEvent.ToNbt();
        }
        
        return compound;
    }
    #endregion


    #region Construction

    public static TextComponent CreateText(string text) =>
        new()
        {
            Type = ComponentType.Text,
            Text = text
        };

    public static TextComponent CreateTranslatable(string translate, params TextComponent[] with) =>
        new()
        {
            Type = ComponentType.Translatable,
            Text = translate,
            With = with.ToList()
        };

    public static TextComponent CreateKeybind(string keybind) =>
        new()
        {
            Type = ComponentType.KeyBind,
            Text = keybind
        };

    private void GuardType(ComponentType type)
    {
        if (Type != type)
            throw new Exception(
                $"Attempted to call a method that expects a component type of {type} with a component type of {Type}");
    }
    
    public TextComponent SetText(string text)
    {
        GuardType(ComponentType.Text);
        Text = text;
        return this;
    }

    public TextComponent SetTranslationKey(string translate)
    {
        GuardType(ComponentType.Translatable);
        Text = translate;
        return this;
    }

    public TextComponent TranslateWith(params TextComponent[] with)
    {
        GuardType(ComponentType.Translatable);
        With.AddRange(with);
        return this;
    }

    public TextComponent SetKeybind(string keybind)
    {
        GuardType(ComponentType.KeyBind);
        Text = keybind;
        return this;
    }

    public TextComponent SetColor(string? color=null)
    {
        Color = color;
        return this;
    }

    public TextComponent ClearColor() => SetColor();

    public TextComponent SetColor(byte r, byte g, byte b) => SetColor($"#{r:x2}{g:x2}{b:x2}");

    public TextComponent SetColor(Color color) => SetColor(color.R, color.G, color.B);

    public TextComponent SetColor(TextColor color) => SetColor(color switch
    {
        TextColor.Black => "black",
        TextColor.DarkBlue => "dark_blue",
        TextColor.DarkGreen => "dark_green",
        TextColor.DarkAqua => "dark_aqua",
        TextColor.DarkRed => "dark_red",
        TextColor.Purple => "purple",
        TextColor.Gold => "gold",
        TextColor.Gray => "gray",
        TextColor.DarkGray => "dark_gray",
        TextColor.Blue => "blue",
        TextColor.Green => "green",
        TextColor.Aqua => "aqua",
        TextColor.Red => "red",
        TextColor.LightPurple => "light_purple",
        TextColor.Yellow => "yellow",
        TextColor.White => "white",
        _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
    });

    public TextComponent SetBold(StylingState stylingState = StylingState.Set)
    {
        Bold = stylingState;
        return this;
    }

    public TextComponent ClearBold() => SetBold(StylingState.Clear);

    public TextComponent InheritBold() => SetBold(StylingState.Inherit);
    public TextComponent SetItalic(StylingState stylingState = StylingState.Set)
    {
        Italic = stylingState;
        return this;
    }

    public TextComponent ClearItalic() => SetItalic(StylingState.Clear);

    public TextComponent InheritItalic() => SetItalic(StylingState.Inherit);
    public TextComponent SetUnderlined(StylingState stylingState = StylingState.Set)
    {
        Underlined = stylingState;
        return this;
    }

    public TextComponent ClearUnderlined() => SetUnderlined(StylingState.Clear);

    public TextComponent InheritUnderlined() => SetUnderlined(StylingState.Inherit);
    public TextComponent SetStrikethrough(StylingState stylingState = StylingState.Set)
    {
        Strikethrough = stylingState;
        return this;
    }

    public TextComponent ClearStrikethrough() => SetStrikethrough(StylingState.Clear);

    public TextComponent InheritStrikethrough() => SetStrikethrough(StylingState.Inherit);
    public TextComponent SetObfuscated(StylingState stylingState = StylingState.Set)
    {
        Obfuscated = stylingState;
        return this;
    }

    public TextComponent ClearObfuscated() => SetObfuscated(StylingState.Clear);

    public TextComponent InheritObfuscated() => SetObfuscated(StylingState.Inherit);

    public TextComponent SetFont(string? font=null)
    {
        Font = font;
        return this;
    }

    public TextComponent ClearFont() => SetFont();

    public TextComponent SetInsertion(string? insertion = null)
    {
        Insertion = insertion;
        return this;
    }

    public TextComponent ClearInsertion() => SetInsertion();

    public TextComponent SetClickEvent(ClickEvent? clickEvent=null)
    {
        ClickEvent = clickEvent;
        return this;
    }

    public TextComponent ClearClickEvent() => SetClickEvent();

    public TextComponent SetHoverEvent(HoverEvent? hoverEvent = null)
    {
        HoverEvent = hoverEvent;
        return this;
    }

    public TextComponent ClearHoverEvent() => SetHoverEvent();

    public TextComponent AddChild(TextComponent child)
    {
        Children.Add(child);
        return this;
    }

    public TextComponent AddChildren(params TextComponent[] children)
    {
        Children.AddRange(children);
        return this;
    }

    public TextComponent AddChildren(IEnumerable<TextComponent> children)
    {
        Children.AddRange(children);
        return this;
    }

    #endregion
}