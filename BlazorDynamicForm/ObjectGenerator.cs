using BlazorDynamicForm.Entities;

namespace BlazorDynamicForm
{
    public record ObjectGeneratorOptions
    {
        public int MaxRecursiveDepth { get; set; } = 10;
        public bool InitStringsEmpty { get; set; } = true;

        public bool CreateCollectionElement { get; set; } = false;
        public bool CreateDictionaryElement { get; set; } = false;
        public bool CreateObjectElement { get; set; } = false;
    }
    public static class ObjectGenerator
    {
        public static object? CreateObject(this FormMap definition, Action<ObjectGeneratorOptions>? options = null)
        {
            return CreateObject(definition, definition.EntryType, options);
        }
        public static object? CreateObject(this FormMap definition, string key, Action<ObjectGeneratorOptions> options = null)
        {
            var optionData = new ObjectGeneratorOptions();
            options?.Invoke(optionData);
            return CreateObject(definition, optionData, key);
        }
        public static object? CreateObject(this FormMap definition, ObjectGeneratorOptions options, string key, int depth = 0, bool canBeNull = true)
        {
            if (!definition.Properties.ContainsKey(key))
            {
                throw new Exception($"Missing definition for this {key}");
            }

            if (depth >= options.MaxRecursiveDepth)
                return null;
            depth++;

            var prop = definition.Properties[key];
            switch (prop.PropertyType)
            {
                case FormPropertyType.Primitive:
                    return GetDefaultType(prop, options, canBeNull);
                case FormPropertyType.Object:
                    var objectData = new Dictionary<string, object>();
                    if (options.CreateObjectElement || depth < 2 && !options.CreateObjectElement)
                    {
                        foreach (var property in prop.Properties)
                        {
                            objectData.Add(property.Key, CreateObject(definition, options, property.Value, depth));
                        }
                    }
                    return objectData;
                case FormPropertyType.Collection:
                    var collectionData = new List<object>();
                    if (options.CreateCollectionElement)
                    {
                        var keyType = prop.Properties.First().Value;
                        collectionData.Add(CreateObject(definition, options, keyType, depth));
                    }

                    return collectionData;
                case FormPropertyType.Dictionary:
                    var dictionaryData = new Dictionary<object, object>();
                    if (options.CreateDictionaryElement)
                    {
                        var dicKey = prop.Properties.First().Value;
                        var dicValue = prop.Properties.Last().Value;
                        var dickObjectKey = CreateObject(definition, options, dicKey, depth, false);
                        var dickObjectValue = CreateObject(definition, options, dicValue, depth);
                        if (dickObjectKey == null)
                        {
                            //this happen on max recursive lenght reaced point, just avoid to add anything
                            //throw new Exception($"the dictionary key cannot be null, {dicKey}");
                        }
                        else
                        {
                            dictionaryData.Add(dickObjectKey, dickObjectValue);
                        }
                    }

                    return dictionaryData;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static object? GetDefaultType(FormProperty prop, ObjectGeneratorOptions options, bool canBeNull)
        {
            var type = Type.GetType(prop.Type);
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            if (!canBeNull && type == typeof(string))
            {
                return Guid.NewGuid().ToString();
            }
            if (options.InitStringsEmpty && type == typeof(string))
            {
                return string.Empty;
            }
            return null;
        }
    }
}
