using System.Collections;
using System.Reflection;
using BlazorDynamicForm.Attributes.Display;
using BlazorDynamicForm.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BlazorDynamicForm;

public record ParserConfiguration
{
    public List<Type> Attributes { get; set; } = new();
    public bool Minimize { get; set; } = true;
}
public class IgnoreSerializerContractResolver : DefaultContractResolver
{
    private readonly Dictionary<Type, HashSet<string>> _ignores = new();

    public void IgnoreProperty(Type type, params string[] jsonPropertyNames)
    {
        if (!_ignores.ContainsKey(type))
            _ignores[type] = new HashSet<string>();

        foreach (var prop in jsonPropertyNames)
            _ignores[type].Add(prop);
    }

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);

        if (IsIgnored(property.DeclaringType, property.PropertyName))
        {
            property.ShouldSerialize = i => false;
            property.Ignored = true;
        }

        return property;
    }

    private bool IsIgnored(Type type, string jsonPropertyName)
    {
        if (!_ignores.ContainsKey(type))
            return false;

        return _ignores[type].Contains(jsonPropertyName);
    }
}
public static class DataAnnotationParser
{
    public static FormMap ReadDataAnnotations<T>(Action<ParserConfiguration>? getConfiguration = null)
    {
        var configData = new ParserConfiguration();
        configData.Attributes.Add(typeof(DisplayNameForm));
        configData.Attributes.Add(typeof(MultipleSelectForm));
        configData.Attributes.Add(typeof(PlaceholderForm));
        configData.Attributes.Add(typeof(DefaultValueForm));
        configData.Attributes.Add(typeof(ReadonlyForm));
        getConfiguration?.Invoke(configData);
        return ReadDataAnnotations(configData, typeof(T));
    }

    public static string ReadDataAnnotationsAsJson<T>(Action<ParserConfiguration>? getConfiguration = null)
    {
        var def = ReadDataAnnotations<T>(getConfiguration);
        var jsonResolver = new IgnoreSerializerContractResolver();
        jsonResolver.IgnoreProperty(typeof(Attribute), "TypeId");
        var json = JsonConvert.SerializeObject(def, Formatting.None, new JsonSerializerSettings()
        {
            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
            TypeNameHandling = TypeNameHandling.Auto,
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = jsonResolver
        });

        return json;
    }

    public static FormMap ReadDataAnnotations(ParserConfiguration config, Type type)
    {
        var form = new FormMap { EntryType = type.Name };
        AddFormProperty(config, form, type);
        return form;
    }

    private static string GeneratePropertyKey(MemberInfo parentProperty, PropertyInfo childProperty)
    {
        return $"#{parentProperty.Name}#{childProperty.Name}";
    }

    private static string? AddFormProperty(ParserConfiguration config, FormMap map, Type property, string? propertyName = null, string? propertyKey = null, PropertyInfo? propertyInfo = null)
    {
        var prop = new FormProperty
        {
            Name = propertyInfo != null ? propertyName : property.Name,
            Type = property.FullName ?? throw new Exception("Property fullname type is null, this is not supported!"),
            PropertyType = GetPropertyType(property)
        };
        GetAttributes(property, prop, config);
        if (propertyInfo != null)
            GetAttributesProperty(propertyInfo, prop, config);
        var effectivePropertyName = propertyKey;

        if (config.Minimize)
        {
            if ((prop.Attributes == null || !prop.Attributes.Any()) && prop.PropertyType != FormPropertyType.Collection && prop.PropertyType != FormPropertyType.Dictionary)
            {
                effectivePropertyName = property.Name;
            }
        }
        
        if (map.Properties.ContainsKey(effectivePropertyName))
        {
            return effectivePropertyName;
        }

        map.Properties.Add(effectivePropertyName, prop);

        switch (prop.PropertyType)
        {
            case FormPropertyType.Object:
            {
                prop.Properties = new Dictionary<string, string>();
                foreach (var propInfo in property.GetProperties())
                {
                    var key = GeneratePropertyKey(property, propInfo);
                    var realKey = AddFormProperty(config, map, propInfo.PropertyType, propInfo.Name, key, propInfo);
                    prop.Properties.Add(propInfo.Name, realKey);
                }

                break;
            }
            case FormPropertyType.Collection or FormPropertyType.Dictionary:
            {
                prop.Type = null;
                var elementTypes = GetBaseArrayType(property);

                for (var index = 0; index < elementTypes.Length; index++)
                {
                    prop.Properties ??= new();
                    var elementType = elementTypes[index];
                    var key = AddFormProperty(config, map, elementType, "", Guid.NewGuid().ToString());
                    prop.Properties.Add(index.ToString(), key);
                }

                break;
            }
        }

        return effectivePropertyName;
    }


    private static void GetAttributesProperty(PropertyInfo property, FormProperty formProperty, ParserConfiguration configuration)
    {
        foreach (var attributeType in configuration.Attributes)
        {
            var attributes = property.GetCustomAttributes(attributeType, false).Cast<Attribute>();
            if (attributes.Any())
            {
                formProperty.Attributes ??= new();
                formProperty.Attributes.AddRange(attributes);
            }
        }
    }

    private static void GetAttributes(Type property, FormProperty formProperty, ParserConfiguration configuration)
    {
        foreach (var attributeType in configuration.Attributes)
        {
            var attributes = property.GetCustomAttributes(attributeType, false).Cast<Attribute>();
            if (attributes.Any())
            {
                formProperty.Attributes ??= new();
                formProperty.Attributes.AddRange(attributes);
            }

        }
    }

    private static FormPropertyType GetPropertyType(Type type)
    {
        if (type.IsPrimitive || type.IsValueType || type == typeof(string) || type.IsEnum)
        {
            return FormPropertyType.Primitive;
        }
        if (typeof(IList).IsAssignableFrom(type))
        {
            return FormPropertyType.Collection;
        }
        if (typeof(IDictionary).IsAssignableFrom(type))
        {
            return FormPropertyType.Dictionary;
        }
        return FormPropertyType.Object;
    }

    private static Type[] GetBaseArrayType(Type type)
    {
        if (type.IsArray)
        {
            return new[] { type.GetElementType()! };
        }

        if (typeof(IEnumerable).IsAssignableFrom(type) || typeof(IDictionary).IsAssignableFrom(type))
        {
            if (type.IsGenericType)
            {
                return type.GetGenericArguments();
            }

            return Array.Empty<Type>();
        }

        return Array.Empty<Type>();
    }
}