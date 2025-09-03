// See https://aka.ms/new-console-template for more information

using System.Net;
using Blocks.Net.TestClient;

var client = new Client(new IPAddress([127, 0, 0, 1]), 25565, "mutable_registries.json");
client.Run();