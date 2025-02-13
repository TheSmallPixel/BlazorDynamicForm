using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;

namespace TypeAnnotationParser.Serialization
{
    public static class Scheme
    {
        public static SchemeModel? GetSchemeFromYaml(List<Type> attributesTypes, string yaml)
        {
            var deserializerBuilder = new DeserializerBuilder()
                .WithNamingConvention(LowerCaseNamingConvention.Instance);
            foreach (var attribute in attributesTypes)
            {
                deserializerBuilder.WithTagMapping("!" + attribute.Name.Replace("Attribute", ""), attribute);
            }
            var deserializer = deserializerBuilder.Build();
            var schemeDeserialize = deserializer.Deserialize<SchemeModel>(yaml);
            return schemeDeserialize;
        }

        public static string GetYamlFromScheme<T>(List<Type> attributesToSerialize)
        {
            return GetYamlFromScheme(typeof(T), attributesToSerialize);
        }

        public static string GetYamlFromScheme(Type type, List<Type> attributesToSerialize)
        {
            var config = new ParserConfiguration();
            config.Attributes = new List<Annotation>();
            var parser = new TypeAnnotationParser(config);

            var scheme = parser.Parse(type);

            var serializerToDo = new SerializerBuilder()
                .WithNamingConvention(LowerCaseNamingConvention.Instance)
                .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
                .DisableAliases();

            foreach (var attribute in attributesToSerialize)
            {
                serializerToDo.WithTagMapping("!" + attribute.Name.Replace("Attribute", ""), attribute);
            }

            var serializer = serializerToDo.Build();
            var yaml = serializer.Serialize(scheme);
            return yaml;
        }
    }
}
