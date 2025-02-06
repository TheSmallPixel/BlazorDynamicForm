using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TypeAnnotationParser;

public static class TypeAnnotationParserExtension
{
    public static TypeAnnotationModel Parse(Type type, Action<ParserConfiguration>? getConfiguration = null)
    {
        var configData = new ParserConfiguration();
        getConfiguration?.Invoke(configData);
        var tap = new TypeAnnotationParser(configData);
        return tap.Parse(type);
    }

    public static TypeAnnotationModel Parse<T>(Action<ParserConfiguration>? getConfiguration = null)
    {
        var configData = new ParserConfiguration();
        getConfiguration?.Invoke(configData);
        var tap = new TypeAnnotationParser(configData);
        return tap.Parse<T>();
    }

    public static string Serialize(this ExpandoObject data)
    {
        return JsonConvert.SerializeObject(data, new JsonSerializerSettings()
        {
            Converters = new[] { new ExpandoObjectConverter() }
        });
    }

   
   
}