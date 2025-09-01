using Blocks.Net.LibSourceGeneration.Builders;
using Blocks.Net.LibSourceGeneration.Definitions;
using Blocks.Net.LibSourceGeneration.Expressions;
using Blocks.Net.PacketSourceGenerator.Schema;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;

namespace Blocks.Net.PacketSourceGenerator;

[Generator]
public class PacketIdsSourceGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
    }

    public void Execute(GeneratorExecutionContext context)
    {
        // Now lets build up the packet registry automatically, this should help when protocol versions change
        if (context.AdditionalFiles.FirstOrDefault(x => x.Path.EndsWith("packets.json")) is { } packetsJson)
        {
            GeneratePacketIdsType(context, packetsJson.GetText()!.ToString());
        }
    }
    

    private void GeneratePacketIdsType(GeneratorExecutionContext context, string packetsJson)
    {
        var states = JsonConvert.DeserializeObject<Dictionary<string, ProtocolSchema>>(packetsJson);
        var sfb = new SourceFileBuilder();
        sfb.WithFileScopedNamespace("Blocks.Net.Packets").AddClass("PacketIds", clazz =>
        {
            clazz.Static();
            clazz.Public();
            foreach (var state in states!)
            {
                var stateName = state.Key;
                var protocol = state.Value;
                clazz.AddClass(char.ToUpper(stateName[0]) + stateName.Substring(1), outer =>
                {
                    outer.Public().Static();
                    if (protocol?.ClientboundPacketIds != null)
                    {
                        outer.AddClass("Clientbound", clientbound =>
                        {
                            clientbound.Public().Static();
                            foreach (var message in protocol.ClientboundPacketIds)
                            {
                                var packetName = message.Key.Replace("minecraft:", "").ToUpperInvariant();
                                clientbound.AddField("int", packetName,
                                    field => field.Public().Const()
                                        .Default(new IntegerLiteral(message.Value.ProtocolId,
                                            @base: IntegerBase.Hexadecimal)));
                            }
                        });
                    }

                    if (protocol?.ServerboundPacketIds != null)
                    {
                        outer.AddClass("Serverbound", serverbound =>
                        {
                            serverbound.Public().Static();
                            foreach (var message in protocol.ServerboundPacketIds)
                            {
                                var packetName = message.Key.Replace("minecraft:", "").ToUpperInvariant();
                                serverbound.AddField("int", packetName,
                                    field => field.Public().Const()
                                        .Default(new IntegerLiteral(message.Value.ProtocolId,
                                            @base: IntegerBase.Hexadecimal)));
                            }
                        });
                    }
                });
            }
        });
        
        context.AddSource("PacketIds.g.cs", sfb.Build());
    }
    
}