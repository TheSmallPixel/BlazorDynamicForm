using System.Collections;
using System.Reflection;
using BlazorDynamicForm.Attributes.Display;
using BlazorDynamicForm.Attributes.Validation;
using BlazorDynamicForm.Entities;

namespace BlazorDynamicForm;

public static class DataAnnotationParser
{
    public static FormMap? ReadDataAnnotations<T>()
    {
        return ReadDataAnnotations(typeof(T));
    }

    public static FormMap ReadDataAnnotations(Type type)
    {
        var form = new FormMap { EntryType = type.Name };
        AddFormPropertyToMap(form, type);
        return form;
    }

    private static string DetermineEffectivePropertyName(string? propertyName, Type property)
    {
        return string.IsNullOrWhiteSpace(propertyName) ? property.Name : propertyName;
    }

    private static FormProperty CreateFormProperty(string? propertyName, Type property, List<ValidationRule> headRules, List<DisplayForm> headAttributes, List<CustomFormAttribute> customAttributes)
    {
        return new FormProperty
        {
            TypeFullName = property.FullName,
            TypeName = property.Name,
            PropertyName = propertyName,
            PropertyType = GetPropertyType(property),
            DisplayRules = MergeAttributes(property, headAttributes),
            ValidationRules = MergeAttributes(property, headRules),
            CustomFormAttributes = MergeAttributes(property, customAttributes)
        };
    }

    private static bool ShouldSkipPropertyAddition(FormProperty prop)
    {
        return prop.DisplayRules.Count == 0 && prop.ValidationRules.Count == 0 && prop.PropertyType == FormPropertyType.Primitive;
    }

    private static void PopulatePropertyStructure(FormMap map, Type property, FormProperty prop)
    {
        switch (prop.PropertyType)
        {
            case FormPropertyType.Object:
                HandleObjectProperty(map, property, prop);
                break;
            case FormPropertyType.Collection:
                HandleCollectionProperty(map, property, prop);
                break;
        }
    }

    private static void HandleObjectProperty(FormMap map, Type property, FormProperty prop)
    {
        prop.Structure = new Dictionary<string, string>();
        foreach (var propInfo in property.GetProperties())
        {
            var displayForms = propInfo.GetCustomAttributes(typeof(DisplayForm), false).Cast<DisplayForm>().ToList();
            var validationRules = propInfo.GetCustomAttributes(typeof(ValidationRule), false).Cast<ValidationRule>().ToList();
            var customAttributes = propInfo.GetCustomAttributes(typeof(CustomFormAttribute), false).Cast<CustomFormAttribute>().ToList();
            var cond = displayForms.Count == 0 && validationRules.Count == 0 && customAttributes.Count == 0;
            var key = GeneratePropertyKey(property, propInfo, cond);
            prop.Structure.Add(propInfo.Name, key);

            AddFormPropertyToMap(map, propInfo.PropertyType, propInfo.Name, key, validationRules, displayForms, customAttributes);
        }
    }

    private static string GeneratePropertyKey(MemberInfo parentProperty, PropertyInfo childProperty, bool cond)
    {
        if (GetPropertyType(childProperty.PropertyType) == FormPropertyType.Collection)
        {
            return $"#{parentProperty.Name}#{childProperty.Name}";
        }

        return cond ? childProperty.PropertyType.Name : $"#{parentProperty.Name}#{childProperty.Name}";
    }


    private static void AddFormPropertyToMap(FormMap map, Type property, string? propertyName = null, string? propertyKey = null, List<ValidationRule> headRules = null, List<DisplayForm> headAttributes = null, List<CustomFormAttribute> customAttributes = null)
    {
        var effectivePropertyName = DetermineEffectivePropertyName(propertyKey, property);
        if (map.Properties.ContainsKey(effectivePropertyName))
        {
            return;
        }

        var prop = CreateFormProperty(propertyName, property, headRules, headAttributes, customAttributes);

        if (ShouldSkipPropertyAddition(prop))
        {
            return;
        }

        map.Properties.Add(effectivePropertyName, prop);
        PopulatePropertyStructure(map, property, prop);
    }

    private static void HandleCollectionProperty(FormMap map, Type property, FormProperty prop)
    {
        prop.Structure = new Dictionary<string, string>();
        prop.TypeName = GetTypeCategory(property);
        prop.TypeFullName = string.Empty;
        var elementTypes = GetBaseArrayType(property);

        for (var index = 0; index < elementTypes.Length; index++)
        {
            var elementType = elementTypes[index];
            var elementTypeCategory = GetPropertyType(elementType);

            if (elementTypeCategory != FormPropertyType.Collection)
            {
                prop.Structure.Add(index.ToString(), elementType.Name);
                AddFormPropertyToMap(map, elementType, elementType.Name);
            }
            else
            {
                var key = Guid.NewGuid().ToString();
                prop.Structure.Add(index.ToString(), key);
                AddFormPropertyToMap(map, elementType, key);
            }
        }
    }

    private static List<T> MergeAttributes<T>(Type property, List<T>? l2) where T : Attribute
    {
        var l1 = property.GetCustomAttributes(typeof(T), false).Cast<T>().ToList();
        if (l1 == null)
        {
            return l2 ?? new List<T>();
        }

        if (l2 == null)
        {
            return l1;
        }

        l2.AddRange(l1.Where(item => l2.TrueForAll(x => x.GetType().FullName != item.GetType().FullName)));
        return l2;
    }

    private static FormPropertyType GetPropertyType(Type type)
    {
        if (type.IsPrimitive || type.IsValueType || type == typeof(string) || type.IsEnum)
        {
            return FormPropertyType.Primitive;
        }

        if (typeof(IDictionary).IsAssignableFrom(type) || typeof(IEnumerable).IsAssignableFrom(type))
        {
            return FormPropertyType.Collection;
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

    private static string GetTypeCategory(Type type)
    {
        if (type.IsGenericType)
        {
            if (type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                return "Dictionary";
            }

            if (type.GetGenericTypeDefinition() == typeof(List<>))
            {
                return "List";
            }
        }

        if (typeof(IDictionary).IsAssignableFrom(type))
        {
            return "Dictionary";
        }

        if (typeof(IList).IsAssignableFrom(type))
        {
            return "List";
        }

        return "Other";
    }
}