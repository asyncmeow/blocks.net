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
    public TcpListener Listener = new(address, port);
    public ConnectionState State = ConnectionState.Handshake;

    public DateTime ConfigStateEnteredAt;

    private static MemoryStream ReadMessage(Stream stream)
    {
        Console.WriteLine("Beginning packet read!");
        var length = VarInt.ReadFrom(stream);
        Console.WriteLine($"Packet length is: {length.Value}");
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
        Console.WriteLine("Ending packet read!");
        Console.WriteLine("Packet dump:");
        for (int i = 0; i < lengthInt; i++)
        {
            Console.WriteLine($"\t{buffer[i]:X2}");
        }
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
                ["description"] = motd.ToJson()
                ["enforcesSecureChat"] = false,
                ["previewsChat"] = false
            };
            return jn.ToJsonString();
        }
    }
    
    public void Run()
    {
        Listener.Start();
        Console.WriteLine("Waiting for initial client");
        while (true)
        {
            using var client = Listener.AcceptTcpClient();
            Console.WriteLine($"Accepted new client: {client}");
            using var stream = client.GetStream();
            
            while (client.Connected)
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
                            HandleStatusMessage(message, stream);
                            break;
                        case ConnectionState.Login:
                            HandleLoginMessage(message, stream);
                            break;
                        case ConnectionState.Configuration:
                            
                            break;
                        default:
                            Console.WriteLine($"Disconnecting client due to being in invalid state: {State}");
                            stream.Close();
                            client.Close();
                            goto reset;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Disconnecting client due to the following error:\n\t{e}");
                    stream.Close();
                    client.Close();
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

    public void HandleStatusMessage(MemoryStream message, NetworkStream ns)
    {
        var packet = PacketParser.ParseStatus(message);
        switch (packet)
        {
            case StatusRequest statusRequest:
                Console.WriteLine("Client sent status request");
                Console.WriteLine($"Responding with status {Status}");
                ((IPacket)new StatusResponse
                {
                    JsonResponse = Status
                }).WriteToStream(ns);
                break;
            case PingRequest pingRequest:
                ((IPacket)new PingResponse
                {
                    Payload = pingRequest.Payload
                }).WriteToStream(ns);
                break;
            default:
                Console.WriteLine($"Client sent unsupported status packet: {packet.GetType()}");
                break;
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
                Console.WriteLine($"\tUUID: {loginStart.PlayerUuid}");
                ((IPacket)new LoginSuccess
                {
                    PlayerUuid = loginStart.PlayerUuid,
                    Username = loginStart.Username,
                    NumProperties = 0,
                    PlayerProperties = []
                }).WriteToStream(ns);
                break;
            case LoginAcknowledged:
                Console.WriteLine("Client has acknowledged login");
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
                Console.WriteLine($"Received plugin message on channel {pluginMessage.Channel}");
                switch (pluginMessage.Channel)
                {
                    case "minecraft:brand":
                        Console.WriteLine($"Client Brand: {Encoding.UTF8.GetString(pluginMessage.Data)}");
                        break;
                    default:
                        Console.WriteLine("Unsupported channel");
                        break;
                }
                break;
            case ClientInformation clientInformation:
                Console.WriteLine("Received client information:");
                Console.WriteLine($"\tLocale: {clientInformation.Locale}");
                Console.WriteLine($"\tView Distance: {clientInformation.ViewDistance}");
                Console.WriteLine($"\tChat Mode: {clientInformation.ChatMode}");
                Console.WriteLine($"\tChat Colors: {clientInformation.ChatColors}");
                Console.WriteLine($"\tDisplayed Skin Parts: {clientInformation.SkinParts:X2}");
                Console.WriteLine($"\tEnable Text Filtering: {clientInformation.EnableTextFiltering}");
                Console.WriteLine($"\tAllow Server Listings: {clientInformation.AllowServerListings}");
                ((IPacket)new Blocks.Net.Packets.Configuration.ClientBound.PluginMessage
                {
                    Channel = "minecraft:brand",
                    Data = "blocks.net"u8.ToArray()
                }).WriteToStream(ns);
                break;
            default:
                Console.WriteLine($"Client sent unsupported configuration packet: {packet.GetType()}");
                break;
        }
    }
}
        