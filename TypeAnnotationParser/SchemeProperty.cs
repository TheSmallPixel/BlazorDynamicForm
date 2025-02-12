using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using YamlDotNet.Serialization;

namespace TypeAnnotationParser;

public record SchemeProperty
{
	public string? Ref { get; set; }
	public string? Name { get; set; }
	// public string? Type { get; set; }
	[JsonConverter(typeof(StringEnumConverter))]
	public PropertyType? Type { get; set; } = null;

	public List<AttributeScheme>? Attributes { get; set; } = null;
	public Dictionary<string, SchemeProperty>? Properties { get; set; } = null;

	public List<SchemeProperty>? Indices { get; set; } = null;
}

public class AttributeScheme : Attribute
{
	[YamlIgnore]
	public override object TypeId => GetType();
}
public abstract class DynamicRendererComponent : AttributeScheme { }

public class SelectBoxAttribute : DynamicRendererComponent
{
	public SelectBoxAttribute(string[] choices)
	{
		Options = choices;
	}
	public SelectBoxAttribute(){}
	public string[] Options { get; set; }
}