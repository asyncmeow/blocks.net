// See https://aka.ms/new-console-template for more information

using System.Net;
using Blocks.Net.TestServer;
using Blocks.Net.Text;


var server = new Server(IPAddress.Any, TextComponent.CreateText("Hello, World!").SetBold().SetColor(TextColor.Green),
    TextComponent.CreateText("Timed out!").SetColor(TextColor.Red).SetUnderlined());

server.Run();