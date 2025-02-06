using TypeAnnotationParser;

namespace BlazorDynamicForm
{
    public static class ObjectGenerator
    {
        public static object? CreateObject(this SchemeModel definition, Action<ObjectGeneratorOptions>? options = null)
        {
            return CreateObject(definition, options);
        }
        public static object? CreateObject(this SchemeModel definition, SchemeProperty key, Action<ObjectGeneratorOptions> options = null)
        {
            var optionData = new ObjectGeneratorOptions();
            options?.Invoke(optionData);
            return CreateObject(definition, optionData, key);
        }
        public static object? CreateObject(this SchemeModel definition, ObjectGeneratorOptions options, SchemeProperty prop, int depth = 0, bool canBeNull = true)
        {
      

            if (depth >= options.MaxRecursiveDepth)
                return null;
            depth++;


            if (!string.IsNullOrEmpty(prop.Ref))
            {
	            //goto ref

            }
            else
            {
                //read
            }

            switch (prop.Type)
            {
                case PropertyType.Integer:
	                return 0;
                case PropertyType.Double:
	                return 0d;
                case PropertyType.Float:
	                return 0f;
                case PropertyType.String:
	                return string.Empty;
				case PropertyType.Object:
                    var objectData = new Dictionary<string, object>();
                    if (options.CreateObjectElement || depth < 2 && !options.CreateObjectElement)
                    {
                        foreach (var property in prop.Properties)
                        {
                            objectData.Add(property.Key, CreateObject(definition, options, property.Value, depth));
                        }
                    }
                    return objectData;
                case PropertyType.Array:
                    var collectionData = new List<object>();
                    if (options.CreateCollectionElement)
                    {
                       // var keyType = prop.Properties.First().Value;
                       // collectionData.Add(CreateObject(definition, options, keyType, depth));
                    }

                    return collectionData;
                case PropertyType.Dictionary:
                    var dictionaryData = new Dictionary<string, object>();
                    if (options.CreateDictionaryElement)
                    {
                        //var dicKey = prop.Properties.First().Value;
                        var dicValue = prop.Properties.Last().Value;
                        //var dickObjectKey = CreateObject(definition, options, dicKey, depth, false);
                        var dickObjectValue = CreateObject(definition, options, dicValue, depth);
                        if (dickObjectValue == null)
                        {
                            //this happen on max recursive lenght reaced point, just avoid to add anything
                            //throw new Exception($"the dictionary prop cannot be null, {dicKey}");
                        }
                        else
                        {
                           // dictionaryData.Add(Guid.NewGuid().ToString(), dickObjectValue);
                        }
                    }

                    return dictionaryData;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        
    }
}
