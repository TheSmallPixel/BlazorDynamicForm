using Newtonsoft.Json.Linq;
using TypeAnnotationParser;

namespace BlazorDynamicForm
{
    public static class ObjectGenerator
    {
        public static object? CreateOrValidateData(this SchemeModel definition, SchemeProperty prop, object? data, ObjectGeneratorOptions? options = null, int depth = 0)
        {
            options ??= new ObjectGeneratorOptions();

            // Avoid infinite recursion
            if (depth >= options.MaxRecursiveDepth)
                return null;


            depth++;

            // If the property references something else, delegate to that
            if (!string.IsNullOrEmpty(prop.Ref))
            {
                if (!definition.References.TryGetValue(prop.Ref, out var refProp))
                    throw new InvalidOperationException($"Reference '{prop.Ref}' not found in definition.");

                return definition.CreateOrValidateData(refProp, data, options, depth);
            }
            // If no 'Type' is specified, we can't validate
            if (prop.Type == null)
                throw new InvalidOperationException($"Type not specified in definition.");


            switch (prop.Type.Value)
            {
                case PropertyType.Boolean:
	                return data is bool ? data : false;
                case PropertyType.Integer:
                    // If 'data' is an integer, keep it; otherwise set default
                    return data is int ? data : 0;

                case PropertyType.Double:
                    return data is double ? data : 0.0;

                case PropertyType.Float:
                    return data is float ? data : 0f;

                case PropertyType.Decimal:
                    return data is decimal ? data : 0d;

                case PropertyType.Enum:
                    return data is int ? data : 0;

                case PropertyType.String:
                    // Force empty string if not a string
                    return data is string s ? s : string.Empty;

                case PropertyType.Object:
                    return EnsureObject(definition, prop, data, options, depth);

                case PropertyType.Array:
                    return EnsureArray(definition, prop, data, options, depth);

                case PropertyType.Dictionary:
                    return EnsureDictionary(definition, prop, data, options, depth);

                default:
                    throw new ArgumentOutOfRangeException($"Unsupported property type: {prop.Type}");
            }
        }
        /// <summary>
        /// Ensure 'data' is a Dictionary for an Object schema, 
        /// and for each sub-property, validate or create data.
        /// </summary>
        private static object EnsureObject(
            SchemeModel definition,
            SchemeProperty prop,
            object? data,
            ObjectGeneratorOptions options,
            int depth
        )
        {
            // Make sure we have a dictionary
            var dict = data as IDictionary<string, object?>;
          
            if (dict == null)
            {
                dict = new Dictionary<string, object?>();
            }

            // If schema has sub-properties, ensure each is validated/created
            if (prop.Properties != null && depth < 10)
            {
                foreach (var (key, subProp) in prop.Properties)
                {
                    object? existing = dict.TryGetValue(key, out var value) ? value : null;
                    dict[key] = definition.CreateOrValidateData(subProp, existing, options, depth);
                }

                // Optionally remove extra fields not in the schema
                // if you want to disallow unknown properties:
                var keysToRemove = dict.Keys.Except(prop.Properties.Keys).ToList();
                foreach (var extraKey in keysToRemove)
                {
                    dict.Remove(extraKey);
                }
            }

            return dict;
        }
        /// <summary>
        /// Ensure 'data' is a List for an Array schema. 
        /// Optionally create an initial item if CreateCollectionElement is true.
        /// </summary>
        private static object EnsureArray(
            SchemeModel definition,
            SchemeProperty prop,
            object? data,
            ObjectGeneratorOptions options,
            int depth
        )
        {
            var list = data as List<object?>;
         
			if (list == null)
            {
                list = new List<object?>();
            }

            // If you want to automatically create an element,
            // check if the schema has Indices or something describing the item type.
            // This snippet does not handle item-level schema because
            // the user can have multiple Indices or subprops. 
            // Adapt as necessary.

            if (list.Count == 0 && options.CreateCollectionElement && prop.Indices != null && prop.Indices.Count > 0)
            {
                // Example: create one item based on the first index
                var firstItemProp = prop.Indices[0];
                var itemData = definition.CreateOrValidateData(firstItemProp, null, options, depth);
                if (itemData != null)
                {
                    list.Add(itemData);
                }
            }
            else
            {
                // Optionally validate each existing item in the list if you know the item schema
                // For example, if there's exactly one Indices item, you might do:

                if (prop.Indices != null && prop.Indices.Count == 1)
                {
                    var itemProp = prop.Indices[0];
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i] = definition.CreateOrValidateData(itemProp, list[i], options, depth);
                    }
                }

            }

            return list;
        }

        /// <summary>
        /// Ensure 'data' is a Dictionary for a Dictionary schema. 
        /// The schema typically has 'Properties' describing the key/value types.
        /// </summary>
        private static object EnsureDictionary(
            SchemeModel definition,
            SchemeProperty prop,
            object? data,
            ObjectGeneratorOptions options,
            int depth
        )
        {
            // We expect a dictionary of <string, object?>
            var dict = data as Dictionary<string, object?>;
           

            if (dict is null && data is JObject jobj)
            {
	            dict  = jobj.ToObject<Dictionary<string, object>>();

			}

            if (dict == null)
            {
	            dict = new Dictionary<string, object?>();
            }

			// If you want to auto-create one key-value pair
			// if none exists and options.CreateDictionaryElement == true:
			if (options.CreateDictionaryElement && dict.Count == 0 && prop.Properties != null && prop.Properties.Count == 2)
            {
                // Typically for dictionary, you have something like:
                //   prop.Properties["key"] = <key type>
                //   prop.Properties["value"] = <value type>
                // This is an example approach; your real schema might differ.

                var dicKeyProp = prop.Properties["key"];
                var dicValueProp = prop.Properties["value"];

                //object? newKey   = definition.CreateOrValidateData(dicKeyProp, null, options, depth);
                object? newValue = definition.CreateOrValidateData(dicValueProp, null, options, depth);

                dict[Guid.NewGuid().ToString()] = newValue;

            }

            // If you want to validate each existing key-value pair by the "value" type
            // you'd do something like:

            if (prop.Properties != null && prop.Properties.TryGetValue("value", out var valueProp))
            {
                var keys = dict.Keys.ToList();
                foreach (var k in keys)
                {
                    dict[k] = definition.CreateOrValidateData(valueProp, dict[k], options, depth);
                }
            }


            return dict;
        }
    }
}
