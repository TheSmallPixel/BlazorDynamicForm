using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Dynamic;

namespace TypeAnnotationParser;

public class TypeAnnotationModel(string EntryType)
{
    public Dictionary<string, TypeAnnotationProperty> Properties { get; set; } = new();
    public string EntryType { get; set; } = EntryType;

    public string SerializeObject(ExpandoObject data)
    {
        var internalDic = data as IDictionary<string, object>;
       
        return JsonConvert.SerializeObject(internalDic[EntryType], new JsonSerializerSettings()
        {
            Converters = new[] { new ExpandoObjectConverter() }
        });
    }

    public static TypeAnnotationModel? FromJson(string jsonDefinition)
    {
        return JsonConvert.DeserializeObject<TypeAnnotationModel>(jsonDefinition, new JsonSerializerSettings()
        {
            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
            TypeNameHandling = TypeNameHandling.Auto
        });
    }

    public string AsJson()
    {
        var jsonResolver = new IgnoreSerializerContractResolver();
        jsonResolver.IgnoreProperty(typeof(Attribute), "TypeId");
        var json = JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings()
        {
            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
            TypeNameHandling = TypeNameHandling.Auto,
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = jsonResolver
        });

        return json;
    }
}