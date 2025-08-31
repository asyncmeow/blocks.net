using System.Diagnostics;
using Blocks.Net.LibSourceGeneration.Builders;
using Blocks.Net.LibSourceGeneration.Expressions;
using Blocks.Net.LibSourceGeneration.Interfaces;
using Blocks.Net.LibSourceGeneration.Query;
using Blocks.Net.LibSourceGeneration.References;
using Blocks.Net.LibSourceGeneration.Statements;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;


namespace Blocks.Net.VanillaSourceGenerator;

[Generator]
public class DataSourceGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
    }

    public void Execute(GeneratorExecutionContext context)
    {
        SyntaxAssembly assembly = new(context);

        var blocksType = assembly.Types.FirstOrDefault(x => x.FullName == "Blocks.Net.Data.Vanilla.Blocks")!;
        GenerateBlocks(context, blocksType);
        
        var coreRegistryDataType = assembly.Types.FirstOrDefault(x => x.FullName == "Blocks.Net.Data.Vanilla.CoreRegistryData")!;
        GenerateRegistries(context, coreRegistryDataType);
    }


    #region Blocks

    private const string BlockCount = "BLOCK_COUNT";
    private const string StateCount = "STATE_COUNT";
    private const string BlockState = "BlockState";

    public void GenerateBlocks(GeneratorExecutionContext context, SyntaxType blocksType)
    {
        var blocksJson = context.AdditionalFiles.First(x => x.Path.EndsWith("blocks.json"));
        var blocksJsonText = blocksJson.GetText()!.ToString();
        var blocksJsonDecoded = JsonConvert.DeserializeObject<Dictionary<string, BlockSchema>>(blocksJsonText);


        var blocksSourceFileBuilder = new SourceFileBuilder();

        var blocksImplementation = blocksType.GenerateImplementation(blocksSourceFileBuilder);
        blocksImplementation.AddField(typeof(int), BlockCount,
            field => field.Const().Default(new IntegerLiteral(blocksJsonDecoded!.Count)));

        blocksImplementation.AddMethod("void", "InitializeLookup", out var initializeLookupMethod);
        initializeLookupMethod.Static().Partial();
        initializeLookupMethod.Add(new Assignment("_allBlocks", new NewArray("Block", new Variable(BlockCount))));
        initializeLookupMethod.Add(new Assignment("_stateToBlockIndexLookup",
            new NewArray("int", new Variable(StateCount))));
        var maxState = 0;
        var nextBlock = 0;
        foreach (var kvp in blocksJsonDecoded!)
        {
            var name = kvp.Key;
            var definition = kvp.Value!;
            var nameWithoutMinecraft = name.Replace("minecraft:", "").Replace("/", "__");
            var constantCase = nameWithoutMinecraft.ToUpperInvariant();
            GenerateBlockImplementation(context, nameWithoutMinecraft, definition, out var initExpr, out var allStates,
                out var defaultState);
            initializeLookupMethod.Add(
                new Assignment(new Subscript(new Variable("_allBlocks"), new IntegerLiteral(nextBlock)), initExpr));
            foreach (var state in allStates)
            {
                initializeLookupMethod.Add(new Assignment(
                    new Subscript(new Variable("_stateToBlockIndexLookup"), new IntegerLiteral(state)),
                    new IntegerLiteral(nextBlock)));
            }

            blocksImplementation.AddField(BlockState, constantCase, field =>
            {
                field.Static();
                field.Public();
                field.Readonly();
                field.Default(new NewObject(BlockState, new IntegerLiteral(defaultState)));
            });
            nextBlock++;
            maxState = Math.Max(maxState, allStates.Max());
        }

        blocksImplementation.AddField(typeof(int), StateCount,
            field => field.Const().Default(new IntegerLiteral(maxState + 1)));


        context.AddSource("Blocks.g.cs", blocksSourceFileBuilder.Build());
    }


    // This returns a list of all the state IDs given
    public void GenerateBlockImplementation(GeneratorExecutionContext context, string nameWithoutMinecraft,
        BlockSchema blockSchema, out IExpression initializationExpression, out int[] allStates, out int defaultState)
    {
        var pascalCase = ToPascalCase(nameWithoutMinecraft);
        var allIds = new List<int>();

        if (blockSchema.Properties.Properties.Count == 0)
        {
            var state = blockSchema.States[0];
            allStates = [state.Id];
            defaultState = state.Id;
            initializationExpression = new NewObject("Block", new StringLiteral($"minecraft:{nameWithoutMinecraft}"));
        }
        else
        {
            var blockImplementationSourceBuilder = new SourceFileBuilder();
            allStates = blockSchema.States.Select(x => x.Id).ToArray();
            defaultState = blockSchema.States.First(x => x.Default).Id;
            // TODO: Eventually this will be the pascal case version
            blockImplementationSourceBuilder.Using("Blocks.Net.DataTypes");
            blockImplementationSourceBuilder.Using("Blocks.Net.Data.Vanilla");
            blockImplementationSourceBuilder.WithFileScopedNamespace("Blocks.Net.Data.Vanilla.BlockImpl");


            blockImplementationSourceBuilder.AddClass(pascalCase, ConstructClass);

            context.AddSource($"BlockImpl.{pascalCase}.g.cs", blockImplementationSourceBuilder.Build());

            initializationExpression = new NewObject($"BlockImpl.{pascalCase}",
                new StringLiteral($"minecraft:{nameWithoutMinecraft}"));
        }

        return;

        void ConstructClass(StructuredTypeReference clazz)
        {
            clazz.Public().Inherit("Block");
            clazz.AddConstructor(ctor => ctor.Public()
                .WithParameters(new ParameterReference("NamespacedIdentifier", "id"))
                .WithBaseCall("base", new Variable("id")));

            MethodReference? setBool = null;
            MethodReference? getBool = null;
            MethodReference? getInt = null;
            MethodReference? setInt = null;
            MethodReference? getString = null;
            MethodReference? setString = null;
            var priorVariantCount = 1;
            var baseState = blockSchema.States.Min(x => x.Id);

            clazz.AddMethod("BlockState", "Create", out var create);
            create.Static();
            create.Public();
            create.Add(new VariableDeclarationStatement("int", "actualId", new IntegerLiteral(baseState)));

            foreach (var property in blockSchema.Properties.Properties.AsEnumerable().Reverse())
            {
                var isBool = property.values.Length == 2 && property.values[0] == "true" &&
                             property.values[1] == "false";
                var isInt = int.TryParse(property.values[0], out var startInt);

                if (isBool)
                {
                    if (setBool == null)
                    {
                        clazz.AddMethod("BlockState", "SetVariantBool", out setBool);
                        setBool.Public().Override().WithParameters(new ParameterReference("BlockState", "state"),
                            new ParameterReference("string", "variantName"), new ParameterReference("bool", "value"));
                        setBool.Add(new VariableDeclarationStatement("actualId",
                            new GetField(new Variable("state"), "StateId")));

                        clazz.AddMethod("bool", "GetVariantBool", out getBool);
                        getBool.Public().Override().WithParameters(new ParameterReference("BlockState", "state"),
                            new ParameterReference("string", "variantName"));
                        getBool.Add(new VariableDeclarationStatement("actualId",
                            new GetField(new Variable("state"), "StateId")));
                    }

                    getBool!.Add(
                        new IfStatement(new Equals(new Variable("variantName"), new StringLiteral(property.key))).Add(
                            new ReturnStatement(new Equals(GetStateValue(priorVariantCount, 2),
                                new IntegerLiteral(0)))));

                    setBool.Add(
                        new IfStatement(new Equals(new Variable("variantName"), new StringLiteral(property.key))).Add(
                            new IfStatement(new Variable("value"))
                                .Add(new ReturnStatement(new NewObject("BlockState",
                                    SetStateValueInt(priorVariantCount, 2, 0)))).Else(x =>
                                    x.Add(new ReturnStatement(new NewObject("BlockState",
                                        SetStateValueInt(priorVariantCount, 2, 1)))))));


                    create.WithParameters(new ParameterReference("bool", '@' + property.key));
                    create.Add(new IfStatement(new Variable('@' + property.key))
                        .Add(new Assignment(new Variable("actualId"), SetStateValueInt(priorVariantCount, 2, 0)))
                        .Else(x => x.Add(new Assignment(new Variable("actualId"),
                            SetStateValueInt(priorVariantCount, 2, 1)))));
                }
                else if (isInt)
                {
                    if (setInt == null)
                    {
                        clazz.AddMethod("BlockState", "SetVariantInt", out setInt);
                        setInt.Public().Override().WithParameters(new ParameterReference("BlockState", "state"),
                            new ParameterReference("string", "variantName"), new ParameterReference("int", "value"));
                        setInt.Add(new VariableDeclarationStatement("actualId",
                            new GetField(new Variable("state"), "StateId")));

                        clazz.AddMethod("int", "GetVariantInt", out getInt);
                        getInt.Public().Override().WithParameters(new ParameterReference("BlockState", "state"),
                            new ParameterReference("string", "variantName"));
                        getInt.Add(new VariableDeclarationStatement("actualId",
                            new GetField(new Variable("state"), "StateId")));
                    }

                    getInt!.Add(
                        new IfStatement(new Equals(new Variable("variantName"), new StringLiteral(property.key))).Add(
                            new ReturnStatement(new Subtraction(
                                GetStateValue(priorVariantCount, property.values.Length),
                                new IntegerLiteral(startInt)))));

                    setInt.Add(
                        new IfStatement(new Equals(new Variable("variantName"), new StringLiteral(property.key))).Add(
                            new ReturnStatement(new NewObject("BlockState",
                                SetStateValue(priorVariantCount, property.values.Length,
                                    new Addition(new Variable("value"), new IntegerLiteral(startInt)))))));

                    create.WithParameters(new ParameterReference("int", '@' + property.key));
                    create.Add(new Assignment(new Variable("actualId"),
                        new Addition(new Variable('@' + property.key), new IntegerLiteral(startInt))));
                }
                else
                {
                    if (setString == null)
                    {
                        clazz.AddMethod("BlockState", "SetVariantString", out setString);
                        setString.Public().Override().WithParameters(new ParameterReference("BlockState", "state"),
                            new ParameterReference("string", "variantName"), new ParameterReference("string", "value"));
                        setString.Add(new VariableDeclarationStatement("actualId",
                            new GetField(new Variable("state"), "StateId")));

                        clazz.AddMethod("string", "GetVariantString", out getString);
                        getString.Public().Override().WithParameters(new ParameterReference("BlockState", "state"),
                            new ParameterReference("string", "variantName"));
                        getString.Add(new VariableDeclarationStatement("actualId",
                            new GetField(new Variable("state"), "StateId")));
                    }

                    var getIfStatement =
                        new IfStatement(new Equals(new Variable("variantName"), new StringLiteral(property.key)));
                    getIfStatement.Add(new VariableDeclarationStatement("int", "stateValue",
                        GetStateValue(priorVariantCount, property.values.Length)));
                    clazz.AddField("List<string>", $"_{property.key}_names", field =>
                    {
                        field.Static();
                        field.Default(new NewObject().InitializeWith(initializer =>
                        {
                            foreach (var value in property.values)
                            {
                                initializer.Add(new StringLiteral(value));
                            }
                        }));
                    });
                    getIfStatement.Add(new ReturnStatement(new Subscript(new Variable($"_{property.key}_names"),
                        GetStateValue(priorVariantCount, property.values.Length))));

                    var setIfStatement =
                        new IfStatement(new Equals(new Variable("variantName"), new StringLiteral(property.key)));

                    create.WithParameters(new ParameterReference("string", '@' + property.key));
                    var idx = 0;
                    foreach (var value in property.values)
                    {
                        var idx0 = idx++;
                        setIfStatement.Add(
                            new IfStatement(new Equals(new Variable("value"), new StringLiteral(value))).Add(
                                new ReturnStatement(new NewObject("BlockState",
                                    SetStateValueInt(priorVariantCount, property.values.Length, idx0)))));
                        create.Add(
                            new IfStatement(new Equals(new Variable('@' + property.key), new StringLiteral(value))).Add(
                                new Assignment(new Variable("actualId"),
                                    SetStateValueInt(priorVariantCount, property.values.Length, idx0))));
                    }

                    getString!.Add(getIfStatement);
                    setString.Add(setIfStatement);
                }

                priorVariantCount *= property.values.Length;
            }

            if (getBool != null)
            {
                getBool.Add(new ReturnStatement(new BoundCall(new GetField(new Variable("base"), "GetVariantBool"),
                    new Variable("state"), new Variable("variantName"))));
                setBool!.Add(new ReturnStatement(new BoundCall(new GetField(new Variable("base"), "SetVariantBool"),
                    new Variable("state"), new Variable("variantName"), new Variable("value"))));
            }

            if (getInt != null)
            {
                getInt.Add(new ReturnStatement(new BoundCall(new GetField(new Variable("base"), "GetVariantInt"),
                    new Variable("state"), new Variable("variantName"))));
                setInt!.Add(new ReturnStatement(new BoundCall(new GetField(new Variable("base"), "SetVariantInt"),
                    new Variable("state"), new Variable("variantName"), new Variable("value"))));
            }


            if (getString != null)
            {
                getString.Add(new ReturnStatement(new BoundCall(new GetField(new Variable("base"), "GetVariantString"),
                    new Variable("state"), new Variable("variantName"))));
                setString!.Add(new ReturnStatement(new BoundCall(new GetField(new Variable("base"), "SetVariantString"),
                    new Variable("state"), new Variable("variantName"), new Variable("value"))));
            }

            create.Add(new ReturnStatement(new NewObject("BlockState", new Variable("actualId"))));

            return;

            TypeCall GetStateValue(int priorVariantCount, int currentVariantCount)
            {
                return new TypeCall("StateUtilities", "GetStateValue", new Variable("actualId"),
                    new IntegerLiteral(baseState), new IntegerLiteral(priorVariantCount),
                    new IntegerLiteral(currentVariantCount));
            }

            TypeCall SetStateValueInt(int priorVariantCount, int currentVariantCount, int value)
            {
                return new TypeCall("StateUtilities", "SetStateValue", new Variable("actualId"),
                    new IntegerLiteral(baseState), new IntegerLiteral(priorVariantCount),
                    new IntegerLiteral(currentVariantCount), new IntegerLiteral(value));
            }

            TypeCall SetStateValue(int priorVariantCount, int currentVariantCount, IExpression value)
            {
                return new TypeCall("StateUtilities", "SetStateValue", new Variable("actualId"),
                    new IntegerLiteral(baseState), new IntegerLiteral(priorVariantCount),
                    new IntegerLiteral(currentVariantCount), value);
            }
        }
    }

    #endregion

    #region Registries

    public void GenerateRegistries(GeneratorExecutionContext context, SyntaxType coreRegistryDataType)
    {
        
        var registriesJson = context.AdditionalFiles.First(x => x.Path.EndsWith("registries.json"));
        var registriesText = registriesJson.GetText()!.ToString();
        var registries = JsonConvert.DeserializeObject<Dictionary<string, RegistrySchema>>(registriesText);

        var registryBuilder = new SourceFileBuilder();
        var impl = coreRegistryDataType.GenerateImplementation(registryBuilder);
        impl.AddMethod("void", "AddAllRegistries", out var addAll);
        addAll.Static().Partial();
        
        
        foreach (var registry in registries!)
        {
            var key = registry.Key;
            var value = registry.Value!;
            var fieldName = key.Replace("minecraft:", "").Replace("/", "_").ToUpperInvariant();
            impl.AddField("CoreRegistry",fieldName, out var field);
            GenerateRegistry(key, value, field);
            addAll.Add(new BoundCall(new GetField(new Variable("Registries"), "Add"),new Variable(fieldName)));
        }
        
        context.AddSource("CoreRegistryData.g.cs", registryBuilder.Build());
    }

    public void GenerateRegistry(string registryKey, RegistrySchema entries, FieldReference field)
    {
        field.Static().Public().Readonly();
        var parameters = new StringLiteral[entries.Entries.Count + 1];
        parameters[0] = new StringLiteral(registryKey);
        foreach (var entry in entries.Entries)
        {
            parameters[entry.Value.ProtocolId + 1] = new StringLiteral(entry.Key);
        }
        field.Default(new NewObject("CoreRegistry", parameters));
    }
    
    
    
    #endregion
    #region utilities

    public static string ToPascalCase(string snakeCaseString)
    {
        if (string.IsNullOrWhiteSpace(snakeCaseString))
        {
            return string.Empty;
        }

        // Split the string by underscore
        string[] words = snakeCaseString.Split(['_'], StringSplitOptions.RemoveEmptyEntries);

        // Capitalize the first letter of each word and convert the rest to lowercase
        var pascalCaseWords = words.Select(word =>
        {
            if (string.IsNullOrEmpty(word))
            {
                return string.Empty;
            }

            return char.ToUpper(word[0]) + word.Substring(1).ToLower();
        });

        // Join the words to form the PascalCase string
        return string.Join("", pascalCaseWords);
    }

    #endregion
}