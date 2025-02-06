using System.Runtime.InteropServices;
using Newtonsoft.Json.Serialization;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TypeAnnotationParser.Test
{
	public class UnitTest1
	{
		public class Cube
		{
			[SelectBox(["1","2"])]
			public int Value { get; set; }
		}
		public class Test
		{
			[CodeEditor("chsarp")]
			public string Name { get; set; }

			public Cube M1 { get; set; }
			public Cube M3 { get; set; }
			public Cube M4 { get; set; }

			public List<Cube> M2 { get; set; }

			public Test Data { get; set; }

			public List<Test> Data2 { get; set; }
		}

		[Fact]
		public void Test1()
		{
			var config = new ParserConfiguration();
			config.Attributes = new List<Annotation>(){}; 
			var parser = new TypeAnnotationParser(config);


			var scheme = parser.Parse<Test>();

			var serializerToDo = new SerializerBuilder()
				.WithNamingConvention(LowerCaseNamingConvention.Instance)
				.ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
				
				.DisableAliases();
			List<Type> attributesTypes = new List<Type>()
			{
				typeof(CodeEditorAttribute),
				typeof(SelectBoxAttribute),
			};
			foreach (var attribute in attributesTypes)
			{
				serializerToDo.WithTagMapping("!"+attribute.Name.Replace("Attribute",""), attribute);
			}


			var serializer = serializerToDo.Build();
			var yaml = serializer.Serialize(scheme);
			System.Console.WriteLine(yaml);
			File.WriteAllText("schemetest.yaml",yaml);
			var deserializerBuilder = new DeserializerBuilder()
				.WithNamingConvention(LowerCaseNamingConvention.Instance);
			foreach (var attribute in attributesTypes)
			{
				deserializerBuilder.WithTagMapping("!" + attribute.Name.Replace("Attribute", ""), attribute);
			}
			var deserializer = deserializerBuilder.Build();
			var schemeDeserialize = deserializer.Deserialize<SchemeModel>(yaml);
			System.Console.WriteLine(yaml);
		}								  
	}
}