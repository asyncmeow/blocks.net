using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Blocks.Net.Packets;
using Blocks.Net.Packets.Configuration.ServerBound;
using Blocks.Net.Packets.Handshake;
using Blocks.Net.Packets.Login.ClientBound;
using Blocks.Net.Packets.Login.ServerBound;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.Status.ClientBound;
using Blocks.Net.Packets.Status.ServerBound;
using Blocks.Net.Text;
using Disconnect = Blocks.Net.Packets.Configuration.ClientBound.Disconnect;
using System.Text.Json.Nodes;

namespace Blocks.Net.TestServer;

public class Server(IPAddress address, TextComponent motd, TextComponent kickReason, ushort port=25565)
{

    public enum ConnectionState
    {
        Handshake,
        Status,
        Login,
        Configuration
    }
    // We want to make a very shitty server that can only handle one client at a time
    // public TcpListener Listener = new(address, port);
    public ConnectionState State = ConnectionState.Handshake;

    public DateTime ConfigStateEnteredAt;

    private static MemoryStream ReadMessage(Stream stream)
    {
        Console.WriteLine("Beginning packet read!");
        var length = VarInt.ReadFrom(stream);
        // Console.WriteLine($"Packet length is: {length.Value}");
        var result = new MemoryStream();
        length.WriteTo(result);
        int lengthInt = length;
        var buffer = new byte[lengthInt];
        var sum = 0;
        while (sum < lengthInt)
        {
            sum += stream.Read(buffer, sum, lengthInt - sum);
        }
        result.Write(buffer);
        result.Seek(0, SeekOrigin.Begin);
        var arr = result.ToArray();
        // Console.WriteLine("Ending packet read!");
        // Console.WriteLine("Packet dump:");
        // for (int i = 0; i < arr.Length; i++)
        // {
        //     Console.WriteLine($"\t{arr[i]:X2}");
        // }
        return result;
    }
    
    // In our final thing having an actual json library will help a lot
    public string Status {
        get
        {
            var jn = new JsonObject
            {
                ["version"] = new JsonObject
                {
                    ["name"] = "1.20.4",
                    ["protocol"] = 765,
                },
                ["players"] = new JsonObject
                {
                    ["max"] = 1,
                    ["online"] = 0,
                    ["sample"] = new JsonArray()
                },
                // ["description"] = JsonNode.Parse(motd.ToJson().ToJsonString())!
                ["enforcesSecureChat"] = false,
                ["previewsChat"] = false
            };
            var motdJson = motd.ToJson();
            jn["description"] = motdJson;
            return jn.ToJsonString();
        }
    }

    public Stream CurrentStream;

    public void WritePacket(IPacket packet)
    {
        Console.WriteLine($"Sending packet: {packet}");
        // using var memStream = new MemoryStream();
        // packet.WriteToStream(memStream);
        // var arr = memStream.ToArray();
        // for (var i = 0; i < arr.Length; i++)
        // {
        //     Console.WriteLine($"\t{arr[i]:x2}");
        // }
        //
        // memStream.Seek(0, SeekOrigin.Begin);
        // memStream.CopyTo(CurrentStream);
        packet.WriteToStream(CurrentStream);
    }
    
    public void Run()
    {
        // Listener.Start();
        using Socket listener = new(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        listener.Bind(new IPEndPoint(address, port));
        listener.Listen(port);
        Console.WriteLine("Waiting for initial client");
        while (true)
        {
            var handler = listener.Accept();
            // using var client = Listener.AcceptTcpClient();
            Console.WriteLine($"Accepted new client: {handler}");
            using var stream = new NetworkStream(handler);
            CurrentStream = stream;
            
            while (handler.Connected)
            {
                if (!stream.DataAvailable)
                {
                    if (State == ConnectionState.Configuration &&
                        DateTime.Now - ConfigStateEnteredAt > new TimeSpan(0, 0, 10))
                    {
                        Console.WriteLine("It has been greater than 10 seconds since server has entered configuration state, kicking player");
                        ((IPacket)new Disconnect
                        {
                            Reason = kickReason.ToNbt()
                        }).WriteToStream(stream);
                        break;
                    }
                    continue;
                }
                using var message = ReadMessage(stream);
                try
                {
                    switch (State)
                    {
                        case ConnectionState.Handshake:
                            HandleHandshakeMessage(message);
                            break;
                        case ConnectionState.Status:
                            if (HandleStatusMessage(message, stream))
                            {
                                stream.Close();
                                handler.Shutdown(SocketShutdown.Both);
                                handler.Close();
                            }
                            break;
                        case ConnectionState.Login:
                            HandleLoginMessage(message, stream);
                            break;
                        case ConnectionState.Configuration:
                            HandleConfigurationMessage(message, stream);
                            break;
                        default:
                            Console.WriteLine($"Disconnecting client due to being in invalid state: {State}");
                            stream.Close();
                            handler.Close();
                            goto reset;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Disconnecting client due to the following error:\n\t{e}");
                    stream.Close();
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                    break;
                }
            }
            reset:
            Console.WriteLine("Client disconnected, resetting to handshake state and waiting for a new client");
            State = ConnectionState.Handshake;
        }
    }

    public void HandleHandshakeMessage(MemoryStream message)
    {
        var packet = PacketParser.ParseHandshaking(message);
        if (packet is Handshake handshake)
        {
            Console.WriteLine($"Client is handshaking:\n\tProtocol: {handshake.ProtocolVersion.Value}\n\tAddress: {handshake.ServerAddress}\n\tPort: {handshake.ServerPort}");
            if (handshake.NextState == Handshake.NextStateEnum.Login)
            {
                Console.WriteLine("Client wants to login!");
                State = ConnectionState.Login;
            }
            else
            {
                Console.WriteLine("Client wants to request status!");
                State = ConnectionState.Status;
            }
        }
        else throw new Exception($"Unsupported handshaking packet type: {packet.GetType()}");
    }

    public bool HandleStatusMessage(MemoryStream message, NetworkStream ns)
    {
        var packet = PacketParser.ParseStatus(message);
        switch (packet)
        {
            case StatusRequest statusRequest:
                Console.WriteLine("Client sent status request");
                Console.WriteLine($"Responding with status {Status}");
                WritePacket(new StatusResponse
                {
                    JsonResponse = Status
                });
                return false;
            case PingRequest pingRequest:
                Console.WriteLine("PING!");
                WritePacket(new PingResponse
                {
                    Payload = pingRequest.Payload
                });
                Console.WriteLine("PONG!");
                return true;
            default:
                Console.WriteLine($"Client sent unsupported status packet: {packet.GetType()}");
                return false;
        }
    }

    public void HandleLoginMessage(MemoryStream message, NetworkStream ns)
    {
        var packet = PacketParser.ParseLogin(message);
        switch (packet)
        {
            case LoginStart loginStart:
                Console.WriteLine("Client wants to log in:");
                Console.WriteLine($"\tUsername: {loginStart.Username}");
                Console.WriteLine($"\tUUID: {loginStart.PlayerUuid.Value}");
                WritePacket(new LoginSuccess
                {
                    PlayerUuid = loginStart.PlayerUuid,
                    Username = loginStart.Username,
                    NumProperties = 0,
                    PlayerProperties = []
                });
                break;
            case LoginAcknowledged:
                Console.WriteLine("Client has acknowledged login");
                ConfigStateEnteredAt = DateTime.Now;
                State = ConnectionState.Configuration;
                break;
            default:
                Console.WriteLine($"Client sent unsupported login packet: {packet.GetType()}");
                break;
        }
    }

    public void HandleConfigurationMessage(MemoryStream message, NetworkStream ns)
    {
        var packet = PacketParser.ParseConfiguration(message);
        switch (packet)
        {
            case PluginMessage pluginMessage:
                // Now we must respond with our own
                {
                    Console.WriteLine($"Received plugin message on channel {pluginMessage.Channel}");
                    using var channelStream = new MemoryStream(pluginMessage.Data.Value);
                    switch (pluginMessage.Channel)
                    {
                        case "minecraft:brand":
                            Console.WriteLine($"Client Brand: {Packets.Primitives.String.ReadFrom(channelStream).Value}");
                            break;
                        default:
                            Console.WriteLine("Unsupported channel");
                            break;
                    }
                }
                break;
            case ClientInformation clientInformation:
                Console.WriteLine("Received client information:");
                Console.WriteLine($"\tLocale: {clientInformation.Locale}");
                Console.WriteLine($"\tView Distance: {clientInformation.ViewDistance}");
                Console.WriteLine($"\tChat Mode: {clientInformation.ChatMode}");
                Console.WriteLine($"\tChat Colors: {clientInformation.ChatColors}");
                Console.WriteLine($"\tDisplayed Skin Parts: {clientInformation.SkinParts:X}");
                Console.WriteLine($"\tEnable Text Filtering: {clientInformation.EnableTextFiltering}");
                Console.WriteLine($"\tAllow Server Listings: {clientInformation.AllowServerListings}");
                {
                    using var channelStream = new MemoryStream();
                    Packets.Primitives.String brand = "blocks.net";
                    brand.WriteTo(channelStream);
                    WritePacket(new Blocks.Net.Packets.Configuration.ClientBound.PluginMessage
                    {
                        Channel = "minecraft:brand",
                        Data = channelStream.ToArray()
                    });
                }
                break;
            default:
                Console.WriteLine($"Client sent unsupported configuration packet: {packet.GetType()}");
                break;
        }
    }
}
        