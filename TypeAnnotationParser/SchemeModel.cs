using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Dynamic;

namespace TypeAnnotationParser;

public record SchemeModel : SchemeProperty
{
	public Dictionary<string, SchemeProperty> References { get; set; } = new();

}