using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TypeAnnotationParser;

public record TypeAnnotationProperty
{
    public string Name { get; set; }
    public string? Type { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public PropertyType PropertyType { get; set; }
    public List<Attribute>? Attributes { get; set; }
    public Dictionary<string, string>? Properties { get; set; }
}