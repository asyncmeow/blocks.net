// See https://aka.ms/new-console-template for more information

using System.Net;
using Blocks.Net.TestServer;
using Blocks.Net.Text;


var server = new Server(IPAddress.Any, TextComponent.CreateText("Hello, World!").SetBold().SetColor(TextColor.Green),
    TextComponent.CreateText("You are ").SetColor(TextColor.Green).AddChild(((TextComponent)"TOO GAY").SetColor(TextColor.Blue).SetBold()).AddChild("for this server").SetUnderlined());

server.Run();