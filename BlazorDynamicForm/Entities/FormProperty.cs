using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BlazorDynamicForm.Entities;

public class FormProperty
{
    public string Name { get; set; }
    public string? Type { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public FormPropertyType PropertyType { get; set; }
    public List<Attribute>? Attributes { get; set; }
    public Dictionary<string, string>? Properties { get; set; }
}