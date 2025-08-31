using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Blocks.Net.VanillaSourceGenerator;

[JsonConverter(typeof(PropertySetConverter))]
public class PropertySet
{
    public List<(string key, string[] values)> Properties = [];
}

public class PropertySetConverter : JsonConverter<PropertySet>
{
    public override void WriteJson(JsonWriter writer, PropertySet? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override PropertySet? ReadJson(JsonReader reader, Type objectType, PropertySet? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        List<(string key, string[] values)> properties = [];
        JObject objectData = JObject.Load(reader);
        foreach (var obj in objectData)
        {
            var arr = obj.Value as JArray;
            properties.Add((obj.Key, arr.Select(x => x.Value<string>()).ToArray()));
        }

        return new PropertySet
        {
            Properties = properties
        };
    }
}