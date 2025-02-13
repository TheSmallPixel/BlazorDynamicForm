using BlazorDynamicForm.Attributes;
using Newtonsoft.Json;
using TypeAnnotationParser.Serialization;

namespace TypeAnnotationParser.Test
{
	public class UnitTest1
	{
		public class Cube
		{
			[SelectBox(["1","2"])]
			public int Value { get; set; }
		}

        public enum Color
        {
            Black,
            Yellow
        }
		public class Test
		{
			[CodeEditor("chsarp")]
			public string Name { get; set; }

			public Color Color { get; set; }
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
            var test = new Test() { Color = Color.Black };

			var json = JsonConvert.SerializeObject(test);

			 Console.WriteLine(json);
        }


        [Fact]
        public void Test2()
		{
            List<Type> attributesTypes = new List<Type>()
            {
                typeof(CodeEditorAttribute),
                typeof(SelectBoxAttribute),
            };
            var yaml = Scheme.GetYamlFromScheme<Test>(attributesTypes);
			var scheme = Scheme.GetSchemeFromYaml(attributesTypes, yaml);


            System.Console.WriteLine(yaml);
		}

       
    }
}