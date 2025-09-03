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
using Blocks.Net.Data.Vanilla;
using Blocks.Net.DataTypes;
using Blocks.Net.Framework;
using Blocks.Net.Packets.Configuration.ClientBound;
using Blocks.Net.Packets.Enums;
using Blocks.Net.Packets.Play.ClientBound;
using Blocks.Net.Packets.Play.ServerBound;
using Blocks.Net.Packets.SubPackets;
using Blocks.Net.Packets.SubPackets.Chunks;
using Blocks.Net.Packets.SubPackets.Configuration;
using ClientInformation = Blocks.Net.Packets.Configuration.ServerBound.ClientInformation;
using KeepAlive = Blocks.Net.Packets.Configuration.ClientBound.KeepAlive;
using PingRequest = Blocks.Net.Packets.Status.ServerBound.PingRequest;
using PingResponse = Blocks.Net.Packets.Status.ClientBound.PingResponse;
using PluginMessage = Blocks.Net.Packets.Configuration.ServerBound.PluginMessage;

namespace Blocks.Net.TestServer;

public class Server(IPAddress address, TextComponent motd, TextComponent kickReason, ushort port = 25565)
{
    public PacketState CurrentState = new PacketState
    {
        DimensionSize = 24,
        CurrentDimensionHeightmapBitsPerEntry = 4,
        BiomeMinBitsPerEntry = 6
    };

    public enum ConnectionState
    {
        Handshake,
        Status,
        Login,
        Configuration,
        Play
    }

    // We want to make a very shitty server that can only handle one client at a time
    // public TcpListener Listener = new(address, port);
    public ConnectionState State = ConnectionState.Handshake;

    public DateTime ConfigStateEnteredAt;

    public RegistrySet Registries = new();
    public TagRegistrySet TagRegistries = new();

    private MemoryStream ReadMessage(Stream stream, PacketState state)
    {
        var length = VarInt.ReadFrom(stream, state);

        // Handle legacy server list ping!
        if (State == ConnectionState.Handshake && length == 254)
        {
            var kickString = "§1\0127\01.21.8\0Blocks.Net Test Server\00\01";
            var bytes = Encoding.BigEndianUnicode.GetBytes(kickString);
            stream.WriteByte(0xFF);
            stream.WriteByte((byte)(bytes.Length >> 8));
            stream.WriteByte((byte)(bytes.Length & 0xff));
            stream.Write(bytes);
            throw new Exception("Kicking player due to legacy ping!");
        }

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
    public string Status
    {
        get
        {
            var jn = new JsonObject
            {
                ["version"] = new JsonObject
                {
                    ["name"] = "1.21.8",
                    ["protocol"] = 772,
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
        packet.WriteToStream(CurrentStream, CurrentState);
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
                try
                {
                    if (!stream.DataAvailable)
                    {
                        if (State == ConnectionState.Configuration &&
                            DateTime.Now - ConfigStateEnteredAt > new TimeSpan(0, 0, 10))
                        {
                            Console.WriteLine(
                                "It has been greater than 10 seconds since server has entered configuration state, kicking player");
                            ((IPacket)new Disconnect
                            {
                                Reason = kickReason
                            }).WriteToStream(stream, CurrentState);
                            break;
                        }

                        continue;
                    }

                    using var message = ReadMessage(stream, CurrentState);
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
                        case ConnectionState.Play:
                            HandlePlayMessage(message, stream);
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
        var packet = ServerboundPacketParser.ParseHandshaking(message, CurrentState);
        if (packet is Handshake handshake)
        {
            Console.WriteLine(
                $"Client is handshaking:\n\tProtocol: {handshake.ProtocolVersion.Value}\n\tAddress: {handshake.ServerAddress}\n\tPort: {handshake.ServerPort}");
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
        var packet = ServerboundPacketParser.ParseStatus(message, CurrentState);
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
        var packet = ServerboundPacketParser.ParseLogin(message, CurrentState);
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
        var packet = ServerboundPacketParser.ParseConfiguration(message, CurrentState);
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
                        Console.WriteLine(
                            $"Client Brand: {Packets.Primitives.String.ReadFrom(channelStream, CurrentState).Value}");
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
                brand.WriteTo(channelStream, CurrentState);
                WritePacket(new Blocks.Net.Packets.Configuration.ClientBound.PluginMessage
                {
                    Channel = "minecraft:brand",
                    Data = channelStream.ToArray()
                });
                WritePacket(new FeatureFlags
                {
                    Flags = []
                });
                WritePacket(new KnownPacks
                {
                    Packs =
                    [
                        new KnownPack
                        {
                            Namespace = "minecraft",
                            Id = "core",
                            Version = "1.21.8"
                        }
                    ]
                });
            }
                break;
            case ServerBoundKnownPacks serverBoundKnownPacks:
                foreach (var pack in serverBoundKnownPacks.Packs)
                {
                    Console.WriteLine($"Received known pack {pack.Namespace}:{pack.Id} version {pack.Version}");
                }

                // Send all the registries
                foreach (var registry in Registries.Registries)
                {
                    WritePacket(registry.Value.GenerateRegistryDataPacket("minecraft:core"));
                }

                WritePacket(TagRegistries.GenerateTagUpdatePacket());
                // Then finish the configuration
                WritePacket(new FinishConfiguration());
                break;
            case AcknowledgeFinishConfiguration acknowledgeFinishConfiguration:
                BeginPlayMode();
                break;
            default:
                Console.WriteLine($"Client sent unsupported configuration packet: {packet.GetType()}");
                break;
        }
    }

    public void BeginPlayMode()
    {
        WritePacket(new Login
        {
            EntityId = 1,
            IsHardcore = false,
            DimensionNames = ["overworld"],
            MaxPlayers = 1,
            ViewDistance = 12,
            SimulationDistance = 16,
            ReducedDebugInfo = false,
            EnableRespawnScreen = false,
            DoLimitedCrafting = false,
            DimensionType = Registries.DimensionType["overworld"],
            DimensionName = "overworld",
            HashedSeed = 0,
            GameMode = 3,
            PreviousGameMode = -1,
            IsDebug = false,
            IsFlat = true,
            DeathLocation = null,
            PortalCooldown = 0,
            SeaLevel = 0,
            EnforcesSecureChat = false
        });
        WritePacket(new SynchronizePlayerPosition
        {
            TeleportId = 0,
            Position = new Double3
            {
                X = 0,
                Y = 1,
                Z = 0
            },
            Velocity = new Double3
            {
                X = 0,
                Y = 0,
                Z = 0
            },
            Yaw = 0,
            Pitch = 0,
            TeleportFlags = 0
        });
        State = ConnectionState.Play;
    }


    private void HandlePlayMessage(MemoryStream message, NetworkStream stream)
    {
        var packet = ServerboundPacketParser.ParsePlay(message, CurrentState);
        switch (packet)
        {
            case ConfirmTeleportation confirmTeleportation:
                LoadAllChunks();
                WritePacket(new ClientboundKeepAlive
                {
                    KeepAliveId = _keepAliveId++
                });
                break;
            case Blocks.Net.Packets.Play.ServerBound.KeepAlive keepAlive:
                WritePacket(new ClientboundKeepAlive
                {
                    KeepAliveId = _keepAliveId++
                });
                break;
        }
    }

    private void LoadAllChunks()
    {
        WritePacket(new GameEvent
        {
            Event = GameEvent.Events.StartWaitingForLevelChunks,
            Value = 0
        });
        WritePacket(new SetCenterChunk
        {
            ChunkX = 0,
            ChunkZ = 0
        });
        for (var x = -4; x < 4; x++)
        {
            for (var z = -4; z < 4; z++)
            {
                SendChunk(x, z);
            }
        }
    }

    private int _keepAliveId = 0;

    private void SendChunk(int x, int z)
    {
        var biomeArray = new int[64];
        Array.Fill(biomeArray, Registries.Biome["badlands"].RegistryId);
        var blockArray = new int[4096];
        Array.Fill(blockArray, Data.Vanilla.Blocks.WHITE_CONCRETE.StateId);
        var blockArray2 = new int[4096];
        Array.Fill(blockArray2, Data.Vanilla.Blocks.AIR.StateId);
        var airChunk = new ChunkSection
        {
            BlockCount = 0,
            Biomes = new PalettedContainer(biomeArray),
            BlockStates = new PalettedContainer(blockArray2)
        };
        var concreteChunk = new ChunkSection
        {
            BlockCount = 4096,
            Biomes = new PalettedContainer(biomeArray),
            BlockStates = new PalettedContainer(blockArray)
        };
        var allTrueBitset = new BitSet(26);
        for (var i = 0; i < 26; i++)
        {
            allTrueBitset[i] = true;
        }
        var allFalseBitset = new BitSet(26);
        var filledSkylightArray = new LightArray
        {
            Array = new byte[2048]
        };
        Array.Fill(filledSkylightArray.Array, (byte)255);
        var lightData = new LightData
        {
            SkyLightMask = allTrueBitset,
            BlockLightMask = allFalseBitset,
            EmptyBlockLightMask = allFalseBitset,
            EmptySkyLightMask = allFalseBitset,
            SkyLightArrays = [
                filledSkylightArray,
                filledSkylightArray,
                filledSkylightArray,
                filledSkylightArray,
                filledSkylightArray,
                filledSkylightArray,
                filledSkylightArray,
                filledSkylightArray,
                filledSkylightArray,
                filledSkylightArray,
                filledSkylightArray,
                filledSkylightArray,
                filledSkylightArray,
                filledSkylightArray,
                filledSkylightArray,
                filledSkylightArray,
                filledSkylightArray,
                filledSkylightArray,
                filledSkylightArray,
                filledSkylightArray,
                filledSkylightArray,
                filledSkylightArray,
                filledSkylightArray,
                filledSkylightArray,
                filledSkylightArray,
                filledSkylightArray,
            ],
            BlockLightArrays = []
        };
        
        var packet = new ChunkDataAndUpdateLight
        {
            ChunkX = x,
            ChunkZ = z,
            Data = new ChunkData
            {
                Heightmaps = [],
                Data = new ChunkDataArray([
                    concreteChunk,
                    concreteChunk,
                    concreteChunk,
                    concreteChunk,
                    airChunk,
                    airChunk,
                    airChunk,
                    airChunk,
                    airChunk,
                    airChunk,
                    airChunk,
                    airChunk,
                    airChunk,
                    airChunk,
                    airChunk,
                    airChunk,
                    airChunk,
                    airChunk,
                    airChunk,
                    airChunk,
                    airChunk,
                    airChunk,
                    airChunk,
                    airChunk,
                ]),
                BlockEntities = []
            },
            Light = lightData
        };
        WritePacket(packet);
    }


    // Eventually we want a client in a white concrete void
}