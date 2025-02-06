using System.Collections;
using System.Reflection;

namespace TypeAnnotationParser;

public class TypeAnnotationParser(ParserConfiguration configuration)
{
    public TypeAnnotationModel Parse<T>()
    {
        var type = typeof(T);
        var form = new TypeAnnotationModel(type.Name);
        AddFormProperty(form, type, type.Name, type.Name);
        return form;
    }

    public TypeAnnotationModel Parse(Type type)
    {
        var form = new TypeAnnotationModel(type.Name);
        AddFormProperty(form, type, type.Name, type.Name);
        return form;
    }

    private string GetUniquePropertyKey(Type parentType, PropertyInfo childProperty)
    {
        return $"#{parentType.Name}#{childProperty.Name}";
    }

    private string AddFormProperty(TypeAnnotationModel typeAnnotationModel, Type propertyType, string? propertyName = null, string? propertyKey = null, PropertyInfo? propertyInfo = null)
    {
        if (typeAnnotationModel.Properties.ContainsKey(propertyKey))
            return propertyKey;

        var prop = new TypeAnnotationProperty
        {
            Name = propertyInfo != null ? propertyName : propertyType.Name,
            Type = propertyType.FullName ?? throw new InvalidOperationException("Property type must have a full name."),
            PropertyType = DeterminePropertyType(propertyType)
        };

        AssignAttributesToProperty(propertyType, prop);
        if (propertyInfo != null)
            AssignAttributesToProperty(propertyInfo, prop);

        typeAnnotationModel.Properties.Add(propertyKey, prop);
        ProcessPropertyType(typeAnnotationModel, propertyType, prop);
        return propertyKey;
    }

    private void ProcessPropertyType(TypeAnnotationModel typeAnnotationModel, Type propertyType, TypeAnnotationProperty prop)
    {
        switch (prop.PropertyType)
        {
            case PropertyType.Object:
                {
                    prop.Properties = new();
                    foreach (var propInfo in propertyType.GetProperties())
                    {
                        var key = GetUniquePropertyKey(propertyType, propInfo);
                        var realKey = AddFormProperty(typeAnnotationModel, propInfo.PropertyType, propInfo.Name, key, propInfo);
                        prop.Properties.Add(propInfo.Name, realKey);
                    }
                    break;
                }
            case PropertyType.Collection or PropertyType.Dictionary:
                {
                    prop.Type = null;
                    var elementTypes = GetBaseArrayType(propertyType);
                    for (var index = 0; index < elementTypes.Length; index++)
                    {
                        prop.Properties ??= new();
                        var elementType = elementTypes[index];
                        var key = AddFormProperty(typeAnnotationModel, elementType, "", Guid.NewGuid().ToString());
                        prop.Properties.Add(index.ToString(), key);
                    }
                    break;
                }
            default:
                break;
        }
    }

    private void AssignAttributesToProperty(MemberInfo property, TypeAnnotationProperty typeAnnotationProperty)
    {
        foreach (var annotation in configuration.Attributes)
        {
            AddAttributeToProperty(annotation.Type, property, typeAnnotationProperty, annotation.Inherit);
        }
    }

    private void AddAttributeToProperty(Type attrType, ICustomAttributeProvider property, TypeAnnotationProperty typeAnnotationProperty, bool inherit)
    {
        var attributes = property.GetCustomAttributes(attrType, inherit).Cast<Attribute>().ToList();
        if (attributes.Any())
        {
            typeAnnotationProperty.Attributes ??= [];
            typeAnnotationProperty.Attributes.AddRange(attributes);
        }
    }

    private PropertyType DeterminePropertyType(Type type)
    {
        if (type.IsPrimitive || type.IsValueType || type == typeof(string) || type.IsEnum)
        {
            return PropertyType.Primitive;
        }
        if (typeof(IList).IsAssignableFrom(type))
        {
            return PropertyType.Collection;
        }
        if (typeof(IDictionary).IsAssignableFrom(type))
        {
            return PropertyType.Dictionary;
        }
        return PropertyType.Object;
    }

    private Type[] GetBaseArrayType(Type type)
    {
        if (type.IsArray)
        {
            return [type.GetElementType()!];
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