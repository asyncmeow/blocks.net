using System.Net;
using System.Net.Sockets;
using System.Text;
using Blocks.Net.Nbt;
using Blocks.Net.Packets;
using Blocks.Net.Packets.Configuration.ClientBound;
using Blocks.Net.Packets.Configuration.ServerBound;
using Blocks.Net.Packets.Handshake;
using Blocks.Net.Packets.Login.ClientBound;
using Blocks.Net.Packets.Login.ServerBound;
using Blocks.Net.Packets.Primitives;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.Packets.SubPackets.Configuration;
using Disconnect = Blocks.Net.Packets.Login.ClientBound.Disconnect;
using PluginMessage = Blocks.Net.Packets.Configuration.ServerBound.PluginMessage;

namespace Blocks.Net.TestClient;

public class Client(IPAddress address, ushort port = 25565, string registryDumpPath = "./registries_mutable.json", string tagDumpPath = "./tags.json")
{
    public PacketState CurrentState = new PacketState
    {
        DimensionSize = 24,
        CurrentDimensionHeightmapBitsPerEntry = 4,
        BiomeMinBitsPerEntry = 6
    };

    public enum ConnectionState
    {
        Login,
        Configuration
    }
    
    public ConnectionState State = ConnectionState.Login;
    
    private MemoryStream ReadMessage(Stream stream, PacketState state)
    {
        var length = VarInt.ReadFrom(stream, state);

        var result = new MemoryStream();
        length.WriteTo(result, state);
        int lengthInt = length;
        var buffer = new byte[lengthInt];
        var sum = 0;
        while (sum < lengthInt)
        {
            sum += stream.Read(buffer, sum, lengthInt - sum);
        }

        result.Write(buffer);
        result.Seek(0, SeekOrigin.Begin);
        // var arr = result.ToArray();
        // Console.WriteLine("Ending packet read!");
        // Console.WriteLine("Packet dump:");
        // for (int i = 0; i < arr.Length; i++)
        // {
        //     Console.WriteLine($"\t{arr[i]:X2}");
        // }
        return result;
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
        packet.WriteToStream(CurrentStream, CurrentState);
    }
    
    
    public DateTime LastPacketRecievedAt;
    
    public void Run()
    {
        using Socket listener = new(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        listener.Connect(address,port);
        using var stream = new NetworkStream(listener);
        CurrentStream = stream;
        LastPacketRecievedAt = DateTime.Now;
        if (!listener.Connected) return;
        WritePacket(new Handshake
        {
            NextState = Handshake.NextStateEnum.Login,
            ProtocolVersion = 772,
            ServerAddress = address.ToString(),
            ServerPort = port
        });
        WritePacket(new LoginStart
        {
            PlayerUuid = new Uuid(),
            Username = "blocks_net_test"
        });
        while (listener.Connected)
        {
            try
            {
                if (!stream.DataAvailable)
                {
                    if (DateTime.Now - LastPacketRecievedAt > new TimeSpan(0, 0, 10))
                    {
                        listener.Close();
                        break;
                    }
                }
                using var message = ReadMessage(stream, CurrentState);
                switch (State)
                {
                    case ConnectionState.Login:
                        HandleLoginMessage(message, stream);
                        break;
                    
                    case ConnectionState.Configuration:
                        HandleConfigurationMessage(message, stream);
                        break;
                    default:
                        Console.WriteLine($"Disconnecting server due to being in invalid state: {State}");
                        stream.Close();
                        throw new Exception("Invalid State!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Disconnecting due to {e}: {e.Message}");
                break;
            }
        }
    }
    
    private void HandleLoginMessage(MemoryStream message, NetworkStream stream)
    {
        var packet = ClientboundPacketParser.ParseLogin(message, CurrentState);
        switch (packet)
        {
            case LoginSuccess loginSuccess:
            {
                using var channelStream = new MemoryStream();
                Packets.Primitives.String brand = "blocks.net";
                brand.WriteTo(channelStream, CurrentState);
                WritePacket(new LoginAcknowledged());
                WritePacket(new PluginMessage
                {
                    Channel = "minecraft:brand",
                    Data = channelStream.ToArray()
                });
                State = ConnectionState.Configuration;
                break;
            }
            case Disconnect disconnect:
                throw new Exception($"{disconnect.Reason}");
            default:
                Console.WriteLine($"Server sent unsupported login packet: {packet.GetType()}");
                break;
        }
    }

    private struct RegistryEntry
    {
        public string Identifier;
        public NbtTag? Tag;
    }
    private Dictionary<string, List<RegistryEntry>> _registries = [];
    private Dictionary<string, Dictionary<string, List<int>>> _tags = [];

    private void HandleConfigurationMessage(MemoryStream message, NetworkStream stream)
    {
        var packet = ClientboundPacketParser.ParseConfiguration(message, CurrentState);
        switch (packet)
        {
            case Packets.Configuration.ClientBound.PluginMessage pluginMessage:
            {
                Console.WriteLine($"Received plugin message on channel {pluginMessage.Channel}");
                using var channelStream = new MemoryStream(pluginMessage.Data.Value);
                switch (pluginMessage.Channel)
                {
                    case "minecraft:brand":
                        Console.WriteLine(
                            $"Server Brand: {Packets.Primitives.String.ReadFrom(channelStream, CurrentState).Value}");
                        break;
                    default:
                        Console.WriteLine("Unsupported channel");
                        break;
                }
                break;
            }
            case FeatureFlags featureFlags:
                Console.WriteLine($"Server feature flags: {string.Join(", ", featureFlags.Flags)}");
                break;
            case KnownPacks knownPacks:
                foreach (var pack in knownPacks.Packs)
                {
                    Console.WriteLine("Server knows pack: " + pack.Namespace + ":" + pack.Id + $" version {pack.Version}");
                }
                WritePacket(new ServerBoundKnownPacks
                {
                    Packs = [new KnownPack
                    {
                        Namespace = "minecraft",
                        Id = "core",
                        Version = "1.21.8"
                    }]
                });
                break;
            case RegistryData registryData:
                Console.WriteLine($"Received registry: {registryData.RegistryId.Value}");
                if (!_registries.TryGetValue(registryData.RegistryId.Value, out var entries))
                {
                    entries = _registries[registryData.RegistryId.Value] = [];
                }
                foreach (var entry in registryData.Entries)
                {
                    entries.Add(new RegistryEntry
                    {
                        Identifier = entry.EntryId.Value,
                        Tag = entry.Data
                    });
                }
                break;
            case UpdateTags updateTags:
                foreach (var registry in updateTags.Registries)
                {
                    Console.WriteLine($"Updating registries ofr {registry.Registry.Value}");
                    _tags[registry.Registry.Value] = [];
                    foreach (var tag in registry.Tags)
                    {
                        Console.WriteLine($"    Tag: {tag.TagName.Value}");
                        _tags[registry.Registry.Value][tag.TagName.Value] = tag.Entries.Select(x => (int)x).ToList();
                    }
                }
                break;
            case FinishConfiguration finishConfiguration:
                WriteAllRegistries();
                WriteAllTags();
                throw new Exception("Configuration Finished");
            case Packets.Configuration.ClientBound.Disconnect disconnect:
                throw new Exception($"{disconnect.Reason}");
            default:
                Console.WriteLine($"Server sent unsupported configuration packet: {packet.GetType()}");
                break;
        }
    }

    private void WriteAllTags()
    {
        var sb = new StringBuilder();
        sb.Append('{');
        var kvps = _tags.ToArray();
        for (var i = 0; i < kvps.Length; i++)
        {
            var key = kvps[i].Key;
            var value = kvps[i].Value;
            sb.Append($"\"{key}\": {{");
            var kvps2 = value.ToArray();
            for (var j = 0; j < kvps2.Length; j++)
            {
                var key2 = kvps2[j].Key;
                var values = kvps2[j].Value;
                sb.Append($"\"{key2}\": [");
                for (var k = 0; k < values.Count; k++)
                {
                    sb.Append(values[k]);
                    if (k != values.Count - 1)
                    {
                        sb.Append(',');
                    }
                }
                sb.Append(']');
                if (j != kvps2.Length - 1)
                {
                    sb.Append(',');
                }
            }
            sb.Append('}');
            if (i != kvps.Length - 1)
            {
                sb.Append(',');
            }
        }

        sb.Append('}');
        File.WriteAllText(tagDumpPath, sb.ToString());
    }

    private void WriteAllRegistries()
    {
        var sb = new StringBuilder();
        sb.Append('{');
        var kvps = _registries.ToArray();
        for (var i = 0; i < _registries.Count; i++)
        {
            var key = kvps[i].Key;
            var value =  kvps[i].Value;
            sb.Append($"\"{key}\": [");
            for (var j = 0; j < value.Count; j++)
            {
                sb.Append($"\"{value[j].Identifier}\"");
                if (j < value.Count - 1)
                {
                    sb.Append(",");
                }
            }

            sb.Append(']');
            if (i != _registries.Count - 1)
            {
                sb.Append(',');
            }
        }
        sb.Append('}');
        File.WriteAllText(registryDumpPath, sb.ToString());
    }

}